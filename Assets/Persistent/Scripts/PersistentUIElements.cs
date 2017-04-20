using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class PersistentUIElements : MonoBehaviour {

    // Persistent stats bar
    public Text totalExperience, totalGold, lootGold, level, distance;

    // Journal
    public Text walkingStats, questStats;
    public GameObject JournalMenu, JournalButton;
    public GameObject statsPanel, questPanel, optionsPanel, itemsPanel;
    public string currentPanel = "";
    public Sprite panelBackground1, panelBackground2, panelBackground3, panelBackground4;

    // Inventory Stuff
    public Text equippedWeapon, equippedArmor, equippedAccessory, playerStats;
    public Image equippedItem1, equippedItem2, equippedItem3;

    public static PersistentUIElements instance;

    //Prefabs
    public GameObject item;

    // Script for walking data
    private GPS walkingScript;

    void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start() {
        walkingScript = GPS.gpsObject;

        // Set the stats
        level.text = Player.level.Value.ToString();
        totalGold.text = Player.gold.Value.ToString();
        lootGold.text = "+" + Player.lootGold.Value.ToString();
        totalExperience.text = Player.experience.Value.ToString();
        distance.text = Math.Round(Player.totalDistance.Value / 1000, 1).ToString();

        // Update each time the experience, gold, or level is updated
        Player.experience.OnValueChange += () => {
            totalExperience.text = Player.experience.Value.ToString();
        };
        Player.gold.OnValueChange += () => {
            totalGold.text = Player.gold.Value.ToString();
        };
        Player.lootGold.OnValueChange += () => {
            lootGold.text = "+" + Player.lootGold.Value.ToString();
        };
        Player.level.OnValueChange += () => {
            level.text = Player.level.Value.ToString();
        };
        Player.totalDistance.OnValueChange += () => {
            distance.text = Math.Round(Player.totalDistance.Value / 1000, 1).ToString();
        };
    }

    // Update is called once per frame
    void Update() {
        if (walkingStats.IsActive()) {
            if (walkingScript == null) {
                if (!walkingStats.Equals("GPS Disabled"))
                    walkingStats.text = "GPS Disabled";
            } else {
                walkingStats.text = walkingScript.getGPSData();
            }
        }

        if (questStats.IsActive()) {
            questStats.text = Questing.currentQuest.getStats();
        }
    }

    #region journal

    public void openJournal() {

        JournalMenu.SetActive(true);
        GameState.pause();
        JournalButton.SetActive(false);
        if (currentPanel.Equals("")) {
            currentPanel = "Quests";
            statsPanel.SetActive(false);
            questPanel.SetActive(true);
            optionsPanel.SetActive(false);
            itemsPanel.SetActive(false);
            JournalMenu.GetComponent<Image>().sprite = panelBackground1;
        }
    }

    public void closeJournal() {
        JournalButton.SetActive(true);
        JournalMenu.SetActive(false);
        GameState.resume();
    }

    // Used for opening panels via buttons
    public void openPanel(string panel) {

        switch (panel) {
            case "Stats":
                statsPanel.SetActive(true);
                questPanel.SetActive(false);
                optionsPanel.SetActive(false);
                itemsPanel.SetActive(false);
                currentPanel = panel;
                JournalMenu.GetComponent<Image>().sprite = panelBackground3;

                break;
            case "Options":
                statsPanel.SetActive(false);
                questPanel.SetActive(false);
                optionsPanel.SetActive(true);
                itemsPanel.SetActive(false);
                currentPanel = panel;
                JournalMenu.GetComponent<Image>().sprite = panelBackground4;
                break;
            case "Quests":
                statsPanel.SetActive(false);
                questPanel.SetActive(true);
                optionsPanel.SetActive(false);
                itemsPanel.SetActive(false);
                currentPanel = panel;
                JournalMenu.GetComponent<Image>().sprite = panelBackground1;
                break;
            case "Items":
                statsPanel.SetActive(false);
                questPanel.SetActive(false);
                optionsPanel.SetActive(false);
                itemsPanel.SetActive(true);
                currentPanel = panel;
                updateItems();
                JournalMenu.GetComponent<Image>().sprite = panelBackground2;
                break;
            default:
                print("Didn't not open a valid panel");
                break;
        }
    }

    private List<GameObject> inventoryButtons;

    public void updateItems() {

        if (GameState.walking) {
            if(WalkingWatchdog.instance != null)
                WalkingWatchdog.instance.setEquippedItemIcon();
        }

        GameObject currentButton;
        if (inventoryButtons == null) {
            inventoryButtons = new List<GameObject>();
        } else {
            while (inventoryButtons.Count > 1) {
                currentButton = inventoryButtons[1];
                inventoryButtons.RemoveAt(1);
                DestroyImmediate(currentButton);
            }
            if (inventoryButtons.Count == 1)
                inventoryButtons.RemoveAt(0);

        }

        if (Player.equippedWeapon.Equals(ItemList.noItem) || Player.equippedWeapon.Equals(default(item))) {
            equippedWeapon.text = "Weapon:" + "\n" + "None";
        } else {
            equippedWeapon.text = "Weapon:" + "\n" + Player.equippedWeapon.name.ToString();
            equippedItem1.sprite = Player.equippedWeapon.icon;
        }

        if (Player.equippedArmor.Equals(ItemList.noItem) || Player.equippedArmor.Equals(default(item))) {
            equippedArmor.text = "Armor:" + "\n" + "None";
        } else {
            equippedArmor.text = "Armor:" + "\n" + Player.equippedArmor.name.ToString();
            equippedItem2.sprite = Player.equippedArmor.icon;
        }

        if (Player.equippedAccessory.Equals(ItemList.noItem) || Player.equippedAccessory.Equals(default(item))) {
            equippedAccessory.text = "Accessory:" + "\n" + "None";
        } else {
            equippedAccessory.text = "Accessory:" + "\n" + Player.equippedAccessory.name.ToString();
            equippedItem3.sprite = Player.equippedAccessory.icon;
        }

        playerStats.text =
                "ATK:  " + Player.attackStrength + "\n" +
                "DEF:  " + Mathf.Round((1 - Player.defenseModifier) * 100) + "\n" + //Defence now shows how much damage the player avoids with a max of 99; 
                "HP:   " + Player.getMaxHealth() + "\n" +
                "CRIT: " + Player.critModifier + "\n" +
                "PWR:  " + Player.attackModifier;

        string itemDescription = "";
        string itemStats = "";
        Sprite icon;

        Button button = itemsPanel.GetComponentInChildren<Button>();
        RectTransform buttonTransform = button.gameObject.transform as RectTransform;
       
        currentButton = button.gameObject;



        if (Inventory.items.Count == 0) {
            button.GetComponentInChildren<Text>().text = "Inventory is empty!";
            return;
        }

        Vector3 buttonPos = buttonTransform.localPosition;
        float buttonHeight = buttonTransform.sizeDelta.y + 10f;

        for (int i = 0 ; i < Inventory.items.Count ; i++) {
            inventoryButtons.Add(currentButton);
            itemDescription = Inventory.items[i].name + "\n" + Inventory.items[i].description + "\n";
            if(Inventory.items[i].type == itemType.Weapon) {
                itemStats =
                "ATK:  " + Inventory.items[i].baseAttack + "\n" +
                "CRIT: " + Inventory.items[i].critModifier + "\n" +
                "PWR:  " + Inventory.items[i].attackModifier;
            } 
            else if(Inventory.items[i].type == itemType.Armor) {
                itemStats = 
                    "DEF:  " + Mathf.Round((1-Inventory.items[i].attributeValue)*100);
                    
            }
            
            icon = Inventory.items[i].icon;
            currentButton.GetComponentsInChildren<Image>()[1].sprite = icon;
            currentButton.GetComponentsInChildren<Text>()[1].text = itemStats;
            currentButton.GetComponentInChildren<Text>().text = itemDescription;
            setButtonClick(currentButton.GetComponent<Button>(), Inventory.items[i]);
            currentButton = Instantiate(button.gameObject, button.transform.parent) as GameObject;
            buttonPos.y -= buttonHeight;
            currentButton.transform.localPosition = buttonPos;
        }
        inventoryButtons.Remove(currentButton);
        Destroy(currentButton);
    }

    void setButtonClick(Button button, item it) {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(new UnityEngine.Events.UnityAction(() => {
            if(SellingInventory.currentlySellingItems) {
                SellingInventory.sell(it);
            } else {
                it.useItem();
            }
        }));
    }
    #endregion

}
