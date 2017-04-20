using UnityEngine;
using System.Collections;
using System;

public class GoToCampFromWalking : MonoBehaviour {
    void Update() {
           }

    // Method for going into town and ending the quest
    public void goToCamp() {
    
        if(EnemyWatchdog.enemiesQueue.Count != 0) {
            PopUp.instance.showPopUp("You cannot sleep while monsters are following you!", new string[] { "Okay" });
            return;
        }
        PopUp.instance.showPopUp("Would you like to make camp for the night?",
            new string[] { "Yes", "No" },
            new Action[] { makeCamp, () => { } });
    }

    private void makeCamp() {
        Questing.makeCamp();
    }
}