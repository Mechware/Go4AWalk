using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StoryOverlord : MonoBehaviour {

    public static int currentLevel = 0;
    public static int firstEnemy = 0;
    public static int lastEnemy = 9;
    private Questing quest;
    public static item reward;
    public static string questStartDialogue = "";
    public static string bossFightDialogue = "";
    public static string questEndDialogue = "";
    public static Sprite characterSpriteStart;
    public static Sprite characterSpriteEnd;
    public static string characterNameStart = "";
    public static string characterNameEnd = "";
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
            characterSpriteStart = Resources.Load("Characters/Moik") as Sprite; // Old Guy
            characterSpriteEnd = Resources.Load("Characters/Moik") as Sprite; // Old Guy
            characterNameStart = "Old Man";
            characterNameEnd = "OldMan";
            questStartDialogue = "Well, if you're set on going on an adventure you'll need a sword."+
                "Tell you what, I'll give you my old sword if you can get it back from those pesky gnomes that stole it."+
                "They should be out in the garden somewhere, and watch out they like to bite.";

            questEndDialogue = "Well look at that, you actually did it."+
                "Hope that old thing works for you, I'd give you my new sword but its actually nice and I like it."+
                "Anyway, if you're set on going on this adventure the mountain is 100km away so I hope you brought your walking shoes cause you're in for quite the trip."+
                "The next town is a few kilometers down the road, you can't miss it. I would start there is I were you."+
                "Well, good luck, and don't come crying to me when you get yourself killed.";
               
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
            // Enemy Spawns: GnomeChompy, GreenChompy, Goblin
            // Boss: Red Goblin
            // Reward: Bronze Armor
            characterSpriteStart = Resources.Load("Characters/Moik") as Sprite; // Narrator            
            characterNameStart = "Narrator";      
            questStartDialogue = "With your \"new\" sword you start out on your quest. In the distance you see the first town, but you'll have to go through the forest to get there." +
                "The entrance to the forest is 1 km down the road so you head out towards the forest";

            characterSpriteEnd = Resources.Load("Characters/Moik") as Sprite; // Red Goblin
            characterNameEnd = "Red Goblin";
            questEndDialogue = "Hah! You thought we would just let you through into the forest for free? Hah! I don't think so!"+
                "Hah! You'll have to pay the toll if you want to pass!" +
                "What's that? You won't pay the toll? Hah! Then prepare to die! Hah! Hah!";

            firstEnemy = 0;
            lastEnemy = 2;
            reward = ItemList.itemMasterList[ItemList.BRONZE_ARMOR];
            int goldReward = 0;
            int xpReward = 0;
            float timeToComplete = -1;
            float distance = 1000;
            float difficulty = 1;
            quest thisQuest = new quest("Quest 2", "Get to the forest.", "Now that you have a weapon your adventure can begin! Make it to the forest.", goldReward, xpReward, null, timeToComplete, distance, difficulty);
            thisQuest.active = true;
            Questing.startQuest(thisQuest);
        }
        if (level == 2) {
            // Enemy Spawns: Goblin, Evil Tree, Red Goblin, Slime
            // Boss: Red Goblin
            // Reward: Bronze Armor
            characterSpriteStart = Resources.Load("Characters/Moik") as Sprite; // Narrator            
            characterNameStart = "Narrator";
            questStartDialogue = "After defeating the goblin you begin to make your way through the forest."+
                "The sign at the entrance of the forest says that the next town is 3km through the forest.";

            characterSpriteEnd = Resources.Load("Characters/Moik") as Sprite; // Red Goblin
            characterNameEnd = "Red Goblin";
            questEndDialogue = "Hah! You thought we would just let you through into the forest for free? Hah! I don't think so!" +
                "Hah! You'll have to pay the toll if you want to pass!" +
                "What's that? You won't pay the toll? Hah! Then prepare to die! Hah! Hah!";

            firstEnemy = 2;
            lastEnemy = 5;
            reward = ItemList.itemMasterList[ItemList.BRONZE_ARMOR];
            int goldReward = 0;
            int xpReward = 0;
            float timeToComplete = -1;
            float distance = 3000;
            float difficulty = 1;
            quest thisQuest = new quest("Quest 3", "Get through the forest.", "Make your way through the forest and get to the first town.", goldReward, xpReward, null, timeToComplete, distance, difficulty);
            thisQuest.active = true;
            Questing.startQuest(thisQuest);
        }
    }


}
