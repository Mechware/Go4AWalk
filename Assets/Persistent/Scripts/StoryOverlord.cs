using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StoryOverlord : MonoBehaviour {

    public static int currentLevel = 0;
    public static int firstEnemy = 0;
    public static int lastEnemy = 9;
    private Questing quest;
    public static item reward;
    public static string questStartDialogue;
    public static string questEndDialogue;
    public static int characterSprite;
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
            // Enemy Spawns: GnomeChompy
            // Boss: GreenChompy
            // Reward: Bronze Sword
            characterSprite = 0; // Old Guy
            questStartDialogue = "";
            questEndDialogue = "";
            firstEnemy = 0;
            lastEnemy = 0;
            reward = ItemList.itemMasterList[ItemList.BRONZE_SWORD];
            int goldReward = 0;
            int xpReward = 0;
            float timeToComplete = -1;
            float distance = 100;
            float difficulty = 1;
            quest thisQuest = new quest("Quest 1", "Get your first sword!", "This is the start of your adventure! Prove yourself by getting back the sword that the gnome stole.", goldReward, xpReward, null, timeToComplete, distance, difficulty);
            thisQuest.active = true;
            Questing.startQuest(thisQuest);            
        }
        if(level == 1) {
            // Enemy Spawns: GnomeChompy, GreenChompy, Goblin, Red Goblin
            // Boss: Goblin Champ
            // Reward: Iron Sword
            firstEnemy = 0;
            lastEnemy = 3;
            int goldReward = 0;
            int xpReward = 0;
            float timeToComplete = -1;
            float distance = 1000;
            float difficulty = 1;
            quest thisQuest = new quest("Quest 2", "Get to the first town.", "Now that you have a weapon your adventure can begin! Make it to the first town.", goldReward, xpReward, null, timeToComplete, distance, difficulty);
            thisQuest.active = true;
            Questing.startQuest(thisQuest);
        }
    }


}
