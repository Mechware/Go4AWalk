using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PersistentUIElements : MonoBehaviour {

    // Persistent stats bar
    public Text totalExperience, totalGold, level;

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
        Button[] buttons = itemsPanel.GetComponentsInChildren<Button>();

    }
	
	// Update is called once per frame
	void Update () {
        if (walkingStats.IsActive()) {
            if(walkingScript == null) {
                walkingStats.text = "GPS Disabled";
            } else {
                walkingStats.text = walkingScript.getGPSData();
            }
        }

        if (questStats.IsActive()) {
            questStats.text = Questing.currentQuest.getStats();
        }

        // TODO: Optimize so only changes when needed
        totalExperience.text = Player.experience + "";
        totalGold.text = Player.gold + "";
        level.text = Player.level + "";
    }

// **** JOURNAL STUFF **** //

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

    void updateItems() {

        print("Updating items");

        string s = "";

        Button[] buttons = itemsPanel.GetComponentsInChildren<Button>();

        for(int i = 0 ; i < buttons.Length ; i++) { 
            s = Inventory.getItem(i).name + "\n" + Inventory.getItem(i).description + "\n";
            buttons[i].GetComponentInChildren<Text>().text = s;
        }
    }
    // ** End Journal Stuffs ** //

}
