using UnityEngine;
using System.Collections;


#region Queststruct
public struct quest {
    public string name;
    public string shortOverview;
    public string description;
    public int goldReward;
    public int xpReward;
    public GameObject[] rewards;
    public float timeToComplete;
    public float distance;
    public float difficulty;
    public System.DateTime endTime;
    public float distanceProgress;
    public bool active;

    public quest(string name, string shortOverview, string description, int goldReward, int xpReward, GameObject[] rewards, float timeToComplete, float distance, float difficulty) {
        UnityEngine.Assertions.Assert.AreNotEqual(0, distance, "Quest has a distance of 0.");
        this.name = name;
        this.shortOverview = shortOverview;
        this.description = description;
        this.goldReward = goldReward;
        this.xpReward = xpReward;
        this.rewards = rewards;
        this.timeToComplete = timeToComplete;
        this.distance = distance;
        this.difficulty = difficulty;

        distanceProgress = 0;
        active = true;

        // If quest has no time limit, make time limit end of time
        if (timeToComplete == -1) {
            endTime = System.DateTime.MaxValue;
        } else {
            endTime = System.DateTime.UtcNow.AddSeconds(timeToComplete);
        }
    }

    public string getStats() {

        if (!active)
            return "No active quest";

        string s = "";
        s += "Name: " + name + "\n";
        s += "Gold Reward: " + goldReward + "\n";
        s += "Xp Reward: " + xpReward + "\n";

        if (timeToComplete != -1) {
            s += "Quest time length: " + string.Format("{0:0.00}", timeToComplete / 60f) + " minutes\n";
        } else {
            s += "Quest time length: Unlimited\n";
        }

        double timeLeft = (endTime - System.DateTime.UtcNow).TotalMinutes;
        if (timeLeft < 5) {
            s += "Time left: " + string.Format("{0:0.00}", timeLeft * 60) + " seconds\n";
        } else if (timeLeft > 1000000) {
            s += "Time left: Unlimited\n";
        }

        if (distance == -1) {
            s += "Quest distance: In Forest\n";
        } else {
            s += "Quest distance: " + distance + " meters\n";
        }

        s += "Distance Travelled: " + distanceProgress + " meters\n";
        return s;
    }

    public string getButtonText() {
        string s = name + "\n" +
                   "Distance: " + distance + " m\n" +
                   "Time: " + timeToComplete + " s\n";
        return s;
    }
}
#endregion

public class Questing : MonoBehaviour {


    // Questing
    public static quest currentQuest;
    public static Questing instance;
    public EnemyWatchdog ew;

    // Use this for initialization
    void Awake() {
        if (currentQuest.active) {
            // Stop it to make sure it's not running twice
            StopCoroutine("checkQuestEnd");
            StartCoroutine("checkQuestEnd");
        }
        instance = this;
    }

    void Start() {
        if(currentQuest.active == false && GameState.walking) {
            StoryOverlord.startQuest(StoryOverlord.currentLevel);
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public static int waitTime = 5;
    IEnumerator checkQuestEnd() {
        while (currentQuest.endTime > System.DateTime.UtcNow) {
            yield return new WaitForSeconds(waitTime);
        }

        print("Didn't finish quest in time");
        endQuest(false);
    }

    public static void startQuest(quest q) {
        currentQuest = q;
        if (q.timeToComplete != -1) {
            instance.StartCoroutine("checkQuestEnd");
        }
        DialoguePopUp.instance.showDialog(StoryOverlord.questStartDialogue, StoryOverlord.characterNameStart, StoryOverlord.characterSpriteStart, () => { });
    }

    public static void endQuest(bool userFinished) {

        if (userFinished) {
            print("Quest passed!");
            Player.giveGold(currentQuest.goldReward);
            Player.giveExperience(currentQuest.xpReward);
            Inventory.items.Add(StoryOverlord.reward);

            Inventory.items.Add(ItemList.itemMasterList[ItemList.IRON_SWORD]);
            Inventory.items.Add(ItemList.itemMasterList[ItemList.STEEL_SWORD]);
            Inventory.items.Add(ItemList.itemMasterList[ItemList.ORICH_SWORD]);
            Inventory.items.Add(ItemList.itemMasterList[ItemList.SAND_SWORD]);
            Inventory.items.Add(ItemList.itemMasterList[ItemList.JADE_SWORD]);
            Inventory.items.Add(ItemList.itemMasterList[ItemList.OBSIDIAN_SWORD]);
            print(Inventory.getInventory());
            DialoguePopUp.instance.showDialog(StoryOverlord.questEndDialogue, StoryOverlord.characterNameEnd, StoryOverlord.characterSpriteEnd, () => {
            PopUp.instance.showPopUp("QUEST COMPLETE! \n \n" + "Continue on your journey." + "\n\n",
                new string[] { "Continue"},
                new System.Action[] {
                    new System.Action(() => {
                        StoryOverlord.currentLevel++;
                        currentQuest.active = false;
                        GameState.loadScene(GameState.scene.WALKING_LEVEL);                        
                        }),
                     });
            });
        
        } else {
            print("Quest failed!");
            Player.distance.Value -= 100; // Sets the player back 100m.
            PopUp.instance.showPopUp("QUEST FAILED! \nOh no you were defeated! \n" + "You run away and returned to camp." + "\n\n",
                new string[] { "Continue" },
                new System.Action[] {
                    new System.Action(() => {GameState.loadScene(GameState.scene.CAMPSITE); }) });
        }           
        
    }


    public static void makeCamp() {
        GameState.loadScene(GameState.scene.CAMPSITE);
    }

    public static void move(float distance) {
        currentQuest.distanceProgress += distance;

        if (currentQuest.distanceProgress >= currentQuest.distance) {
            if (currentQuest.distance != -1) {
                currentQuest.distanceProgress = currentQuest.distance;
                // PopUp.instance.showPopUp("A BOSS HAS APPEARED!",
                //     new string[] { "FIGHT" },
                //    new System.Action[] { EnemyWatchdog.instance.startBossFight() }
                //);
                EnemyWatchdog.instance.startBossFight();

                //endQuest(true);
            }
        }
    }
   
    public static quest createRandomQuest() {

        float randomVal = Random.Range(0f, 1f);

        string name = "bounty " + randomVal;
        string shortOverview = "This is the first bounty";
        string description = "This is the description of the first bounty where you will go for walks and do amazing things";
        int goldReward = 50 + Mathf.RoundToInt(500f * randomVal);
        int xpReward = 10 + Mathf.RoundToInt(100f * randomVal);
        int timeToComplete = 1000 + Mathf.RoundToInt(randomVal * 1500f);
        int distance = Mathf.RoundToInt(1000f * randomVal);
        int difficulty = 1;
        quest q = new quest(name, shortOverview, description, goldReward, xpReward, null, timeToComplete, distance, difficulty);
        return q;
    }
}
