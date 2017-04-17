using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Gamelogic.Extensions;

public class PotionInventory : MonoBehaviour {
    public static ObservedValue<int> numHealthPots;
    public static ObservedValue<int> numCritPots;
    public static ObservedValue<int> numAttackPots;

    public static int numRedLeaf, numYellowLeaf, numBlueLeaf;

    public Text healthPotsText, critPotsText, attackPotsText;
    public Text redLeafText, yellowLeafText, blueLeafText;

    public static item healthPot;
    public static item critPot;
    public static item attackPot;

    public AudioClip health, crit, attack;
    public AudioSource sound;

    // Use this for initialization
    void Awake() {
        if (numHealthPots == null)
            numHealthPots = new ObservedValue<int>(0);
        else
            numHealthPots = new ObservedValue<int>(numHealthPots.Value);

        if (numCritPots == null)
            numCritPots = new ObservedValue<int>(0);
        else
            numCritPots = new ObservedValue<int>(numCritPots.Value);

        if (numAttackPots == null)
            numAttackPots = new ObservedValue<int>(0);
        else
            numAttackPots = new ObservedValue<int>(numAttackPots.Value);

        healthPot = ItemList.itemMasterList[ItemList.HEALTH_POTION];
        critPot = ItemList.itemMasterList[ItemList.CRIT_POTION];
        attackPot = ItemList.itemMasterList[ItemList.ATTACK_POTION];

        if (healthPotsText != null) {
            healthPotsText.text = "x" + numHealthPots.Value;
            numHealthPots.OnValueChange += () => {
                healthPotsText.text = "x" + numHealthPots.Value;
            };
        }

        if (critPotsText != null) {
            critPotsText.text = "x" + numCritPots.Value;
            numCritPots.OnValueChange += () => {
                critPotsText.text = "x" + numCritPots.Value;
            };
        }

        if (attackPotsText != null) {
            attackPotsText.text = "x" + numAttackPots.Value;
            numAttackPots.OnValueChange += () => {
                attackPotsText.text = "x" + numAttackPots.Value;
            };
        }
    }

    public static void addLeaf(item item) {
        if (item.name == "Red Leaf") {
            if (numRedLeaf >= 99) {
                numRedLeaf = 99;
            } else {
                numRedLeaf++;
            }
        } else if (item.name == "Yellow Leaf") {
            if (numYellowLeaf >= 99) {
                numYellowLeaf = 99;
            } else {
                numYellowLeaf++;
            }
        } else if (item.name == "Blue Leaf") {
            if (numBlueLeaf >= 99) {
                numBlueLeaf = 99;
            } else {
                numBlueLeaf++;
            }
        }
        save();
    }

    public static void removeLeaf(item item, int num) {
        if (item.name == "Red Leaf") {
            if (numRedLeaf <= 0) {
                numRedLeaf = 0;
            } else {
                numRedLeaf = numRedLeaf - num;
            }
        } else if (item.name == "Yellow Leaf") {
            if (numYellowLeaf <= 0) {
                numYellowLeaf = 0;
            } else {
                numYellowLeaf = numYellowLeaf - num;
            }
        } else if (item.name == "Blue Leaf") {
            if (numBlueLeaf <= 0) {
                numBlueLeaf = 0;
            } else {
                numBlueLeaf = numBlueLeaf - num;
            }
        }
        save();
    }

    public static void addPotion(item item) {
        if (item.name == "Health Potion") {
            if (numHealthPots.Value >= 99) {
                numHealthPots.Value = 99;
            } else {
                numHealthPots.Value++;
            }
        } else if (item.name == "Crit Potion") {
            if (numCritPots.Value >= 99) {
                numCritPots.Value = 99;
            } else {
                numCritPots.Value++;
            }
        } else if (item.name == "Attack Potion") {
            if (numAttackPots.Value >= 99) {
                numAttackPots.Value = 99;
            } else {
                numAttackPots.Value++;
            }
        }
        save();

    }

    public static void removePotion(item item) {
        if (item.name == "Health Potion") {
            if (numHealthPots.Value <= 0)
                numHealthPots.Value = 0;
            else
                numHealthPots.Value--;
        } else if (item.name == "Crit Potion") {
            if (numCritPots.Value <= 0)
                numCritPots.Value = 0;
            else
                numCritPots.Value--;
        } else if (item.name == "Attack Potion") {
            if (numAttackPots.Value <= 0)
                numAttackPots.Value = 0;
            else
                numAttackPots.Value--;
        }
        save();

    }

    public static int getNumPotions(item item) {
        if (item.name == "Health Potion") {
            return numHealthPots.Value;
        } else if (item.name == "Crit Potion") {
            return numCritPots.Value;
        } else if (item.name == "Attack Potion") {
            return numAttackPots.Value;
        } else {
            return 0;
        }
    }

    public void useHealthPot() {
        if (numHealthPots.Value != 0) {
            sound.clip = health;
            sound.Play();
            healthPot.useItem();
            removePotion(healthPot);
        }

    }
    public void useCritPot() {
        if (numCritPots.Value != 0) {
            sound.clip = crit;
            sound.Play();
            critPot.useItem();
            removePotion(critPot);
        }

    }
    public void useAttackPot() {
        if (numAttackPots.Value != 0) {
            sound.clip = attack;
            sound.Play();
            attackPot.useItem();
            removePotion(attackPot);
        }
    }

    public static void load() {
        numHealthPots.Value = PlayerPrefs.GetInt("HealthPotions", numHealthPots.Value);
        numCritPots.Value = PlayerPrefs.GetInt("CritPotions", numCritPots.Value);
        numAttackPots.Value = PlayerPrefs.GetInt("AttackPotions", numAttackPots.Value);
        numRedLeaf = PlayerPrefs.GetInt("RedLeaf", numRedLeaf);
        numYellowLeaf = PlayerPrefs.GetInt("YellowLeaf", numYellowLeaf);
        numBlueLeaf = PlayerPrefs.GetInt("BlueLeaf", numBlueLeaf);
    }

    public static void save() {
        PlayerPrefs.SetInt("HealthPotions", numHealthPots.Value);
        PlayerPrefs.SetInt("CritPotions", numCritPots.Value);
        PlayerPrefs.SetInt("AttackPotions", numAttackPots.Value);
        PlayerPrefs.SetInt("RedLeaf", numRedLeaf);
        PlayerPrefs.SetInt("YellowLeaf", numYellowLeaf);
        PlayerPrefs.SetInt("BlueLeaf", numBlueLeaf);
        PlayerPrefs.Save();
    }

    public static void clear() {
        numHealthPots.Value = 0;
        numCritPots.Value = 0;
        numAttackPots.Value = 0;
        numBlueLeaf = 0;
        numRedLeaf = 0;
        numYellowLeaf = 0;
    }
}
