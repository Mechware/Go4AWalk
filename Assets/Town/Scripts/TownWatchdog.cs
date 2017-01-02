using UnityEngine;
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

    public void openTavern() {
        int goldReward = 100;
        int xpReward = 15;
        float timeToComplete = 10;
        float distance = 10;
        float difficulty = 1;
        quest thisQuest = new quest("Test Quest", "Overview goes here!", "This is a description! lalalalala", goldReward, xpReward, null, timeToComplete, distance, difficulty);
        Questing.startQuest(thisQuest);
    }

    public void closeButton() {
        menus.SetActive(false);
    }



}
