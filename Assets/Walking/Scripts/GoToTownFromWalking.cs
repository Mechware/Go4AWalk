using UnityEngine;
using System.Collections;

public class GoToTownFromWalking : MonoBehaviour {

    // Method for going into town and ending the quest
    public void goToTown() {
        if (Questing.currentQuest.name.Equals("Forest")) {
            Questing.endQuest(true);
        } else {
            print("Went to town via tavern and gave up on quest");
            Questing.endQuest(false);
        }
    }
}
