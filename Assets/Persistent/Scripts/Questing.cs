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

    private const string QUESTING_DISTANCE = "QuestingDistance";

    // Questing
    public static quest currentQuest;
    public static Questing instance;
    public EnemyWatchdog ew;

    // Use this for initialization
    void Awake() {
        instance = this;
    }

    void OnApplicationPause() {

    }

    void Start() {
        if(currentQuest.active == false && GameState.walking) {
            float progressThroughQuest = PlayerPrefs.GetFloat(QUESTING_DISTANCE, 0);
            StoryOverlord.startQuest(StoryOverlord.currentLevel, progressThroughQuest);
        }
        if(GameState.walking)
            WalkingWatchdog.instance.setQuestStuff();
    }

    // Update is called once per frame
    void Update() {

    }

    public static void startQuest(quest q, float progress) {
        currentQuest = q;
        if(progress == 0) {
            DialoguePopUp.instance.showDialog(StoryOverlord.questStartDialogue, 
                StoryOverlord.characterNameStart, 
                StoryOverlord.characterSpriteStart, 
                () => { });
        } else {
            currentQuest.distanceProgress = progress;
        }
    }

    public static void endQuest(bool userFinished) {

        if (userFinished) {
            print("Quest passed!");
            Player.giveGold(currentQuest.goldReward);
            Player.giveExperience(currentQuest.xpReward);

            DialoguePopUp.instance.showDialog(StoryOverlord.questEndDialogue, StoryOverlord.characterNameEnd, StoryOverlord.characterSpriteEnd, () => {
            PopUp.instance.showPopUp("QUEST COMPLETE! \n \n" + "Continue on your journey." + "\n\n",
                new string[] { "Continue"},
                new System.Action[] {
                    new System.Action(() => {
                        StoryOverlord.currentLevel++;
                        print("Current level in quest complete: " + StoryOverlord.currentLevel);
                        Player.instance.savePlayer();
                        currentQuest.active = false;
                        GameState.loadScene(GameState.scene.WALKING_LEVEL);                        
                        }),
                     }
                );
            });
        } else {
            print("Quest failed!");
            Player.distance.Value -= 100; // Sets the player back 100m.
            PopUp.instance.showPopUp("QUEST FAILED! \nOh no you were defeated! \n" + "You run away and returned to camp." + "\n\n",
                new string[] { "Continue" },
                new System.Action[] {
                    new System.Action(() => {GameState.loadScene(GameState.scene.CAMPSITE); }) });
        }

        PlayerPrefs.DeleteKey(QUESTING_DISTANCE);
    }

    public static void makeCamp() {
        GameState.loadScene(GameState.scene.CAMPSITE);
    }

    public static void move(float distance) {
        currentQuest.distanceProgress += distance;
        PlayerPrefs.SetFloat(QUESTING_DISTANCE, currentQuest.distanceProgress);

        if (currentQuest.distanceProgress >= currentQuest.distance) {
            if (currentQuest.distance != -1) {
                currentQuest.distanceProgress = currentQuest.distance;
                EnemyWatchdog.instance.startBossFight();
            }
        }
    }
}
