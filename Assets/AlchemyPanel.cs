using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlchemyPanel : MonoBehaviour {

    public const int RED_LEAVES_FOR_HEALTH = 3;
    public const int YELLOW_LEAVES_FOR_CRIT = 4;
    public const int BLUE_LEAVES_FOR_POWER = 5;

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

    public AudioSource sound;
    public AudioClip success, negative;

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
        if (PotionInventory.numRedLeaf >= RED_LEAVES_FOR_HEALTH && 
            Player.gold.Value >= healthPotPrice) {

            Player.takeGold(healthPotPrice);
            PotionInventory.removeLeaf(redLeaf, RED_LEAVES_FOR_HEALTH);
            PotionInventory.addPotion(healthPot);
            sound.clip = success;
            sound.Play();
        } else {
            PopUp.instance.showPopUp("You don't have the neccessary items!", new string[] { "Okay" });
            sound.clip = negative;
            sound.Play();
        };
    }
    public void makeCritPot() {
        if (PotionInventory.numYellowLeaf >= YELLOW_LEAVES_FOR_CRIT && Player.gold.Value >= critPotPrice) {
            Player.takeGold(critPotPrice);
            PotionInventory.removeLeaf(yellowLeaf, YELLOW_LEAVES_FOR_CRIT);
            PotionInventory.addPotion(critPot);
            sound.clip = success;
            sound.Play();
        } else {
            PopUp.instance.showPopUp("You don't have the neccessary items!", new string[] { "Okay" });
            sound.clip = negative;
            sound.Play();
        };
    }
    public void makeAttackPot() {
        if (PotionInventory.numBlueLeaf >= BLUE_LEAVES_FOR_POWER && Player.gold.Value >= attackPotPrice) {
            Player.takeGold(attackPotPrice);
            PotionInventory.removeLeaf(blueLeaf, BLUE_LEAVES_FOR_POWER);
            PotionInventory.addPotion(attackPot);
            sound.clip = success;
            sound.Play();
        } else {
            PopUp.instance.showPopUp("You don't have the neccessary items!", new string[] { "Okay" });
            sound.clip = negative;
            sound.Play();
        };
    }
}
