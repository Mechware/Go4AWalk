using UnityEngine;
using System.Collections;
using Gamelogic.Extensions;
using System;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    private const string EQUIPPED_WEAPON = "equipped_weapon";
    private const string EQUIPPED_ARMOR = "equipped_armor";
    private const string EQUIPPED_ACCESSORY = "equipped_accessory";
    private const string LAST_ATTRIB = "last_accessory_attribute";
    private const string LAST_TYPE = "last_accessory_type";

    public static bool died = false;

    public static Player instance;

    public static ObservedValue<float> totalDistance;
    public static ObservedValue<int> gold, experience, level, lootGold, distance;
    public static int experienceOfLastLevel = 0;

    public static int dotHit;
    public static int healAmount;
    public static bool isDOT;
    public static bool isHeal;

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
    internal static item equippedAccessory;

    public AudioSource equipSound;


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

        loadPlayer();

        if (equippedWeapon.Equals(ItemList.noItem) || equippedWeapon.Equals(default(item))) {
            Inventory.items.Add(ItemList.itemMasterList[ItemList.WOOD_SWORD]);
            equipWeapon(ItemList.itemMasterList[ItemList.WOOD_SWORD]);
        }

        if (died) {
            health.Value = maxHealth / 2;
            lootGold = new ObservedValue<int>(0);
        }

        if(GameState.atCamp) {
            gold.Value += lootGold.Value;
            lootGold.Value = 0;
        }
        
        savePlayer();
        instance = this;
    }

    void Start() {
        loadOthers();
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
        }

        if (GameState.atCamp) {
            EnemyWatchdog.enemiesQueue.Clear();
            EnemyWatchdog.instance.saveQueue();

        }
    }

    public void equipWeapon(item newItem) {
        UnityEngine.Assertions.Assert.AreEqual(newItem.type, itemType.Weapon, "Trying to equip something that is not a weapon.");

        attackStrength -= equippedWeapon.baseAttack;
        equippedWeapon = newItem;
        attackStrength += equippedWeapon.baseAttack;
        GetComponent<PersistentUIElements>().updateItems();
        equipSound.Play();
        savePlayer();
    }

    public void equipArmor(item newItem) {
        UnityEngine.Assertions.Assert.AreEqual(newItem.type, itemType.Armor, "Trying to equip something that is not Armor.");

        defenseModifier -= equippedArmor.attributeValue;
        equippedArmor = newItem;
        defenseModifier = equippedArmor.attributeValue;
        GetComponent<PersistentUIElements>().updateItems();
        equipSound.Play();
        savePlayer();
    }

    public void equipAccessory(item newItem) {
        UnityEngine.Assertions.Assert.AreEqual(newItem.type, itemType.Accessory, "Trying to equip something that is not an accessory.");

        if((int)newItem.otherInfo == (int)BuffManager.BuffType.attack) {
            equippedAccessory = newItem;
            attackModifier += equippedAccessory.attributeValue;
            GetComponent<PersistentUIElements>().updateItems();
            savePlayer();
        }
        if ((int) newItem.otherInfo == (int) BuffManager.BuffType.crit) {
            equippedAccessory = newItem;
            critModifier += equippedAccessory.attributeValue;
            GetComponent<PersistentUIElements>().updateItems();
            savePlayer();
        }
        if ((int) newItem.otherInfo == (int) BuffManager.BuffType.defense) {
            equippedAccessory = newItem;
            defenseModifier += equippedAccessory.attributeValue;
            GetComponent<PersistentUIElements>().updateItems();
            savePlayer();
        }
        if ((int)newItem.otherInfo == (int)BuffManager.BuffType.fire) {
            isDOT=true;
            equippedAccessory = newItem;
            GetComponent<PersistentUIElements>().updateItems();
            dotHit = Mathf.RoundToInt(equippedAccessory.attributeValue);
            savePlayer();
        }
        if((int)newItem.otherInfo == (int) BuffManager.BuffType.heal) {
            isHeal = true;
            equippedAccessory = newItem;
            healAmount = Mathf.RoundToInt(equippedAccessory.attributeValue);
           // BuffManager.instance.CreateDOT("Heal_Over_Time", BuffManager.BuffType.heal, -1*healAmount, -1, 1, gameObject);
            savePlayer();
        }
        if ((int) newItem.otherInfo==(int) BuffManager.BuffType.health) {
            equippedAccessory = newItem;
            maxHealth += Mathf.RoundToInt(equippedAccessory.attributeValue);
            GetComponent<PersistentUIElements>().updateItems();
            savePlayer();
        }
        equipSound.Play();

    }

    public void removeAccessory(BuffManager.BuffType type, float attrib) {
        if (type == BuffManager.BuffType.attack) {
            attackModifier -= attrib;
        }
        if (type == BuffManager.BuffType.crit) {
            critModifier -= attrib;
        }
        if (type == BuffManager.BuffType.defense) {
            defenseModifier -= attrib;
        }
        if (type == BuffManager.BuffType.fire) {
            isDOT = false;
            dotHit -= Mathf.RoundToInt(attrib);
        }
        if (type == BuffManager.BuffType.heal) {
            isHeal = false;
            healAmount -= Mathf.RoundToInt(attrib);
        }
        if (type == BuffManager.BuffType.health) {
            maxHealth -= Mathf.RoundToInt(attrib);
        }
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
        EnemyWatchdog.enemiesQueue.Clear();
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

    void OnApplicationPause(bool isPaused) {
        if (isPaused) {
            savePlayer();
        }
    }

    public void savePlayer() {
        PlayerPrefs.SetInt("Health", health.Value);
        PlayerPrefs.SetInt("Gold", gold.Value);
        PlayerPrefs.SetInt("XP", experience.Value);
        PlayerPrefs.SetFloat("Distance", totalDistance.Value);
        PlayerPrefs.SetInt("Level", level.Value);
        PlayerPrefs.SetInt("StoryLevel", StoryOverlord.currentLevel);
        PlayerPrefs.SetFloat("LevelProgress", Questing.currentQuest.distanceProgress);
        PlayerPrefs.SetInt("LootGold", lootGold.Value);
        saveEquippedItems();

        PlayerPrefs.Save();
    }

    void saveEquippedItems() {
        if (!equippedWeapon.Equals(default(item)) &&
            !equippedWeapon.Equals(ItemList.noItem))
            PlayerPrefs.SetString(EQUIPPED_WEAPON, equippedWeapon.name);
        if (!equippedArmor.Equals(default(item)) &&
            !equippedArmor.Equals(ItemList.noItem))
            PlayerPrefs.SetString(EQUIPPED_ARMOR, equippedArmor.name);
        if (!equippedAccessory.Equals(default(item)) &&
            !equippedAccessory.Equals(ItemList.noItem)) {
            PlayerPrefs.SetString(EQUIPPED_ACCESSORY, equippedAccessory.name);
            PlayerPrefs.SetFloat(LAST_ATTRIB, equippedAccessory.attributeValue);
            PlayerPrefs.SetInt(LAST_TYPE, (int) equippedAccessory.otherInfo);
        }
    }

    public void loadPlayer() {
        health.Value = PlayerPrefs.GetInt("Health", health.Value);
        gold.Value = PlayerPrefs.GetInt("Gold", gold.Value);
        experience.Value = PlayerPrefs.GetInt("XP", experience.Value);
        totalDistance.Value = PlayerPrefs.GetFloat("Distance", totalDistance.Value);
        level.Value = PlayerPrefs.GetInt("Level", level.Value);
        lootGold.Value = PlayerPrefs.GetInt("LootGold", lootGold.Value);
        StoryOverlord.currentLevel = PlayerPrefs.GetInt("StoryLevel", StoryOverlord.currentLevel);
        loadEquippedItems();
    }

    private void loadEquippedItems() {
        string equipped;
        if (PlayerPrefs.HasKey(EQUIPPED_WEAPON)) {
            equipped = PlayerPrefs.GetString(EQUIPPED_WEAPON);
            equipWeapon(ItemList.itemMasterList[equipped]);
        }
        if (PlayerPrefs.HasKey(EQUIPPED_ARMOR)) {
            equipped = PlayerPrefs.GetString(EQUIPPED_ARMOR);
            equipArmor(ItemList.itemMasterList[equipped]);
        }
        if (PlayerPrefs.HasKey(EQUIPPED_ACCESSORY)) {
            equipped = PlayerPrefs.GetString(EQUIPPED_ACCESSORY);
            equipAccessory(ItemList.itemMasterList[equipped]);
            ItemList.lastAttrib=PlayerPrefs.GetFloat(LAST_ATTRIB);
            if (PlayerPrefs.GetInt(LAST_TYPE)==(int) BuffManager.BuffType.attack) {
                ItemList.lastBuff = BuffManager.BuffType.attack;
            }
            if (PlayerPrefs.GetInt(LAST_TYPE)==(int) BuffManager.BuffType.crit) {
                ItemList.lastBuff = BuffManager.BuffType.crit;
            }
            if (PlayerPrefs.GetInt(LAST_TYPE)==(int) BuffManager.BuffType.defense) {
                ItemList.lastBuff = BuffManager.BuffType.defense;
            }
            if (PlayerPrefs.GetInt(LAST_TYPE)==(int) BuffManager.BuffType.fire) {
                ItemList.lastBuff = BuffManager.BuffType.fire;
            }
            if (PlayerPrefs.GetInt(LAST_TYPE)==(int) BuffManager.BuffType.heal) {
                ItemList.lastBuff = BuffManager.BuffType.heal;
            }
            if (PlayerPrefs.GetInt(LAST_TYPE)==(int) BuffManager.BuffType.health) {
                ItemList.lastBuff = BuffManager.BuffType.health;
            }
        }

    }



    public void loadOthers() {
        Inventory.load();
        PotionInventory.load();
        EnemyWatchdog.instance.loadQueue();
    }

    public void clearStats() {
        PopUp.instance.showPopUp("Are you sure you want to reset your players stats permanently?",
            new string[] { "Yes", "No" },
            new System.Action[] {
                new System.Action(()=> {
                    PlayerPrefs.DeleteAll();

                    PotionInventory.clear();
                    Inventory.clear();
                    StoryOverlord.currentLevel = 0;

                    health.Value = 100;
                    maxHealth = 110;
                    gold.Value = 0;
                    experience.Value = 0;
                    totalDistance.Value = 0;
                    level.Value = 0;
                    equippedWeapon = ItemList.noItem;
                    equippedArmor = ItemList.noItem;
                    equippedAccessory = ItemList.noItem;
                    
                    
                    savePlayer();
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
