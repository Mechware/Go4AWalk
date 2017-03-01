using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TownWatchdog : MonoBehaviour {

    public GameObject menus;
    public GameObject exitButton;

    // Bounty board
    public GameObject bountyBoard;

    // Tavern
    public GameObject tavern;

    // Shop
    public GameObject shop;

    // 
    public GameObject forestPopUp;

    public void showForestPopUp() {
        //forestPopUp.SetActive(true);
        PopUp.instance.showPopUp("Are you sure you would like to go to the forest?", 
            new string[] { "Yes", "No" }, 
            new Action[] { toForest, () => { } }
            );
    }

    public void closeForestPopUp() {
        //forestPopUp.SetActive(false);
    }

    public void toForest() {
        
        int goldReward = 0;
        int xpReward = 0;
        float timeToComplete = -1;
        float distance = -1;
        float difficulty = 1;
        quest thisQuest = new quest("Test Quest", "You're exploring in the forest", "Find new enemies and get experience and gold!", goldReward, xpReward, null, timeToComplete, distance, difficulty);
        thisQuest.active = true;
        Questing.startQuest(thisQuest);
    }

   public void open(GameObject menu) {
        menu.SetActive(true);
        menus.SetActive(true);
        exitButton.SetActive(true);
    }

    public void closeButton() {
        menus.SetActive(false);
        //bountyBoard.SetActive(false);
        //tavern.SetActive(false);
        shop.SetActive(false);
    }



}
