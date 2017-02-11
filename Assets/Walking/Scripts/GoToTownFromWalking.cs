using UnityEngine;
using System.Collections;
using System;

public class GoToTownFromWalking : MonoBehaviour {
    void Update() {
        CheckInput.checkTapOrMouseDown(goToTown);
    }

    // Method for going into town and ending the quest
    public void goToTown(Collider2D touchedCollider) {
        if (touchedCollider != GetComponent<Collider2D>())
            return;

        PopUp.instance.showPopUp("Would you like to return to town?",
            new string[] { "Yes", "No" },            
            new Action[] { endQuest, () => { } });
    }

    private void endQuest() {
        if (Questing.currentQuest.name.Equals("Forest") || Questing.currentQuest.name.Equals("TestQuest")) {
            Questing.endQuest(true);
        } else {
            print("Went to town via tavern and gave up on quest");
            Questing.endQuest(false);
        }
    }
}
