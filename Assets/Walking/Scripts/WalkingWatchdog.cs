using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class WalkingWatchdog : MonoBehaviour {


    // UI for walking
    public static WalkingWatchdog instance;
    public Vector2 startPosition, endPosition;
    public GameObject characterSprite;

    public GameObject goToTownPanel, slowDownAlertObject;
    public Button takeStepButton, randomEncounterButton, fightEnemiesButton;
    public Text walkingStats, questDistanceTravelled;
    public Text questDistanceToTravel;
    public Text enemyQueueCount;
    private GPS gps;
    

    // Use this for initialization
    void Start () {
        instance = this;
        gps = GetComponent<GPS>();

        // Make it so when the GPS changes, increase player distance is called
        gps.deltaDistance.OnValueChange += increasePlayerDistance;

        takeStepButton.onClick.AddListener(() => {
            // Move 1 meter per step click
            updateDistance(10, 10);
        });

        // Display the correct distance to travel
        if (Questing.currentQuest.distance == -1) {
            questDistanceToTravel.text = "∞";
        } else {
            if (Questing.currentQuest.distance > 1001f) {
                questDistanceToTravel.text = Questing.currentQuest.distance/1000 + "km";
            }
            else {
                questDistanceToTravel.text = Questing.currentQuest.distance + "m";
            }
            
        }

        updateDistanceTravelledUI();
    }
	
	// Update is called once per frame
	void Update () {
       
    }


    public void setQueueSize(int queueSize)
    {
        enemyQueueCount.text = "" + queueSize;
    }

    void increasePlayerDistance() {
        updateDistance(gps.deltaDistance.Value, gps.deltaTime);
    }

    /// <summary>
    /// This function is to be called whenever the distance the player has travelled is to be
    /// increased
    /// </summary>
    /// <param name="changeInDistance"> Change in distance since last update in meters</param>
    /// <param name="changeInTime"> Change of time since last update in seconds</param>
    public void updateDistance(float changeInDistance, float changeInTime) {
        
        UnityEngine.Assertions.Assert.AreNotEqual(0, changeInTime, "Moved a distance in zero time.");
        UnityEngine.Assertions.Assert.IsTrue(changeInDistance >= 0, "Distance moved is negative.");

        float speed = changeInDistance / changeInTime; // in m/s

        // Check if going too fast ( > 20 km/h || > 5.56 m/s)
        if (speed > 5.56f) {
            StartCoroutine(slowDownAlert(speed));
            return;
        }

        // Update player values
        Player.totalDistance.Value += changeInDistance;

        // Update quest values
        Questing.move(changeInDistance);

        updateDistanceTravelledUI();
    }

    /// <summary>
    /// Updates the user interface to reflect the changes in distance travelled
    /// </summary>
    void updateDistanceTravelledUI() {

        // Update questing distance travelled text
        questDistanceTravelled.text = "" + Questing.currentQuest.distanceProgress;

        // Update position of character sprite on screen
        if (Questing.currentQuest.distance == -1 || Questing.currentQuest.distance == 0) {
            characterSprite.transform.position = startPosition;
        } else {
            float percentToEnd = Questing.currentQuest.distanceProgress/Questing.currentQuest.distance;
            float xDistanceDifference = endPosition.x - startPosition.x;
            float yDistanceDifference = endPosition.y - startPosition.y;
            float xPos = xDistanceDifference * percentToEnd + startPosition.x;
            float yPos = yDistanceDifference * percentToEnd + startPosition.y;
            characterSprite.transform.position = new Vector2(xPos, yPos);
        }
    }

    /// <summary>
    /// Displays an alert telling the user to slow down
    /// </summary>
    /// <param name="speed">The speed the user was travelling</param>
    /// <returns></returns>
    IEnumerator slowDownAlert(float speed) {
        slowDownAlertObject.SetActive(true);
        speed *= 1000 / 3600;
        slowDownAlertObject.GetComponentInChildren<Text>().text = "Slow the fuck down speed racer!\n" +
                                                              "You were going " + speed + " km/h";

        yield return new WaitForSeconds(5);
        slowDownAlertObject.SetActive(false);
    }

    /// <summary>
    /// This is called from EnemyWatchdog to enable random encounters and the "encounter" method
    /// is usually just the spawnEnemy() method in EnemyWatchdog
    /// </summary>
    /// <param name="encounter">Function called once user clicks enemy</param>
    public void enableRandomEncounter(Action encounter) {
        randomEncounterButton.GetComponentInChildren<Text>().text = "Random Encounter";
        randomEncounterButton.gameObject.SetActive(true);
        randomEncounterButton.GetComponent<Button>().onClick.AddListener(() => {
            encounter();
        });
    }

    public void fightEnemiesFromQueue(Action encounter)
    {
        fightEnemiesButton.gameObject.SetActive(true);
        fightEnemiesButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            encounter();
        });

    }

    /// <summary>
    /// This is called from EnemyWatchdog to enable boss encounters and the "encounter" method
    /// is usually just the spawnBoss() method in EnemyWatchdog
    /// </summary>
    /// <param name="encounter">Function called once user clicks boss</param>
    public void enableBossEncounter(Action encounter) {
        randomEncounterButton.GetComponentInChildren<Text>().text = "BOSS FIGHT";
        randomEncounterButton.gameObject.SetActive(true);
        randomEncounterButton.GetComponent<Button>().onClick.RemoveAllListeners();
        randomEncounterButton.GetComponent<Button>().onClick.AddListener(() => {
            encounter();
        });
    }
}
