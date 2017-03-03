using UnityEngine;
using System.Collections;

public class StoryOverlord : MonoBehaviour {

    public static int currentLevel = 0;
    private Questing quest;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setLevel(int storyLevel) {
        currentLevel = storyLevel;
    }

    // This could end up being a massive class with a bajillion if statements for each possible level. There might be a better way to do it but `\_("/)_/`
    public static void startQuest(int level) {
        if(level == 0) {
            int goldReward = 0;
            int xpReward = 0;
            float timeToComplete = -1;
            float distance = 10;
            float difficulty = 1;
            quest thisQuest = new quest("Quest 1", "Get to the first town.", "This is the start of your adventure! Make your way to the first town!", goldReward, xpReward, null, timeToComplete, distance, difficulty);
            thisQuest.active = true;
            Questing.startQuest(thisQuest);
        }
        if(level == 1) {
            int goldReward = 0;
            int xpReward = 0;
            float timeToComplete = -1;
            float distance = 100;
            float difficulty = 1;
            quest thisQuest = new quest("Quest 2", "Get to the second town.", "This is the start of your adventure! Make your way to the second town!", goldReward, xpReward, null, timeToComplete, distance, difficulty);
            thisQuest.active = true;
            Questing.startQuest(thisQuest);
        }
    }


}
