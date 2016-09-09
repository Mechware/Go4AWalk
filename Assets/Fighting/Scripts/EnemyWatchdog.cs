using UnityEngine;
using System.Collections;

public enum encounterState {
    Fight,
    Loot,
    None
}

public class EnemyWatchdog : MonoBehaviour {


    public GameObject[] enemies;
    public GameObject randomEncounterButton;
    public WalkingWatchdog walkingWatchdogUI;
    public static GameObject currentEnemy;

    // Distance will be anywhere from this distance to 10 times this distance
    public static float randomEncounterDistance = 10f; 

    private static float lastEncounterTime;
    private static float lastEncounterDistance;
    private static float nextEncounterDistance;
    private static encounterState state;
    private static GameObject currentEnemyPrefab;
    

    void Awake() {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("FightingScene")) {
            // Spawn enemy decided in the walking screen
            currentEnemy = Instantiate(currentEnemyPrefab);
        } else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("WalkingScreen")) {

            // Initialize encounter variables
            lastEncounterDistance = Player.totalDistance;
            nextEncounterDistance = lastEncounterDistance + Random.Range(1f, 10f);
            lastEncounterTime = Time.time;

            // Not encountering anything
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
            if (state == encounterState.None) {
                if (Player.totalDistance > nextEncounterDistance) {
                    walkingWatchdogUI.enableRandomEncounter(spawnEnemy);
                    nextEncounterDistance = lastEncounterDistance + randomEncounterDistance*Random.Range(1f, 10f);
                }
            }
        } else if (Player.fighting) {
            if(currentEnemy == null) {
                encounterIsOver();
            }
        }
	    
	}

    public void encounterIsOver() {
        switch(state) {
            case encounterState.Fight:
                endFight();
                break;
            case encounterState.None:
                print("ERROR: Encounter of nothing ended");
                break;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("WalkingScreen");
    }

    private void endFight() {
        currentEnemy = null;
        Player.resetCrit();
        lastEncounterDistance = Player.totalDistance;
        state = encounterState.None;
    }


    private void spawnEnemy() {
        if(state != encounterState.None) {
            return;
        }

        currentEnemyPrefab = pickEnemy();
        state = encounterState.Fight;
        UnityEngine.SceneManagement.SceneManager.LoadScene("FightingScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private GameObject pickEnemy() {
        return enemies[0];
    }
}
