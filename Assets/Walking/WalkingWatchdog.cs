using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class WalkingWatchdog : MonoBehaviour {


    // UI for walking
    public Vector2 startPosition, endPosition;
    public GameObject characterSprite;


    public GameObject goToTownPanel, slowDownAlertObject;
    private static WalkingWatchdog objectWatchdog;
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
        objectWatchdog = this;
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

            // Update player walking on screen (could probably do this in coroutine)
            if(Questing.currentQuest.distance != -1 && Questing.currentQuest.distance != 0) {
                float xDistanceDifference = endPosition.x - startPosition.x;
                float yDistanceDifference = endPosition.y - startPosition.y;
                float percentToEnd = Questing.currentQuest.distanceProgress/Questing.currentQuest.distance;
                xDistanceDifference *= percentToEnd;
                yDistanceDifference *= percentToEnd;
                characterSprite.transform.position = new Vector2(startPosition.x + xDistanceDifference, startPosition.y + yDistanceDifference);
            }
        }
    }

    IEnumerator slowDownAlertFade(string speed) {
        slowDownAlertObject.SetActive(true);
        slowDownAlertObject.GetComponentInChildren<Text>().text = "Slow the fuck down speed racer!\n" +
                                                              "You were going " + speed + " km/h";
        yield return new WaitForSeconds(5);
        slowDownAlertObject.SetActive(false);
    }

    public static void slowTheFuckDownAlert(string speed) {
        objectWatchdog.StartCoroutine("slowDownAlertFade", speed);
    }

    // ** For walking ** //
    // This is called from EnemyWatchdog to enable random encounters and the "encounter" method
    // is usually just the spawnEnemy() method in EnemyWatchdog
    public void enableRandomEncounter(Action encounter) {
        randomEncounterButton.GetComponentInChildren<Text>().text = "Random Encounter";
        randomEncounterButton.gameObject.SetActive(true);
        randomEncounterButton.GetComponent<Button>().onClick.AddListener(() => {
            encounter();
        });
    }

    // This is called from EnemyWatchdog to enable boss encounters and the "encounter" method
    // is usually just the spawnBoss() method in EnemyWatchdog
    public void enableBossEncounter(Action encounter) {
        randomEncounterButton.GetComponentInChildren<Text>().text = "BOSS FIGHT";
        randomEncounterButton.gameObject.SetActive(true);
        randomEncounterButton.GetComponent<Button>().onClick.RemoveAllListeners();
        randomEncounterButton.GetComponent<Button>().onClick.AddListener(() => {
            encounter();
        });
    }

    // Used for confirming going to town
    public void confirmGoToTown() {
        goToTownPanel.SetActive(true);
    }

    // Used to close the "Go into town?" panel
    public void closeGoToTown() {
        goToTownPanel.SetActive(false);
    }

    // Method for going into town and ending the quest
    public void goToTown() {
        if(Questing.currentQuest.name.Equals("Forest")) {
            Questing.endQuest(true);
        } else {
            print("Went to town via tavern and gave up on quest");
            Questing.endQuest(false);
        }
    }
    // ** End walking ** //
}
