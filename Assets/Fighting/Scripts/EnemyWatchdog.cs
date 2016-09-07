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

    private static GameObject[] privEnemies;
    private static float lastEncounterTime;
    private static float lastEncounterDistance;
    private static float nextEncounterDistance;
    private static encounterState state;


    void Awake() {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("FightingScene")) {
            // Spawn a random enemy from a list
            int index = Mathf.RoundToInt(Random.Range(0, privEnemies.Length-1));
            currentEnemy = Instantiate(privEnemies[index]);
        } else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("WalkingScreen")) {
            // Load static variable of enemies
            privEnemies = enemies;

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
                    nextEncounterDistance = lastEncounterDistance + 10f*Random.Range(1f, 10f);
                }
            }
        } else if (Player.fighting) {
            if(currentEnemy == null) {
                encounterIsOver();
            }
        }
	    
	}

    public static void encounterIsOver() {
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

    private static void endFight() {
        currentEnemy = null;
        Player.resetCrit();
        lastEncounterDistance = Player.totalDistance;
        state = encounterState.None;
    }


    private static void spawnEnemy() {
        if(state != encounterState.None) {
            return;
        }

        state = encounterState.Fight;
        UnityEngine.SceneManagement.SceneManager.LoadScene("FightingScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
