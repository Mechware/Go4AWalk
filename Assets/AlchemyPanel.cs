using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlchemyPanel : MonoBehaviour {

    public Text redLeafText, yellowLeafText, blueLeafText;
    public Text healthPrice, critPrice, attackPrice;
    public Button makeHealth, makeCrit, makeAttack;
    private int healthPotPrice = 0, critPotPrice = 0, attackPotPrice = 0;

    item healthPot;
    item critPot;
    item attackPot;
    item redLeaf;
    item yellowLeaf;
    item blueLeaf;

    // Use this for initialization
    void Start() {
        healthPot = ItemList.itemMasterList[ItemList.HEALTH_POTION];
        critPot = ItemList.itemMasterList[ItemList.CRIT_POTION];
        attackPot = ItemList.itemMasterList[ItemList.ATTACK_POTION];
        redLeaf = ItemList.itemMasterList[ItemList.RED_LEAF];
        yellowLeaf = ItemList.itemMasterList[ItemList.YELLOW_LEAF];
        blueLeaf = ItemList.itemMasterList[ItemList.BLUE_LEAF];

        healthPotPrice = 10 * Player.level.Value;
        critPotPrice = 20 * Player.level.Value;
        attackPotPrice = 30 * Player.level.Value;

        healthPrice.text = "Health Potion" + "\n" + "        : x3" + "\n\n" + "      : x" + healthPotPrice;
        critPrice.text = "Crit Potion" + "\n" + "        : x4" + "\n\n" + "      : x" + critPotPrice;
        attackPrice.text = "Attack Potion" + "\n" + "        : x5" + "\n\n" + "      : x" + attackPotPrice;
    }

    // Update is called once per frame
    void Update() {
        redLeafText.text = "x" + PotionInventory.numRedLeaf;
        yellowLeafText.text = "x" + PotionInventory.numYellowLeaf;
        blueLeafText.text = "x" + PotionInventory.numBlueLeaf;

    }

    public void makeHealthPot() {
        if (PotionInventory.numRedLeaf >= 3 && Player.gold.Value >= healthPotPrice) {
            Player.takeGold(healthPotPrice);
            PotionInventory.removeLeaf(redLeaf, 3);
            PotionInventory.addPotion(healthPot);
        } else {
            print("You dont have the necessary ingredients!");
        };
    }
    public void makeCritPot() {
        if (PotionInventory.numYellowLeaf >= 4 && Player.gold.Value >= critPotPrice) {
            Player.takeGold(critPotPrice);
            PotionInventory.removeLeaf(yellowLeaf, 4);
            PotionInventory.addPotion(critPot);
        } else {
            print("You dont have the necessary ingredients!");
        };
    }
    public void makeAttackPot() {
        if (PotionInventory.numBlueLeaf >= 5 && Player.gold.Value >= attackPotPrice) {
            Player.takeGold(attackPotPrice);
            PotionInventory.removeLeaf(blueLeaf, 5);
            PotionInventory.addPotion(attackPot);
        } else {
            print("You dont have the necessary ingredients!");
        };
    }
}
