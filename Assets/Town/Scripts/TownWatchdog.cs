﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
        forestPopUp.SetActive(true);
    }

    public void closeForestPopUp() {
        forestPopUp.SetActive(false);
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

   

    public void closeButton() {
        menus.SetActive(false);
    }



}
