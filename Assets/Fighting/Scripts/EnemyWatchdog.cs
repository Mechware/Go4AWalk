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
    public FightingWatchdog fw;
    public static EnemyWatchdog instance;
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
        instance = this;
        
    }

    // Use this for initialization
    void Start() {
        if (GameState.walking) {
            // Initialize encounter variables
            lastEncounterDistance = Player.totalDistance.Value;
            nextEncounterDistance = lastEncounterDistance + Random.Range(1f, 100f);
            WalkingWatchdog.instance.setQueueSize(enemiesQueue.getSize());
        } else if (GameState.fighting) {
            if (currentEnemyPrefab != null) {
                // Spawn enemy decided in the walking screen
                currentEnemy = Instantiate(currentEnemyPrefab);
            } else {
                currentEnemy = Instantiate(enemies[0]);
            }
            enemiesLeft.text = "" + (enemiesQueue.getSize() + 1);
        }
    }


    // Update is called once per frame
    void Update() {

        if (GameState.walking) {

            if (Questing.currentQuest.distance != -1 && Questing.currentQuest.distance <= Questing.currentQuest.distanceProgress) {
                nextEncounterDistance = float.MaxValue;
            } else if (Player.totalDistance.Value > nextEncounterDistance) {
                enemiesQueue.putEnemy();
                WalkingWatchdog.instance.setQueueSize(enemiesQueue.getSize());
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
            GameState.loadScene(GameState.scene.WALKING_LEVEL);
        } else {
            enemiesLeft.text = "" + enemiesQueue.getSize();
            yield return new WaitForSeconds(2);

            currentEnemyPrefab = enemiesQueue.removeEnemy();
            GameState.loadScene(GameState.scene.FIGHTING_LEVEL);
        }
        
    }

    public void startFighting() {
        if (enemiesQueue.IsEmpty()) {
            print("There are no enemies to fight ya dummy!");
            return;
        } else {
            enemiesQueue.removeEnemy();
            GameState.loadScene(GameState.scene.FIGHTING_LEVEL);
        }
    }

    public void clearEnemies() {
        enemiesQueue.emptyQueue();
    }

    public bool isEmpty() {
        return enemiesQueue.IsEmpty();
    }


    private void endFight() {
        currentEnemy = null;
        lastEncounterDistance = Player.totalDistance.Value;
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
