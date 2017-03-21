using UnityEngine;
using System.Collections;
using Gamelogic.Extensions;
using System;

public class Player : MonoBehaviour {


    public static bool died = false;

    public static Player instance;

    public static ObservedValue<float> totalDistance;
    public static ObservedValue<int> gold, experience, level, lootGold, distance;
    public static int experienceOfLastLevel = 0;

    private static int maxHealth = 100;
    public static ObservedValue<int> health;

    public static ObservedValue<int> crit;
    public static int attackStrength = 5;
    private static int critFactor = 4;
    public static float attackModifier = 1;
    public static float defenseModifier = 1;
    public static float critModifier = 1;
    public static item equippedWeapon;
    public static item equippedArmor;


    #region nonstatic

    void Awake() {
        if (gold == null) {
            gold = new ObservedValue<int>(0);
            lootGold = new ObservedValue<int>(0);
            experience = new ObservedValue<int>(0);
            level = new ObservedValue<int>(1);
            totalDistance = new ObservedValue<float>(0);
            health = new ObservedValue<int>(100);
        } else {
            gold = new ObservedValue<int>(gold.Value);
            lootGold = new ObservedValue<int>(lootGold.Value);
            experience = new ObservedValue<int>(experience.Value);
            level = new ObservedValue<int>(level.Value);
            totalDistance = new ObservedValue<float>(totalDistance.Value);
            health = new ObservedValue<int>(health.Value);
        }
        crit = new ObservedValue<int>(0);
        attackStrength = 5 + level.Value + equippedWeapon.baseAttack;

        if (defenseModifier == 0) {
            defenseModifier = 1;
        } else { defenseModifier = equippedArmor.attributeValue; }

        maxHealth = 100 + 10 * level.Value;
        critFactor = 4 + Mathf.RoundToInt((equippedWeapon.critModifier - 1f) * 40);

        if (Player.equippedWeapon.Equals(ItemList.noItem) || Player.equippedWeapon.Equals(default(item))) {
            Inventory.items.Add(ItemList.itemMasterList[ItemList.STEEL_RAPIER]);
            equipWeapon(ItemList.itemMasterList[ItemList.STEEL_RAPIER]);

        }
        if (Player.equippedArmor.Equals(ItemList.noItem) || Player.equippedArmor.Equals(default(item))) {
            Inventory.items.Add(ItemList.itemMasterList[ItemList.BRONZE_ARMOR]);
            equipArmor(ItemList.itemMasterList[ItemList.BRONZE_ARMOR]);

        }


        if (died) {
            health.Value = maxHealth / 2;
            lootGold = new ObservedValue<int>(0);
        }

        if (GameState.inTown) {
            gold.Value += lootGold.Value;
            lootGold.Value = 0;
        }
        if (GameState.atCamp) {
            gold.Value += lootGold.Value;
            lootGold.Value = 0;
        }

        instance = this;
    }

    void Start() {

        // Let user know they died
        if (died) {
            PopUp.instance.showPopUp("Oh no! You were defeated! \n You conveniently find yourself back at camp, \n but the gold you have accumulated has been stolen.", new string[] { "Okay", "No." },
                new System.Action[] {
                    new System.Action(() => {}),
                    new System.Action(() => {
                        PopUp.instance.showPopUp("Too bad.", new string[] {"Fine."}, new System.Action[] {
                            new System.Action(()=> { })
                        });
                    })
                }, new bool[] { true, false });
            died = false;
        } else if (GameState.walking) {
            if (Questing.currentQuest.name == null) {
                quest testQuest = new quest(
                    name: "Test Quest",
                    shortOverview: "You're exploring in the forest",
                    description: "Find new enemies and get experience and gold!",
                    goldReward: 0,
                    xpReward: 0,
                    rewards: null,
                    timeToComplete: -1,
                    distance: -1,
                    difficulty: 1);
                Questing.startQuest(testQuest);
            }
        }

        if (experience.Value == 0)
            load();
        else {
            save();
        }

    }

    public void equipWeapon(item newItem) {
        UnityEngine.Assertions.Assert.AreEqual(newItem.type, itemType.Weapon, "Trying to equip something that is not a weapon.");

        attackStrength -= equippedWeapon.baseAttack;
        equippedWeapon = newItem;
        attackStrength += equippedWeapon.baseAttack;
        GetComponent<PersistentUIElements>().updateItems();
    }

    public void equipArmor(item newItem) {
        UnityEngine.Assertions.Assert.AreEqual(newItem.type, itemType.Armor, "Trying to equip something that is not Armor.");

        defenseModifier -= equippedArmor.attributeValue;
        equippedArmor = newItem;
        defenseModifier += equippedArmor.attributeValue;
        GetComponent<PersistentUIElements>().updateItems();
    }


    #endregion

    #region static

    #region combat
    public static void damage(int amount) {
        health.Value -= Mathf.RoundToInt((float)amount*defenseModifier);
        if (health.Value <= 0) {
            die();
        }
    }



    public static int updateCrit(int randFactor) {

        if (crit.Value == 100) return 100;

        // Updates crit and returns updated value
        float rand = UnityEngine.Random.Range(0.0f, 1.0f);

        if ((rand + 0.30f) * critModifier > 1f / 16000f * (float)(crit.Value * crit.Value)) {
            crit.Value += critFactor + randFactor;
            if (crit.Value > 100) crit.Value = 100;
        } else {
            crit.Value = 0;
        }

        return crit.Value;
    }


    public static void resetCrit() {
        crit.Value = 0;
    }

