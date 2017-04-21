using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class WalkingWatchdog : MonoBehaviour {


    // UI for walking
    public static WalkingWatchdog instance;
    public Vector2 startPosition, endPosition;
    public GameObject characterSprite;
    public GameObject equippedWeaponSprite, equippedArmorSprite, equippedAccessorySprite;
    public Material garden, grass, forest, hauntedForest;
    public GameObject goToTownPanel, slowDownAlertObject;
    public GameObject backgroundTexture;
    public Button takeStepButton, randomEncounterButton, fightEnemiesButton;
    public Text walkingStats, questDistanceTravelled;
    public Text questDistanceToTravel;
    public Text enemyQueueCount;
    private GPS gps;
    
    void Awake() {
        instance = this;
        gps = GetComponent<GPS>();

    }

    // Use this for initialization
    void Start () {

        checkQueueSize();
        Player.totalDistance.OnValueChange += checkQueueSize;


        // Make it so when the GPS changes, increase player distance is called
        gps.deltaDistance.OnValueChange += increasePlayerDistance;

        takeStepButton.onClick.AddListener(() => {
            // Move 1 meter per step click
            updateDistance(25, 25);
        });
    }
	
    public void setQuestStuff() {

        backgroundTexture.GetComponent<MeshRenderer>().material = StoryOverlord.walkingBackground;

        // Display the correct distance to travel
        if (Questing.currentQuest.distance == -1) {
            questDistanceToTravel.text = "∞";
        } else {
            if (Questing.currentQuest.distance > 1001f) {
                questDistanceToTravel.text = Questing.currentQuest.distance / 1000 + "km";
            } else {
                questDistanceToTravel.text = Questing.currentQuest.distance + "m";
            }

        }

        updateDistanceTravelledUI();
        setEquippedItemIcon();
    }
	// Update is called once per frame
	void Update () {
        
    }

    public void setEquippedItemIcon() {
        equippedWeaponSprite.GetComponent<SpriteRenderer>().sprite = Player.equippedWeapon.icon;
        if (!Player.equippedArmor.Equals(ItemList.noItem) && !Player.equippedArmor.Equals(default(item))) {
            equippedArmorSprite.GetComponentInChildren<SpriteRenderer>().enabled = true;            
            Texture2D texture = Resources.Load("EquippedItems/" + Player.equippedArmor.name) as Texture2D;
            Sprite armorIcon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            equippedArmorSprite.GetComponent<SpriteRenderer>().sprite = armorIcon;
        }
        if (!Player.equippedAccessory.Equals(ItemList.noItem) && !Player.equippedAccessory.Equals(default(item))) {
            equippedAccessorySprite.GetComponentInChildren<SpriteRenderer>().enabled = true;
            Texture2D texture = Resources.Load("EquippedItems/" + Player.equippedAccessory.name) as Texture2D;
            Sprite armorIcon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            equippedAccessorySprite.GetComponent<SpriteRenderer>().sprite = armorIcon;
        }

    }

    private void checkQueueSize() {
        enemyQueueCount.text = string.Format("{0}", EnemyWatchdog.enemiesQueue.Count);
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
            slowDownAlert(speed);
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
        questDistanceTravelled.text = "" + Mathf.RoundToInt(Questing.currentQuest.distanceProgress);

        // Update position of character sprite on screen
        if (Questing.currentQuest.distance == -1 || Questing.currentQuest.distance == 0) {
            //characterSprite.transform.position = startPosition;
        } /*else {
            float percentToEnd = Questing.currentQuest.distanceProgress/Questing.currentQuest.distance;
            float xDistanceDifference = endPosition.x - startPosition.x;
            float yDistanceDifference = endPosition.y - startPosition.y;
            float xPos = xDistanceDifference * percentToEnd + startPosition.x;
            float yPos = yDistanceDifference * percentToEnd + startPosition.y;
            characterSprite.transform.position = new Vector2(xPos, yPos);
        }*/
    }

    /// <summary>
    /// Displays an alert telling the user to slow down
    /// </summary>
    /// <param name="speed">The speed the user was travelling</param>
    /// <returns></returns>
    void slowDownAlert(float speed) {
        
        speed *= 1000 / 3600;
        PopUp.instance.showPopUp(string.Format("It seems like you're going too fast, please slow down for us to track you.\nYou were going {0} km\\h", speed), 
            new string[] { "Okay" });
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
