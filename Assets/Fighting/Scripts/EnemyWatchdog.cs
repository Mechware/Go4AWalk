using UnityEngine;
using System.Collections;

public enum encounterState {
    Fight,
    Boss,
    Loot,
    None
}

public class EnemyWatchdog : MonoBehaviour {


    public GameObject[] enemies;
    public GameObject randomEncounterButton;
    public WalkingWatchdog walkingWatchdogUI;
    public FightingWatchdog fw;
    public static GameObject currentEnemy;
    

    // Distance will be anywhere from this distance to 10 times this distance
    public static float randomEncounterDistance = 10f; 

    private static float lastEncounterDistance;
    private static float nextEncounterDistance;
    private static encounterState state;
    private static GameObject currentEnemyPrefab;
    private static bool bossEncounterAvailable = false;
	private int randomEnemy;

    void Awake() {
        bossEncounterAvailable = false;
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(Player.FIGHTING_LEVEL)) {
            if(currentEnemyPrefab != null) {
                // Spawn enemy decided in the walking screen
                currentEnemy = Instantiate(currentEnemyPrefab);
            } else {
                currentEnemy = Instantiate(enemies[0]);
            }
            
        } else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(Player.WALKING_LEVEL)) {

            // Initialize encounter variables
            lastEncounterDistance = Player.totalDistance;
            nextEncounterDistance = lastEncounterDistance + Random.Range(1f, 10f);

            state = encounterState.None;
        }
    }

	// Use this for initialization
	void Start () {
        
	}

    

	// Update is called once per frame
	void Update () {

        if(Player.walking) {
            // Check encounters 
            if (state == encounterState.None && !bossEncounterAvailable) {

                if (Questing.currentQuest.distance != -1 && Questing.currentQuest.distance <= Questing.currentQuest.distanceProgress) {
                    walkingWatchdogUI.enableBossEncounter(spawnBoss);
                    nextEncounterDistance = float.MaxValue;
                    bossEncounterAvailable = true;
                } else if (Player.totalDistance > nextEncounterDistance) {
                    walkingWatchdogUI.enableRandomEncounter(spawnEnemy);
                    nextEncounterDistance = lastEncounterDistance + randomEncounterDistance*Random.Range(1f, 10f);
                } 
            }
        }  
	}

    // Called to end an encounter
    public void encounterIsOver() {
        switch(state) {
            case encounterState.Fight:
                endFight();
                fw.endRegularFight();
                break;
            case encounterState.Boss:
                currentEnemy = null;
                StartCoroutine(fw.questFightEnd());
                return;
            case encounterState.None:
                print("ERROR: Encounter of nothing ended");
                break;
        }
        // Delay loading walking screen for a few seconds (for animations to finish)
        Invoke("goToWalkingScreen", 2);
    }

    private void goToWalkingScreen() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Player.WALKING_LEVEL);
    }

    private void endFight() {
        currentEnemy = null;
        lastEncounterDistance = Player.totalDistance;
        state = encounterState.None;
    }


    private void spawnEnemy() {
        if(state != encounterState.None) {
            return;
        }

        currentEnemyPrefab = pickEnemy();
        state = encounterState.Fight;
        UnityEngine.SceneManagement.SceneManager.LoadScene(Player.FIGHTING_LEVEL);
    }

    private void spawnBoss() {
        if (state != encounterState.None) {
            return;
        }

        currentEnemyPrefab = pickBoss();
        state = encounterState.Boss;
        UnityEngine.SceneManagement.SceneManager.LoadScene(Player.FIGHTING_LEVEL);
    }

    private GameObject pickEnemy() {
		int randomEnemy = Mathf.RoundToInt(Random.value*4);
		return enemies[randomEnemy];
    }

    private GameObject pickBoss() {
        return enemies[0];
    }
}
