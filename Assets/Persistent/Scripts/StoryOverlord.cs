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
    public static Material walkingBackground;
    public static Sprite fightingBackground;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // This could end up being a massive class with a bajillion if statements for each possible level. There might be a better way to do it but `\_("/)_/`
    public static void startQuest(int level, float progress) {
        if(level == 0) {
            walkingBackground = Resources.Load("Materials/garden") as Material;
            texture = Resources.Load("Materials/SmallTreeFightingBackground") as Texture2D;
            fightingBackground = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
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
                "Meadsville is a few kilometers down the road, you can't miss it. I would start there is I were you. "+
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
            Questing.startQuest(thisQuest, progress);            
        }
        if(level == 1) {
            walkingBackground = Resources.Load("Materials/grass") as Material;
            texture = Resources.Load("Materials/SmallTreeFightingBackground") as Texture2D;
            fightingBackground = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
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
            Questing.startQuest(thisQuest, progress);
        }
        if (level == 2) {
            walkingBackground = Resources.Load("Materials/forest") as Material;
            texture = Resources.Load("Materials/BigTreeFightingBackground") as Texture2D;
            fightingBackground = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            // Enemy Spawns: Goblin, Evil Tree, Red Goblin, Slime
            // Boss: Evil Dead Tree
            // Reward: Iron Armor

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

            textureEnd = Resources.Load("Characters/Narrator") as Texture2D; // Narrator
            characterSpriteEnd = Sprite.Create(textureEnd, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            characterNameEnd = "Narrator";
            questEndDialogue = "You defeat the tree and make your way past it. The trees begin to clear - oh wait, no they dont. The trees actually get thicker as you enter an even spookier "+
                "part of the forest! Oh no!";
            
            firstEnemy = 2;
            lastEnemy = 5;
            reward = ItemList.itemMasterList[ItemList.IRON_ARMOR];
            int goldReward = 0;
            int xpReward = 0;
            float timeToComplete = -1;
            float distance = 3000;
            float difficulty = 1;
            quest thisQuest = new quest("Quest 3", "Get through the forest.", "Make your way through the forest to get to Meadsville.", goldReward, xpReward, null, timeToComplete, distance, difficulty);
            thisQuest.active = true;
            Questing.startQuest(thisQuest, progress);
        }
        if (level == 3) {
            walkingBackground = Resources.Load("Materials/hauntedForest") as Material;
            texture = Resources.Load("Materials/SpookyTreeFightingBackground") as Texture2D;
            fightingBackground = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            // Enemy Spawns: Blue Goblin, Dead Tree, Spooky Ghost, Skeleton
            // Boss: Ghost
            // Reward: Steel Sword

            texture = Resources.Load("Characters/Narrator") as Texture2D; // Narrator  
            characterSpriteStart = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            characterNameStart = "Narrator";
            questStartDialogue = "This part of the forest looks haunted. Be careful!";

            textureBoss = Resources.Load("Characters/Ghost") as Texture2D; // Narrator  
            bossfightSprite = Sprite.Create(textureBoss, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            bossfightName = "Spooky Ghost";
            bossFightDialogue = "WOoOoOoOoOoO!!!!";

            textureEnd = Resources.Load("Characters/Narrator") as Texture2D; // Red Goblin
            characterSpriteEnd = Sprite.Create(textureEnd, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            characterNameEnd = "Narrator";
            questEndDialogue = "Wow! That was all way to spooky for me! I'm glad we're through that. You see the exit of the forest up ahead, and this time its actually the end of the forest.";

            firstEnemy = 5;
            lastEnemy = 9;
            reward = ItemList.itemMasterList[ItemList.STEEL_SWORD];
            int goldReward = 0;
            int xpReward = 0;
            float timeToComplete = -1;
            float distance = 2000;
            float difficulty = 1;
            quest thisQuest = new quest("Quest 4", "Get through the HAUNTED forest.", "You're almost through the forest, push on and get to Meadsville.", goldReward, xpReward, null, timeToComplete, distance, difficulty);
            thisQuest.active = true;
            Questing.startQuest(thisQuest, progress);
        }
        if (level == 4) {
            walkingBackground = Resources.Load("Materials/grass") as Material;
            texture = Resources.Load("Materials/SmallTreeFightingBackground") as Texture2D;
            fightingBackground = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            // Enemy Spawns: Blue Goblin, Goblin King, Rock Golem
            // Boss: Goblin Champ
            // Reward: Orichalcum Sword

            texture = Resources.Load("Characters/Narrator") as Texture2D; // Narrator  
            characterSpriteStart = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            characterNameStart = "Narrator";
            questStartDialogue = "You emerge from the forest, and up ahead in the distance you see the town of Meadsville. A faint tendril of smoke snakes its' way to the sky, and you can "+
                "hear faint shouting on the wind. Meadsville looks like its in trouble! You'd better hurry.";

            textureBoss = Resources.Load("Characters/Goblin Champion") as Texture2D; // Narrator  
            bossfightSprite = Sprite.Create(textureBoss, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            bossfightName = "Goblin Champion";
            bossFightDialogue = "Hey you! Get out of here. This town is mine to attack and plunder, go find your own! Oh wait, you're not an adventurer that's going to try and stop me are you? " +
                "Oh bother, you are. Well then if you want a piece of the CHAMP then you're gonna regret it! YOUUU CAN'T SEEEE MEEE!! \n*waves hand in front of face*";

            textureEnd = Resources.Load("Characters/Old Man") as Texture2D; // Red Goblin
            characterSpriteEnd = Sprite.Create(textureEnd, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            characterNameEnd = "Meadsville Mayor";
            questEndDialogue = "Hey, thanks for defeating that boss for us Adventurer! Everyone knows how poorly NPC town guards perform when it comes to fighting bosses, who knows what we would "+
                "have done if your hadn't come along at such a convenient time! As is customary when being saved by an adventurer I will give you some loot. Here is a sword made of orichalcum! " +
                "Its a rare metal that we mine in Meadsville, its a wonder they don't call us Orichalcumville! Ha Ha Ha! Yeah I know it doesn't sound as good, but it would make more sense "+ 
                "economics-wise. \n\nSo you want to get to Mt. Flume and kill the beast that awoke there eh? Yeah, sounds like something an adventurer would do. Well the quickest way to Mt. Flume "+
                "is to go through the desert to the north of here. Well, I'll let you get going, I know how you adventurer types get when you have to read too much dialogue. Good luck Adventurer" +
                "and thanks again for saving our town!";

            firstEnemy = 8;
            lastEnemy = 10;
            reward = ItemList.itemMasterList[ItemList.ORICH_SWORD];
            int goldReward = 0;
            int xpReward = 0;
            float timeToComplete = -1;
            float distance = 4000;
            float difficulty = 1;
            quest thisQuest = new quest("Quest 5", "Get to Meadsville.", "You've made it out of the forest now get to Meadsville and see what the problem is.", goldReward, xpReward, null, timeToComplete, distance, difficulty);
            thisQuest.active = true;
            Questing.startQuest(thisQuest, progress);
        }
        if (level == 5) {
            walkingBackground = Resources.Load("Materials/grass") as Material;
            texture = Resources.Load("Materials/SmallTreeFightingBackground") as Texture2D;
            fightingBackground = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            // Enemy Spawns: All
            // Boss: N/A
            // Reward: Orichalcum Sword

            texture = Resources.Load("Characters/Chicken") as Texture2D; // Narrator  
            characterSpriteStart = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            characterNameStart = "Spiffy Chicken";
            questStartDialogue = "Hey player! I would like to personally thank you for playing our game to this point. If you enjoyed it please leave some feedback on the Play Store. "+
                "Unfortunately the game ends here. You can keep playing and fight all the enemies you have encountered so far but this is as far as the story goes. If this makes you angry "+
                "feel free to call us lazy devs in some strongly worded hate mail, but if you would like to see more of the game please let us know that there is interest out there for us " +
                "to continue with this project. Hopefully with your help we can turn this 10 km quest into a true 100 km adventure!";

            textureBoss = Resources.Load("Characters/Goblin Champion") as Texture2D; // Narrator  
            bossfightSprite = Sprite.Create(textureBoss, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            bossfightName = "Goblin Champion";
            bossFightDialogue = "Hey you! Get out of here. This town is mine to attack and plunder, go find your own! Oh wait, you're not an adventurer that's going to try and stop me are you? " +
                "Oh bother, you are. Well then if you want a piece of the CHAMP then you're gonna regret it! YOUUU CAN'T SEEEE MEEE!! \n*waves hand in front of face*";

            textureEnd = Resources.Load("Characters/Old Man") as Texture2D; // Red Goblin
            characterSpriteEnd = Sprite.Create(textureEnd, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            characterNameEnd = "Meadsville Mayor";
            questEndDialogue = "Hey, thanks for defeating that boss for us Adventurer! Everyone knows how poorly NPC town guards perform when it comes to fighting bosses, who knows what we would " +
                "have done if your hadn't come along at such a convenient time! As is customary when being saved by an adventurer I will give you some loot. Here is a sword made of orichalcum! " +
                "Its a rare metal that we mine in Meadsville, its a wonder they don't call us Orichalcumville! Ha Ha Ha! Yeah I know it doesn't sound as good, but it would make more sense " +
                "economics-wise. \n\nSo you want to get to Mt. Flume and kill the beast that awoke there eh? Yeah, sounds like something an adventurer would do. Well the quickest way to Mt. Flume " +
                "is to go through the desert to the north of here. Well, I'll let you get going, I know how you adventurer types get when you have to read too much dialogue. Good luck Adventurer" +
                "and thanks again for saving our town!";

            firstEnemy = 0;
            lastEnemy = 11;
            reward = ItemList.itemMasterList[ItemList.ORICH_SWORD];
            int goldReward = 0;
            int xpReward = 0;
            float timeToComplete = -1;
            float distance = -1;
            float difficulty = 1;
            quest thisQuest = new quest("Quest 5", "Thanks for playing!", "Keep walking and fighting enemies for as long as your heart desires.", goldReward, xpReward, null, timeToComplete, distance, difficulty);
            thisQuest.active = true;
            Questing.startQuest(thisQuest, progress);
        }
    }


}
