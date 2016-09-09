using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class WalkingWatchdog : MonoBehaviour {


    // UI for walking 
    public GameObject goToTownPanel;
    public Button takeStepButton, randomEncounterButton;
    public Text walkingStats, questDistanceTravelled;
    public Text questDistanceToTravel;
    

    // Use this for initialization
    void Start () {
        if (Player.walking) {
            takeStepButton.onClick.AddListener(() => {
                Player.updateDistance(1f);
            });
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (Player.walking) {
            
            if (Questing.currentQuest.distance == -1) {
                questDistanceToTravel.text = "∞";

            } else {
                questDistanceToTravel.text = Questing.currentQuest.distance/100 + "";
            }
            questDistanceTravelled.text = Questing.currentQuest.distanceProgress/100 + "";
        }
    }

    // ** For walking ** //
    public void enableRandomEncounter(Action encounter) {
        randomEncounterButton.gameObject.SetActive(true);
        randomEncounterButton.GetComponent<Button>().onClick.AddListener(() => {
            encounter();
        });
    }

    public void confirmGoToTown() {
        goToTownPanel.SetActive(true);
    }

    public void closeGoToTown() {
        goToTownPanel.SetActive(false);
    }

    public void goToTown() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TownScreen");
    }
    // ** End walking ** //
}
