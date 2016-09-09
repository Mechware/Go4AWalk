using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TownWatchdog : MonoBehaviour {

    public GameObject menus;
    public GameObject exitButton;

    // Bounty board
    public GameObject bountyBoard, bBBackPanel, bBQuests;
    public GameObject bBQuestButton0, bBQuestButton1, bBQuestButton2, bBQuestButton3, bBQuestButton4, bBQuestButton5, bBQuestButton6;
    public GameObject questInfoPanel;
    public Text questPageTitle, questPageOverview, questPageDescription;
    public Text questPageStats;

    // Tavern
    public GameObject tavern;

    // Shop
    public GameObject shop;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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

    public void openTavern() {
        int goldReward = 100;
        int xpReward = 15;
        float timeToComplete = 10;
        float distance = 10;
        float difficulty = 1;
        quest thisQuest = new quest("Test Quest", "Overview goes here!", "This is a description! lalalalala", goldReward, xpReward, null, timeToComplete, distance, difficulty);
        Questing.startQuest(thisQuest);
    }

    public void openShop() {

    }

    public void openBounties() {
        setQuests();
        menus.SetActive(true);
        bountyBoard.SetActive(true);
        exitButton.SetActive(true);
    }

    public void openQuestInBounties(quest q) {
        questPageTitle.text = q.name;
        questPageOverview.text = q.shortOverview;
        questPageDescription.text = q.description;
        questPageStats.text = "Distance To Travel: " + q.distance + " m\n" +
                              "Time to complete: " + q.timeToComplete + " s\n" +
                              "Average Speed: " + q.distance/q.timeToComplete + "m/s\n" +
                              "XP Reward: " + q.xpReward + "\n" +
                              "Gold Reward: " + q.goldReward;
        questInfoPanel.SetActive(true);
    }

    public void acceptQuest() {
        if(currentQuestNum == -1) {
            print("No quest selected");
            return;
        }
        Questing.startQuest(quests[currentQuestNum]);
    }

    public void ignoreQuest() {
        questInfoPanel.SetActive(false);
    }

    public void selectQuest(int buttonNum) {
        openQuestInBounties(quests[buttonNum]);
        currentQuestNum = buttonNum;
    }

    quest[] quests;
    int currentQuestNum=-1;

    void setQuests() {

        quests = new quest[7];
        for(int i = 0 ; i < 7 ; i++) {
            quests[i] = Questing.createRandomQuest();
        }

        bBQuestButton0.GetComponentInChildren<Text>().text = quests[0].getButtonText();
        bBQuestButton1.GetComponentInChildren<Text>().text = quests[1].getButtonText();
        bBQuestButton2.GetComponentInChildren<Text>().text = quests[2].getButtonText();
        bBQuestButton3.GetComponentInChildren<Text>().text = quests[3].getButtonText();
        bBQuestButton4.GetComponentInChildren<Text>().text = quests[4].getButtonText();
        bBQuestButton5.GetComponentInChildren<Text>().text = quests[5].getButtonText();
        bBQuestButton6.GetComponentInChildren<Text>().text = quests[6].getButtonText();
    }

    public void closeButton() {
        menus.SetActive(false);
    }
}
