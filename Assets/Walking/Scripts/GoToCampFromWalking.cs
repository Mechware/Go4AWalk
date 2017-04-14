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
        PopUp.instance.showPopUp("WOULD YOU LIKE TO MAKE CAMP FOR THE NIGHT?",
            new string[] { "YES", "NO" },
            new Action[] { makeCamp, () => { } });
    }

    private void makeCamp() {
        Questing.makeCamp();
    }
}