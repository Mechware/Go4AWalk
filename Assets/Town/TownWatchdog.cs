using UnityEngine;
using System.Collections;

public class TownWatchdog : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void toForest() {
        int goldReward = 100;
        int xpReward = 15;
        float timeToComplete = -1;
        float distance = 10;
        float difficulty = 1;
        quest thisQuest = new quest("Test Quest", goldReward, xpReward, timeToComplete, distance, difficulty);
        thisQuest.active = true;
        Player.startQuest(thisQuest);
    }

    public void openTavern() {
        int goldReward = 100;
        int xpReward = 15;
        float timeToComplete = 10;
        float distance = 10;
        float difficulty = 1;
        quest thisQuest = new quest("Test Quest", goldReward, xpReward, timeToComplete, distance, difficulty);
        Player.startQuest(thisQuest);
    }

    public void openShop() {

    }

    public void openBounties() {

    }
}
