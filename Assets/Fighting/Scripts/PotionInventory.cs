using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Gamelogic.Extensions;

public class PotionInventory : MonoBehaviour {
    public static ObservedValue<int> numHealthPots;
    public static ObservedValue<int> numCritPots;
    public static ObservedValue<int> numAttackPots;

    public Text healthPotsText, critPotsText, attackPotsText;

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
        } else {
            return;
        }

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
        } else {
            return;
        }

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
}