    public static int getCrit() {
        return crit.Value;
    }

    private static void die() {
        EnemyWatchdog.instance.clearEnemies();
        died = true;
        Questing.makeCamp();
    }

    // Returns a regular random attack
    public static int getRegularAttack() {
        float randFactor = UnityEngine.Random.Range(-1.0f, 1.0f);
        float fAttackStrength = (float)attackStrength;

        // Update crit to some value
        updateCrit(Mathf.RoundToInt(randFactor));

        return Mathf.RoundToInt(fAttackStrength * attackModifier) + Mathf.RoundToInt(randFactor * fAttackStrength); ;
    }

    /// <summary>
    /// Get the damage from a swipe
    /// </summary>
    /// <param name="swipe">Vector2 of the swipe the was performed</param>
    /// <returns></returns>
    public static int getCriticalAttack(Vector2 swipe) {

        //Debug.Log("Swipe length: " + swipe.magnitude);
        //Debug.Log("Swipe angle: " + Mathf.Atan2(swipe.y, swipe.x));

        float fAttack;
        int attack;

        if (crit.Value < 100) {
            fAttack = (((float)crit.Value) / 5f) * attackStrength * attackModifier;
            attack = Mathf.RoundToInt(fAttack);
        } else {
            attack = (int)(150f / 5f * attackStrength * attackModifier);
        }

        return attack;
    }
    #endregion

    #region savingAndLoading
    public void save() {
        PlayerPrefs.SetInt("Health", health.Value);
        PlayerPrefs.SetInt("Gold", gold.Value);
        PlayerPrefs.SetInt("XP", experience.Value);
        PlayerPrefs.SetFloat("Distance", totalDistance.Value);
        PlayerPrefs.SetInt("Level", level.Value);
        PlayerPrefs.SetInt("HealthPotions", PotionInventory.numHealthPots.Value);
        PlayerPrefs.SetInt("CritPotions", PotionInventory.numCritPots.Value);
        PlayerPrefs.SetInt("AttackPotions", PotionInventory.numAttackPots.Value);
        PlayerPrefs.SetInt("StoryLevel", StoryOverlord.currentLevel);
        PlayerPrefs.SetString("Inventory", Inventory.getInventory());
        PlayerPrefs.Save();
    }

    public void load() {
        health.Value = PlayerPrefs.GetInt("Health", health.Value);
        gold.Value = PlayerPrefs.GetInt("Gold", gold.Value);
        experience.Value = PlayerPrefs.GetInt("XP", experience.Value);
        totalDistance.Value = PlayerPrefs.GetFloat("Distance", totalDistance.Value);
        level.Value = PlayerPrefs.GetInt("Level", level.Value);
        StoryOverlord.currentLevel = PlayerPrefs.GetInt("StoryLevel", StoryOverlord.currentLevel);

        PotionInventory.numHealthPots.Value = PlayerPrefs.GetInt("HealthPotions", PotionInventory.numHealthPots.Value);
        PotionInventory.numCritPots.Value = PlayerPrefs.GetInt("CritPotions", PotionInventory.numCritPots.Value);
        PotionInventory.numAttackPots.Value = PlayerPrefs.GetInt("AttackPotions", PotionInventory.numAttackPots.Value);
    }

    public void clearStats() {
        PopUp.instance.showPopUp("Are you sure you want to reset your players stats permanently?",
            new string[] { "Yes", "No" },
            new System.Action[] {
                new System.Action(()=> {
                    PlayerPrefs.DeleteAll();
                    /*
                    PlayerPrefs.DeleteKey("Health");
                    PlayerPrefs.DeleteKey("Gold");
                    PlayerPrefs.DeleteKey("XP");
                    PlayerPrefs.DeleteKey("Distance");
                    PlayerPrefs.DeleteKey("Level");
                    PlayerPrefs.DeleteKey("HealthPotions");
                    PlayerPrefs.DeleteKey("CritPotions");
                    PlayerPrefs.DeleteKey("AttackPotions");
                    */
                    
                    PotionInventory.numHealthPots.Value = 0;
                    PotionInventory.numCritPots.Value = 0;
                    PotionInventory.numAttackPots.Value = 0;
                    health.Value = 100;
                    maxHealth = 100;
                    gold.Value = 0;
                    experience.Value = 0;
                    totalDistance.Value = 0;
                    level.Value = 0;
                    StoryOverlord.currentLevel = 0;
                    save();
                    GameState.loadScene(GameState.scene.CAMPSITE);
                     }),
                new System.Action(()=> { })
            });

    }

    #endregion

    #region gettersAndSetters

    public static void giveExperience(int amount) {
        experience.Value += amount;
        if (experience.Value > experienceOfLastLevel + 100) {
            level.Value++;
            experienceOfLastLevel = experience.Value;
        }
    }

    public static void giveHealth(int amount) {
        health.Value += amount;
        if (health.Value > maxHealth)
            health.Value = maxHealth;
        else if (health.Value <= 0)
            die();
    }

    public static void giveGold(int amount) {
        gold.Value += amount;
    }

    public static void giveLootGold(int amount) {
        lootGold.Value += amount;
    }

    public static void takeGold(int amount) {
        gold.Value -= amount;
    }

    public static void takeLootGold(int amount) {
        lootGold.Value -= amount;
    }

    public static int getMaxHealth() {
        return maxHealth;
    }

    public static int getCurrentHealth() {
        return health.Value;
    }

    public static int getAttack() {
        return attackStrength;
    }

    public static void setAttack(int attack) {
        attackStrength = attack;
    }

    #endregion

    #endregion


}
