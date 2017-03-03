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


    //Prefabs
    public GameObject item;

    // Script for walking data
    private GPS walkingScript;

    // Use this for initialization
    void Start () {
        walkingScript = GPS.gpsObject;

        Inventory.onValueChanged += updateItems;

        // Set the stats
        level.text = Player.level.Value.ToString();
        totalGold.text = Player.gold.Value.ToString();
        lootGold.text = "+" + Player.lootGold.Value.ToString();
        totalExperience.text = Player.experience.Value.ToString();
        distance.text = Math.Round(Player.totalDistance.Value/1000,1).ToString();
     
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
            distance.text = Player.totalDistance.Value.ToString();
        };
    }
	
	// Update is called once per frame
	void Update () {
        if (walkingStats.IsActive()) {
            if(walkingScript == null) {
                if(!walkingStats.Equals("GPS Disabled"))
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
        JournalButton.SetActive(false);
        if(currentPanel.Equals("")) {
            currentPanel = "Quests";
            statsPanel.SetActive(false);
            questPanel.SetActive(true);
            optionsPanel.SetActive(false);
            itemsPanel.SetActive(false);
        }
    }

    public void closeJournal() {
        JournalButton.SetActive(true);
        JournalMenu.SetActive(false);
    }

    // Used for opening panels via buttons
    public void openPanel(string panel) {
        
        switch(panel) {
            case "Stats":
                statsPanel.SetActive(true);
                questPanel.SetActive(false);
                optionsPanel.SetActive(false);
                itemsPanel.SetActive(false);
                currentPanel = panel;
                break;
            case "Options":
                statsPanel.SetActive(false);
                questPanel.SetActive(false);
                optionsPanel.SetActive(true);
                itemsPanel.SetActive(false);
                currentPanel = panel;
                break;
            case "Quests":
                statsPanel.SetActive(false);
                questPanel.SetActive(true);
                optionsPanel.SetActive(false);
                itemsPanel.SetActive(false);
                currentPanel = panel;
                break;
            case "Items":
                statsPanel.SetActive(false);
                questPanel.SetActive(false);
                optionsPanel.SetActive(false);
                itemsPanel.SetActive(true);
                currentPanel = panel;
                updateItems();
                break;
            default:
                print("Didn't not open a valid panel");
                break;
        }
    }

    private List<GameObject> inventoryButtons;

    void updateItems() {

        GameObject currentButton;
        if (inventoryButtons == null) {
            inventoryButtons = new List<GameObject>();
        } else {
            while (inventoryButtons.Count > 1) {
                currentButton = inventoryButtons[1];
                inventoryButtons.RemoveAt(1);
                DestroyImmediate(currentButton);
            }
            if(inventoryButtons.Count == 1)
                inventoryButtons.RemoveAt(0);
                
        }
        string s = "";

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
            s = Inventory.items[i].name + "\n" + Inventory.items[i].description + "\n";
            currentButton.GetComponentInChildren<Text>().text = s;
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
            it.useItem();
        }));
    }
    #endregion

}
