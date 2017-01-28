using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
    private static EnemyQueue enemiesQueue;
    public Text enemiesLeft;
 

    // Distance will be anywhere from this distance to 10 times this distance
    public static float maxRandomEncounterDistance = 10f;

    private static float lastEncounterDistance;
    private static float nextEncounterDistance;
    private static GameObject currentEnemyPrefab;

    void Awake() {
        if (enemiesQueue == null) {
            enemiesQueue = new EnemyQueue(this);
        }
   
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(Player.FIGHTING_LEVEL)) {
            if (currentEnemyPrefab != null) {
                // Spawn enemy decided in the walking screen
                currentEnemy = Instantiate(currentEnemyPrefab);
            } else {
                currentEnemy = Instantiate(enemies[0]);
            }

        } else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(Player.WALKING_LEVEL)) {

            // Initialize encounter variables
            lastEncounterDistance = Player.totalDistance;
            nextEncounterDistance = lastEncounterDistance + Random.Range(1f, 10f);
        }
    }

    // Use this for initialization
    void Start() {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(Player.WALKING_LEVEL)) {
            walkingWatchdogUI.setQueueSize(enemiesQueue.getSize());
        }
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(Player.FIGHTING_LEVEL)) {
            enemiesLeft.text = "" + (enemiesQueue.getSize() + 1);
        }
    }


    // Update is called once per frame
    void Update() {

        if (Player.walking) {

            if (Questing.currentQuest.distance != -1 && Questing.currentQuest.distance <= Questing.currentQuest.distanceProgress) {
                nextEncounterDistance = float.MaxValue;
            } else if (Player.totalDistance > nextEncounterDistance) {
                enemiesQueue.putEnemy();
                walkingWatchdogUI.setQueueSize(enemiesQueue.getSize());
                nextEncounterDistance += maxRandomEncounterDistance * Random.Range(0f, 1f);
            }
        }
    }

    // Called to end an encounter
    public IEnumerator enemyHasDied() {
        if (enemiesQueue.IsEmpty()) {
            endFight();
            fw.endRegularFight();
            yield return new WaitForSeconds(2);
            goToWalkingScreen();
        } else {
            enemiesLeft.text = "" + enemiesQueue.getSize();
            yield return new WaitForSeconds(2);

            spawnEnemy();
        }
        
    }

    public void startFighting() {
        if (enemiesQueue.IsEmpty()) {
            print("There are no enemies to fight ya dummy!");
            return;
        } else {
            enemiesQueue.removeEnemy();
            UnityEngine.SceneManagement.SceneManager.LoadScene(Player.FIGHTING_LEVEL);
        }
    }

    private void goToWalkingScreen() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Player.WALKING_LEVEL);
    }


    private void endFight() {
        currentEnemy = null;
        lastEncounterDistance = Player.totalDistance;
    }

    // this class takes enemies from EnemyQueue to fight

    public void spawnEnemy() {
        currentEnemyPrefab = enemiesQueue.removeEnemy();
        UnityEngine.SceneManagement.SceneManager.LoadScene(Player.FIGHTING_LEVEL);
    }

    public GameObject pickEnemy() {
        int randomEnemyNum = Mathf.FloorToInt(Random.value * (enemies.Length));
        Enemy possibleEnemy = enemies[randomEnemyNum].GetComponent<Enemy>();
        int spawnRateComparer = Random.Range(0, 100);
        if (spawnRateComparer < possibleEnemy.spawnRate) {
            return enemies[randomEnemyNum];
        } else {
            return pickEnemy();
        }
    }

    private GameObject pickBoss() {
        return enemies[0];
    }
}
