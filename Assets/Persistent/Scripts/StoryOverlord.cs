using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StoryOverlord : MonoBehaviour {

    public static int currentLevel = 0;
    public static int firstEnemy = 0;
    public static int lastEnemy = 9;
    private Questing quest;
    public static item reward;
    private static Texture2D texture;
    private static Texture2D textureBoss;
    private static Texture2D textureEnd;
    public static string questStartDialogue = "";
    public static string bossFightDialogue = "";
    public static string questEndDialogue = "";
    public static Sprite characterSpriteStart;
    public static Sprite bossfightSprite;
    public static Sprite characterSpriteEnd;
    public static string characterNameStart = "";
    public static string bossfightName = "";
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
            texture = Resources.Load("Characters/Old Man") as Texture2D; // Old Guy
            characterSpriteStart = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            textureBoss = Resources.Load("Characters/Gnome Chompy") as Texture2D; // Gnome Chompy
            bossfightSprite = Sprite.Create(textureBoss, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            textureEnd = Resources.Load("Characters/Old Man") as Texture2D; // Old Guy
            characterSpriteEnd = Sprite.Create(textureEnd, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            characterNameStart = "Old Man";
            bossfightName = "Gnome Chompy";
            characterNameEnd = "OldMan";
            questStartDialogue = "Well, if you're set on going on an adventure you'll need a sword. "+
                "Tell you what, I'll give you my old sword if you can get it back from those pesky gnomes that stole it. "+
                "They should be out in the garden somewhere, and watch out they like to bite.";

            bossFightDialogue = "So you finally found me eh? Well its too late! Now that I have this sword I have become more powerful than you can even imagine! "+
                "With this sword I shall rule over every garden in the world! MUAHAHAHAHAH!";

            questEndDialogue = "Well look at that, you actually did it. "+
                "Hope that old thing works for you, I'd give you my new sword but its actually nice and I like it. "+
                "Anyway, if you're set on going on this adventure the mountain is 100 km away so I hope you brought your walking shoes cause you're in for quite the trip. "+
                "The next town is a few kilometers down the road, you can't miss it. I would start there is I were you. "+
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
            texture = Resources.Load("Characters/Narrator") as Texture2D; // Narrator  
            characterSpriteStart = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            characterNameStart = "Narrator";      
            questStartDialogue = "With your \"new\" sword you start out on your quest. In the distance you see the first town, but you'll have to go through the forest to get there." +
                "You see the entrance to the forest down the road and start walking in that direction.";

            textureBoss = Resources.Load("Characters/Red Goblin") as Texture2D; // Narrator  
            bossfightSprite = Sprite.Create(textureBoss, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            bossfightName = "Red Goblin";
            bossFightDialogue = "Hah! You thought we would just let you through into the forest for free? Hah! I don't think so! " +
                "Hah! You'll have to pay the toll if you want to pass! " +
                "What's that? You won't pay the toll? Hah! Then prepare to die! Hah! Hah!"; 

            textureEnd = Resources.Load("Characters/Red Goblin") as Texture2D; // Red Goblin
            characterSpriteEnd = Sprite.Create(textureEnd, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            characterNameEnd = "Red Goblin";
            questEndDialogue = "Urrgg. Fine you win. You may have defeated me but you'll never survive in the forest all alone! Its way too spooky for the likes of you! Hah! Hah! Hrrrnngg.";

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
         
            texture = Resources.Load("Characters/Narrator") as Texture2D; // Narrator  
            characterSpriteStart = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            characterNameStart = "Narrator";
            questStartDialogue = "After defeating the goblin you continue your journey throught the forest. The overgrowth is thick and the trees tower above you. "+
                "You feel like you are being watched everywhere you go and the trees themselves feel alive. Man, that goblin was right, this place is pretty spooky.";

            textureBoss = Resources.Load("Characters/Tree") as Texture2D; // Narrator  
            bossfightSprite = Sprite.Create(textureBoss, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            bossfightName = "Giant Tree";
            bossFightDialogue = "Hey it's me the narrator. Theres a giant tree blocking the path. The tree would probably say something here but trees can't talk, and even if they did they probably "+
                "can't speak English. Anyway defeat this tree so you can get out of the forest. ";

            textureEnd = Resources.Load("Characters/Narrator") as Texture2D; // Red Goblin
            characterSpriteEnd = Sprite.Create(textureEnd, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            characterNameEnd = "Narrator";
            questEndDialogue = "";
            
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
