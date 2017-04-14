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
        }
    }

    void spawnEnemyFromQueue() {
        GameObject currentEnemy;

        if (isBoss) {
            currentEnemy = bosses[StoryOverlord.currentLevel];
        } else {
            currentEnemy = enemies[enemiesQueue.Dequeue()];
        }

        if (currentEnemy == null) {
            print("No enemy created, spawning gnome chompy.");
            currentEnemy = enemies[0];
        }

        this.currentEnemy = Instantiate(currentEnemy);
        saveQueue();
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update() {

        if (GameState.walking) {

            if (Questing.currentQuest.distance != -1 && Questing.currentQuest.distance <= Questing.currentQuest.distanceProgress) {
                nextEncounterDistance = float.MaxValue;
            } else if (Player.totalDistance.Value > nextEncounterDistance) {
                if(enemiesQueue.Count <= 9) {
                    enemiesQueue.Enqueue(pickEnemy());
                }
                nextEncounterDistance += maxRandomEncounterDistance * Random.Range(0.1f, 1f);
                saveQueue();
            }
        }
    }

    // Called to end an encounter
    public IEnumerator enemyHasDied(bool isBoss) {
        if (isBoss == true) {
            currentEnemy = null;
            lastEncounterDistance = Player.totalDistance.Value;
            yield return fw.questFightEnd();
        } else if (enemiesQueue.Count == 0) {
            currentEnemy = null;
            lastEncounterDistance = Player.totalDistance.Value;
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
        saveQueue();
        DialoguePopUp.instance.showDialog(
            dialog: StoryOverlord.bossFightDialogue,
            characterName: StoryOverlord.bossfightName, 
            characterSprite: StoryOverlord.bossfightSprite, 
            functionToCallWhenDialogFinished: () => {
            GameState.loadScene(GameState.scene.FIGHTING_LEVEL);
        });
        
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

    const string SAVE_QUEUE_STRING = "Monster_Queue";

    public void saveQueue() {
        int i = 0;
        while(PlayerPrefs.HasKey(SAVE_QUEUE_STRING+i)) {
            PlayerPrefs.DeleteKey(SAVE_QUEUE_STRING+i);
            i++;
        }
        
        int[] monsters = enemiesQueue.ToArray();
        for(i = 0 ; i < monsters.Length ; i++) {
            PlayerPrefs.SetInt(SAVE_QUEUE_STRING + i, monsters[i]);
        }
    }

    public void loadQueue() {
        int i = 0;
        int monster;
        enemiesQueue = new Queue<int>();
        while(PlayerPrefs.HasKey(SAVE_QUEUE_STRING+i)) {
            monster = PlayerPrefs.GetInt(SAVE_QUEUE_STRING + i);
            enemiesQueue.Enqueue(monster);
            i++;
        }
    }
}
