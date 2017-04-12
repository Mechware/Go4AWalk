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
    public GameObject[] bosses;
    public GameObject randomEncounterButton;
    public FightingWatchdog fw;
    public GameObject currentEnemy;
    public Text enemiesLeft;

    public static EnemyWatchdog instance;
    public static Queue<int> enemiesQueue;
    


    // Distance will be anywhere from this distance to 10 times this distance
    public static float maxRandomEncounterDistance = 100f;
    public static bool isBoss;

    private static float lastEncounterDistance;
    private static float nextEncounterDistance;

    void Awake() {
        if (enemiesQueue == null) {
            enemiesQueue = new Queue<int>();
        }
        instance = this;

        if (GameState.fighting) {
            spawnEnemyFromQueue();
            enemiesLeft.text = "" + (enemiesQueue.Count + 1);
        }
    }

    // Use this for initialization
    void Start() {
        if (GameState.walking) {
            // Initialize encounter variables
            lastEncounterDistance = Player.totalDistance.Value;
            nextEncounterDistance = lastEncounterDistance + Random.Range(50f, 150f);
            WalkingWatchdog.instance.setQueueSize(enemiesQueue.Count);
        }
    }

    void spawnEnemyFromQueue() {
        GameObject currentEnemy;

        if (isBoss) {
            currentEnemy = bosses[StoryOverlord.currentLevel];
        } else {
            currentEnemy = enemies[enemiesQueue.Dequeue()];
            print("Removing " + currentEnemy.name + " from the queue and spawning them");
        }

        if (currentEnemy == null) {
            print("No enemy created, spawning gnome chompy.");
            currentEnemy = enemies[0];
        }

        this.currentEnemy = Instantiate(currentEnemy);
    }

    // Update is called once per frame
    void Update() {

        if (GameState.walking) {

            if (Questing.currentQuest.distance != -1 && Questing.currentQuest.distance <= Questing.currentQuest.distanceProgress) {
                nextEncounterDistance = float.MaxValue;
            } else if (Player.totalDistance.Value > nextEncounterDistance) {
                enemiesQueue.Enqueue(pickEnemy());
                WalkingWatchdog.instance.setQueueSize(enemiesQueue.Count);
                nextEncounterDistance += maxRandomEncounterDistance * Random.Range(0.1f, 1f);
            }
        }
    }

    // Called to end an encounter
    public IEnumerator enemyHasDied(bool isBoss) {
        if (isBoss == true) {
            endFight();
            yield return fw.questFightEnd();
        } else if (enemiesQueue.Count == 0) {
            endFight();
            StartCoroutine(fw.endRegularFight());
        } else {
            fw.fadeOutStats();
            enemiesLeft.text = "" + enemiesQueue.Count;
            yield return new WaitForSeconds(2);

            GameState.loadScene(GameState.scene.FIGHTING_LEVEL);
        }

    }

    public void startFighting() {
        if (enemiesQueue.Count == 0) {
            return;
        } else {
            isBoss = false;
            GameState.loadScene(GameState.scene.FIGHTING_LEVEL);
        }
    }

    public void startBossFight() {
        isBoss = true;
        enemiesQueue.Clear();
        DialoguePopUp.instance.showDialog(
            dialog: StoryOverlord.bossFightDialogue,
            characterName: StoryOverlord.bossfightName, 
            characterSprite: StoryOverlord.bossfightSprite, 
            functionToCallWhenDialogFinished: () => {
            GameState.loadScene(GameState.scene.FIGHTING_LEVEL);
        });
        
    }


    private void endFight() {
        currentEnemy = null;
        lastEncounterDistance = Player.totalDistance.Value;
    }

    public int pickEnemy() {
        int randomEnemyNum = Random.Range(StoryOverlord.firstEnemy, StoryOverlord.lastEnemy+1);
        Enemy possibleEnemy = enemies[randomEnemyNum].GetComponent<Enemy>();
        int spawnRateComparer = Random.Range(0, 100);
        if (spawnRateComparer < possibleEnemy.spawnRate) {
            return randomEnemyNum;
        } else {
            return pickEnemy();
        }
    }

    private GameObject pickBoss() {
        return bosses[StoryOverlord.currentLevel];
    }

}
