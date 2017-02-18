using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PotionInventory : MonoBehaviour {
    public static int numHealthPots;
    public static int numCritPots;
    public static int numAttackPots;
 
    public Text healthPots, critPots, attackPots;

    public static item healthPot;
    public static item critPot;
    public static item attackPot;

    // Use this for initialization
    void Start() {
        healthPot = ItemList.itemMasterList[ItemList.HEALTH_POTION];
        critPot = ItemList.itemMasterList[ItemList.CRIT_POTION];
        attackPot = ItemList.itemMasterList[ItemList.ATTACK_POTION];
        healthPots.text = "x" + numHealthPots;
        critPots.text = "x" + numCritPots;
        attackPots.text = "x" + numAttackPots;
        
    }

    // Update is called once per frame
    void Update() {
        healthPots.text = "x" + numHealthPots;
        critPots.text = "x" + numCritPots;
        attackPots.text = "x" + numAttackPots;
    }

    public static void addPotion(item item) {
        if (item.name == "Health Potion") {
            if (numHealthPots >= 99) {
                numHealthPots = 99;
            } else {
                numHealthPots++;
            }
        } else if (item.name == "Crit Potion") {
            if (numCritPots >= 99) {
                numCritPots = 99;
            } else {
                numCritPots++;
            }
        } else if (item.name == "Attack Potion") {
            if (numAttackPots >= 99) {
                numAttackPots = 99;
            } else {
                numAttackPots++;
            }
        } else {
            return;
        }

    }

    public static void removePotion(item item) {
        if (item.name == "Health Potion") {
            if (numHealthPots <= 0)
                numHealthPots = 0;
            else
                numHealthPots--;
        } else if (item.name == "Crit Potion") {
            if (numCritPots <= 0)
                numCritPots = 0;
            else
                numCritPots--;
        } else if (item.name == "Attack Potion") {
            if (numAttackPots <= 0)
                numAttackPots = 0;
            else
                numAttackPots--;
        } else {
            return;
        }

    }

    public static int getNumPotions(item item) {
        if (item.name == "Health Potion") {
            return numHealthPots;
        } else if (item.name == "Crit Potion") {
            return numCritPots;
        } else if (item.name == "Attack Potion") {
            return numAttackPots;
        } else {
            return 0;
        }
    }

    public void useHealthPot() {
        healthPot.useItem();
        removePotion(healthPot);
    }
    public void useCritPot() {
        critPot.useItem();
        removePotion(critPot);
    }
    public void useAttackPot() {
        attackPot.useItem();
        removePotion(attackPot);
    }

}
