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
    public static Sprite characterSprite;
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
            characterSprite = Resources.Load("Characters/Moik") as Sprite; // Old Guy
            questStartDialogue = "Well, if you're set on going on an adventure you'll need a sword. Tell you what, I'll give you my old sword if you can "+
                "get it back from those pesky gnomes that stole it. They should be out in the garden somewhere, and watch out they like to bite.";

            questEndDialogue = "Well look at that, you actually did it. Hope that old thing works for you, I'd give you my new sword but its actually nice and "+
                "I like it. Anyway, if you're set on going on this adventure the mountain is 100km away so I hope you brought your walking shoes cause you're in " +
                "for quite the trip. The next town is a few kilometers down the road, you can't miss it. I would start there is I were you. Well, good luck, and "+
                "don't come crying to me when you get yourself killed.";
               
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
            questStartDialogue = "Hey new quest here";
            questEndDialogue = "Woo u did it";
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
