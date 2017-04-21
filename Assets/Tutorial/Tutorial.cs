using System;
using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    private const string DONE_WALKING = "Done Walking Tutorial";
    private const string DONE_WALKING_BUTTONS = "Done Walking Buttons Tutorial";
    private const string DONE_FIGHTING = "Done Fighting Tutorial";
    private const string DONE_STURDY = "Done Sturdy Tutorial";
    private const string DONE_ARMORED = "Done Armored Tutorial";
    private const string DONE_INVENTORY = "Done Inventory Tutorial";
    private const string DONE_ALCHEMY = "Done Alchemy Tutorial";
    private const string DONE_RESTING = "Done Resting Tutorial";
    private const string DONE_GOTORESTING = "Done Go To Resting Tutorial";

    private static bool doneWalkingTutorial, doneWalkingButtonTutorial, doneFightingTutorial, doneSturdyTutorial, doneArmoredTutorial;
    private static bool doneInventoryTutorial, doneAlchemyTutorial, doneRestingTutorial, doneGoToRestingTutorial;  

	// Use this for initialization
	void Start () {

        load();

        if (!doneRestingTutorial && GameState.atCamp) {
            StartCoroutine(callAfterOneFrame(restingTutorial()));
        }

        if (!doneWalkingTutorial && GameState.walking) {
            StartCoroutine(callAfterOneFrame(walkingTutorial()));
        }

        if(!doneWalkingButtonTutorial && GameState.walking) {
            StartCoroutine(callAfterOneFrame(walkingButtonsTutorial()));
        }

        if(!doneInventoryTutorial && GameState.walking && Inventory.getInventory().Length > 0) {
            StartCoroutine(callAfterOneFrame(inventoryTutorial()));
        }

        if(!doneAlchemyTutorial && GameState.atCamp && !canBuyPotion().Equals("")) {
            StartCoroutine(callAfterOneFrame(alchemyTutorial()));
        }

        if(!doneFightingTutorial && GameState.fighting) {
            StartCoroutine(callAfterOneFrame(fightingTutorial()));

        } else if(!doneSturdyTutorial && GameState.fighting && 
            EnemyWatchdog.instance.currentEnemy.GetComponent<Enemy>().sturdy < 1) {
            StartCoroutine(callAfterOneFrame(sturdyTutorial()));

        } else if(!doneArmoredTutorial && GameState.fighting && 
            EnemyWatchdog.instance.currentEnemy.GetComponent<Enemy>().armor < 1) {
            StartCoroutine(callAfterOneFrame(armoredTutorial()));
        }
        
        if(!doneGoToRestingTutorial && GameState.walking && Player.health.Value < Player.getMaxHealth()/2) {
            StartCoroutine(callAfterOneFrame(goToRestingTutorial()));
        }
    }
	
    private IEnumerator callAfterOneFrame(IEnumerator otherCoroutine) {
        yield return 0;
        yield return otherCoroutine;
    }

    private string canBuyPotion() {

        if (PotionInventory.numRedLeaf >= AlchemyPanel.RED_LEAVES_FOR_HEALTH) {
            return "health";
        } else if (PotionInventory.numYellowLeaf >= AlchemyPanel.YELLOW_LEAVES_FOR_CRIT) {
            return "critical";
        } else if (PotionInventory.numBlueLeaf >= AlchemyPanel.BLUE_LEAVES_FOR_POWER) {
            return "power";
        } else {
            return "";
        }
    }

	// Update is called once per frame
	void Update () {
	
	}

    private const string WALKING_TUTORIAL_TEXT = "TUTORIAL\nWhile you walk in real life, you will walk through the game. As you walk enemies will follow you that you must defeat!";
    IEnumerator walkingTutorial() {
        tutorial(() => {
            doneWalkingTutorial = true;
            save();
        }, WALKING_TUTORIAL_TEXT);
        yield break;
    }

    private const string WALKING_BUTTON_TUTORIAL_TEXT = "TUTORIAL\nYou have a monster following you! By clicking the monster head on the bottom right you will be able to fight them.";
    IEnumerator walkingButtonsTutorial() {
        while(EnemyWatchdog.enemiesQueue.Count == 0) {
            yield return new WaitForSeconds(2);
        }

        tutorial(() => {
            doneWalkingButtonTutorial = true;
            save();
        }, WALKING_BUTTON_TUTORIAL_TEXT);
        yield break;
    }

    private const string FIGHTING_TUTORIAL_TEXT = "TUTORIAL\nTo fight enemies you must tap on them quickly! As you tap more your critical bar (yellow) will increase, with a greater chance of resetting back to zero with each tap. When you swipe over the enemy you do a critical hit that does damage based on your crit bar.";
    IEnumerator fightingTutorial() {
        yield return new WaitForSeconds(1);
        Time.timeScale = 0;
        tutorial(() => {
            Time.timeScale = 1;
            StartCoroutine(fightingEndingTutorial());
        }, FIGHTING_TUTORIAL_TEXT);
        yield break;
    }

    private const string FIGHTING_ENDING_TEXT = "TUTORIAL\nGood job! At the end of some fights, enemies or treasure chests drop items. Click on them to pick them up!";
    IEnumerator fightingEndingTutorial() {
        Enemy currentEnemy = EnemyWatchdog.instance.currentEnemy.GetComponent<Enemy>();
        while(!currentEnemy.isDead) {
            yield return 0;
        }
        Time.timeScale = 0;
        tutorial(() => {
            Time.timeScale = 1;
            doneFightingTutorial = true;
            save();
        }, FIGHTING_ENDING_TEXT);
        yield break;
    }

    private const string STURDY_TUTORIAL_TEXT = "TUTORIAL\nThis is a sturdy enemy. They are not affected by criticals but tapping does more damage, so tap away!";
    IEnumerator sturdyTutorial() {
        yield return new WaitForSeconds(1);
        Time.timeScale = 0;
        tutorial(() => {
            Time.timeScale = 1;
            doneSturdyTutorial = true;
            save();
        }, STURDY_TUTORIAL_TEXT);
        yield break;
    }

    private const string ARMORED_TUTORIAL_TEXT = "TUTORIAL\nThis is an armored enemy. Criticals (swipes) do extra damage to them, so try to get as many criticals as possible!";
    IEnumerator armoredTutorial() {
        yield return new WaitForSeconds(1);
        Time.timeScale = 0;
        tutorial(() => {
            Time.timeScale = 1;
            doneArmoredTutorial = true;
            save();
        }, ARMORED_TUTORIAL_TEXT);
        yield break;
    }

    private const string INVENTORY_TUTORIAL_TEXT = "TUTORIAL\nLooks like you've gotten a few items! Try to access them through your journal in the upper left corner.";
    IEnumerator inventoryTutorial() {
        tutorial(() => {
            StartCoroutine(openInventoryTutorial());
        }, INVENTORY_TUTORIAL_TEXT);
        yield break;
    }

    private const string INVENTORY_OPEN_TEXT = "TUTORIAL\nGo to the \"Items\" tab to see your new items";
    IEnumerator openInventoryTutorial() {
        while (!PersistentUIElements.instance.JournalMenu.activeInHierarchy) {
            yield return 0;
        }

        tutorial(() => {
            StartCoroutine(openItemsTutorial());
        }, INVENTORY_OPEN_TEXT);
    }

    private const string ITEMS_OPEN_TEXT = "TUTORIAL\nClick on the image of the items to equip them, and they will replace what is in either the weapon slot, armor slot, or the accessory slot, depending on what the item it is.";
    IEnumerator openItemsTutorial() {
        while(!PersistentUIElements.instance.itemsPanel.activeInHierarchy) {
            yield return 0;
        }

        tutorial(() => {
            doneInventoryTutorial = true;
            save();
        }, ITEMS_OPEN_TEXT);
    }

    private const string ALCHEMY_TUTORIAL_TEXT = "TUTORIAL\nIt looks like you are able to make some potions! To start performing alchemy click on the backpack.";
    IEnumerator alchemyTutorial() {
        tutorial(() => {
            StartCoroutine(alchemyOpenTutorial());
        }, ALCHEMY_TUTORIAL_TEXT);
        yield break;
    }

    private const string ALCHEMY_OPEN_TEXT = "TUTORIAL\nEach potion requires a leaf from an enemy and gold. Try crafting a potion now by tapping on the potion's icon!";
    IEnumerator alchemyOpenTutorial() {
        while (!Shop.instance.shopPanel.activeInHierarchy)
            yield return 0;

        tutorial(() => {
            StartCoroutine(alchemyCreatePotionTutorial());
        }, ALCHEMY_OPEN_TEXT);
        yield break;
    }

    private const string ALCHEMY_POTION_TEXT = "TUTORIAL\nCongratulations! You've crafted your first potion! Don't forget to always stock up on potions. They can save your life!";
    IEnumerator alchemyCreatePotionTutorial() {
        int healthPots = PotionInventory.numHealthPots.Value;
        int critPots = PotionInventory.numCritPots.Value;
        int powerPots = PotionInventory.numAttackPots.Value;

        while (healthPots == PotionInventory.numHealthPots.Value
            && critPots == PotionInventory.numCritPots.Value
            && powerPots == PotionInventory.numAttackPots.Value) {
            yield return 0;
        }

        tutorial(() => {
            doneAlchemyTutorial = true;
            save();
        }, ALCHEMY_POTION_TEXT);
        yield break;
    }

    private const string SHOULD_REST_TEXT = "TUTORIAL\nYour health is a little low, try building a camp using the camp button on the bottom left and resting for the night! This will also save all of the gold you have gotten from fighting monsters.";
    IEnumerator goToRestingTutorial() {
        tutorial(() => {
            doneGoToRestingTutorial = true;
            save();
        }, SHOULD_REST_TEXT);
        yield break;
    }


    private const string RESTING_TUTORIAL_TEXT = "TUTORIAL\nTo rest for the night and regain your health, click on the sleeping bag. Once you're rested you will start walking again.";
    IEnumerator restingTutorial() {
        tutorial(() => {
            doneRestingTutorial = true;
            save();
        }, RESTING_TUTORIAL_TEXT);
        yield break;
    }

    void tutorial(Action whenDone, string text) {
        PopUp.instance.showPopUp(text,
            new string[] { "Got it." },
            new Action[] { whenDone });
    }

    static void save() {
        PlayerPrefs.SetString(DONE_ALCHEMY, doneAlchemyTutorial.ToString());
        PlayerPrefs.SetString(DONE_ARMORED, doneArmoredTutorial.ToString());
        PlayerPrefs.SetString(DONE_FIGHTING, doneFightingTutorial.ToString());
        PlayerPrefs.SetString(DONE_INVENTORY, doneInventoryTutorial.ToString());
        PlayerPrefs.SetString(DONE_RESTING, doneRestingTutorial.ToString());
        PlayerPrefs.SetString(DONE_STURDY, doneSturdyTutorial.ToString());
        PlayerPrefs.SetString(DONE_WALKING, doneWalkingTutorial.ToString());
        PlayerPrefs.SetString(DONE_GOTORESTING, doneGoToRestingTutorial.ToString());
        PlayerPrefs.SetString(DONE_WALKING_BUTTONS, doneWalkingButtonTutorial.ToString());
        PlayerPrefs.Save();
    }

    public static void load() {
        doneAlchemyTutorial = bool.Parse(PlayerPrefs.GetString(DONE_ALCHEMY, bool.FalseString));
        doneArmoredTutorial = bool.Parse(PlayerPrefs.GetString(DONE_ARMORED, bool.FalseString));
        doneFightingTutorial = bool.Parse(PlayerPrefs.GetString(DONE_FIGHTING, bool.FalseString));
        doneInventoryTutorial = bool.Parse(PlayerPrefs.GetString(DONE_INVENTORY, bool.FalseString));
        doneRestingTutorial = bool.Parse(PlayerPrefs.GetString(DONE_RESTING, bool.FalseString));
        doneSturdyTutorial = bool.Parse(PlayerPrefs.GetString(DONE_STURDY, bool.FalseString));
        doneWalkingTutorial = bool.Parse(PlayerPrefs.GetString(DONE_WALKING, bool.FalseString));
        doneGoToRestingTutorial = bool.Parse(PlayerPrefs.GetString(DONE_GOTORESTING, bool.FalseString));
        doneWalkingButtonTutorial = bool.Parse(PlayerPrefs.GetString(DONE_WALKING_BUTTONS, bool.FalseString));
    }

    public static void clear() {
        doneAlchemyTutorial = false;
        doneArmoredTutorial = false;
        doneFightingTutorial = false;
        doneInventoryTutorial = false;
        doneRestingTutorial = false;
        doneSturdyTutorial = false;
        doneWalkingTutorial = false;
        doneWalkingButtonTutorial = false;
    }
}
