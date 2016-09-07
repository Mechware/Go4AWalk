using UnityEngine;
using System.Collections;

#region Queststruct
public struct quest {
    public string name;
    public int goldReward;
    public int xpReward;
    public float timeToComplete;
    public float distance;
    public float difficulty;
    public System.DateTime endTime;
    public float distanceProgress;
    public bool active;

    public quest(string name, int goldReward, int xpReward, float timeToComplete, float distance, float difficulty) {
        this.name = name;
        this.goldReward = goldReward;
        this.xpReward = xpReward;
        this.timeToComplete = timeToComplete;
        this.distance = distance;
        this.difficulty = difficulty;
        this.distanceProgress = 0;
        active = true;
        // If quest has no time limit, make time limit end of time
        if(timeToComplete == -1) {
            this.endTime = System.DateTime.MaxValue;
        } else {
            this.endTime = System.DateTime.UtcNow.AddSeconds(timeToComplete);
        }
    }

    public string toString() {

        if (!active)
            return "No active quest";

        string s = "";
        s += "Name: " + name + "\n";
        s += "Gold Reward: " + goldReward + "\n";
        s += "Xp Reward: " + xpReward + "\n";
        
        if(timeToComplete != -1) {
            s += "Quest time length: " + string.Format("{0:0.00}", timeToComplete/60f) + " minutes\n";
        } else {
            s += "Quest time length: Unlimited\n";
        }

        double timeLeft = (endTime - System.DateTime.UtcNow).TotalMinutes;
        if(timeLeft < 5) {
            s += "Time left: " + string.Format("{0:0.00}", timeLeft*60) + " seconds\n";
        } else if (timeLeft > 1000000) {
            s += "Time left: Unlimited\n";
        }
        
        s += "Quest distance: " + distance + " meters\n";
        s += "Distance Travelled: " + distanceProgress + " meters\n";
        return s;
    }
}
#endregion
public class Player : MonoBehaviour {

    #region nonstatic
    void Awake() {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("FightingScene")) {
            print("Fighting");
            fighting = true;
            walking = false;
            inTown = false;
        } else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("TownScreen")) {
            print("In town");
            fighting = false;
            walking = false;
            inTown = true;
        } else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("WalkingScreen")) {
            print("Walking");
            fighting = false;
            walking = true;
            inTown = false;
        } else {
            print("Unexpected scene was loaded");
        }

        thisGo = this;

        if(currentQuest.active) {
            StopCoroutine("checkQuestEnd");
            StartCoroutine("checkQuestEnd");
        }
    }

    void Update() {
        
    }

    public static int waitTime = 5;
    IEnumerator checkQuestEnd() {
        while (currentQuest.endTime > System.DateTime.UtcNow) {
            yield return new WaitForSeconds(waitTime);
        }
        endQuest(false);
    }

    #endregion

    #region static

    // General variables
    public static bool fighting = false, walking = false, inTown = false;
    public static Player thisGo;



    #region combat
    public static int maxHealth = 100;
    public static int health = 100;
    public static int attackStrength = 5;
    public static int critFactor = 4;
    public static int crit = 0;

    public static void damage(int amount) {
        health -= amount;
        if(health <= 0) {
            combatDie();
        }
    }

    public static int updateCrit(int randFactor) {

        if (crit == 100) return 100;
        
        // Updates crit and returns updated value
        float rand = Random.Range(0.0f, 1.0f);

        if (rand+0.30f > 1f/16000f * (float)(crit*crit)) {
            crit += critFactor + randFactor;
            if (crit > 100) crit = 100;
        } else {
            crit = 0;
        }

        return crit;
    }

    public static void resetCrit() {
        crit = 0;
    }

    public static int getCrit() {
        return crit;
    }

    private static void combatDie() {
        
    }

    public static int getRegularAttack() {
        float randFactor = Random.Range(-1.0f, 1.0f);
        float fAttackStrength = (float) attackStrength;

        // Update crit to some value
        updateCrit(Mathf.RoundToInt(randFactor));

        return attackStrength + Mathf.RoundToInt(randFactor * fAttackStrength); ;
    }

    public static int getSwipeAttack(Vector2 swipe) {

        //Debug.Log("Swipe length: " + swipe.magnitude);
        //Debug.Log("Swipe angle: " + Mathf.Atan2(swipe.y, swipe.x));

        float fAttack;
        int attack;
        int regAttack = getRegularAttack();

        while(regAttack < attackStrength) {
            regAttack = getRegularAttack();
        }

        if (crit != 100) {
            fAttack = (((float) crit)/10f)*regAttack;
            attack = Mathf.RoundToInt(fAttack);
        } else {
            attack = (int) (15f * regAttack);
        }

        // Reset crit to 0
        resetCrit();

        return attack;
    }
    #endregion 
    #region Walking and questing

    private static float lastTimeGettingDistance;
    public static float totalDistance;
    public static int gold, experience, level;

    public static void updateDistance(float deltaDistance) {
        giveExperience(Mathf.RoundToInt(deltaDistance));
        totalDistance += deltaDistance;
        currentQuest.distanceProgress += deltaDistance;
        if(currentQuest.distanceProgress >= currentQuest.distance) {
            endQuest(true);
        }
    }

    // Questing
    public static quest currentQuest;

    /**
    *   distance: Distance in meters until the end of the quest
    *   time: Time to complete the quest in minutes
    *   goldReward: Amount of gold rewarded for completion
    */
    public static void startQuest(quest q) {
        currentQuest = q;
        if(q.timeToComplete != -1) {
            thisGo.StartCoroutine("checkQuestEnd");
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("WalkingScreen");
    }

    public static void endQuest(bool userFinished) {

        if (userFinished) {
            print("Quest passed!");
            giveGold(currentQuest.goldReward);
            giveExperience(currentQuest.xpReward);
        } else {
            print("Quest failed!");
        }

        currentQuest.active = false;

        UnityEngine.SceneManagement.SceneManager.LoadScene("TownScreen");
    }

    #endregion
    #region gettersAndSetters
    public static void giveGold(int amount) {
        gold += amount;
    }


    public static int lastLevel = 0;
    public static void giveExperience(int amount) {
        experience += amount;
        if(experience > lastLevel + 100) {
            level++;
            lastLevel = experience;
        }
    }
    #endregion
    #endregion

}
