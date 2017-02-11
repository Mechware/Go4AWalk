using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyQueue {

    public int maxQueueLength = 9;
    private static Queue<GameObject> enemiesQueue;
    public static GameObject currentEnemy;

    public EnemyQueue(EnemyWatchdog enemyWatchdog)
    {
        enemiesQueue = new Queue<GameObject>();
    }

    public void putEnemy()
    {
        currentEnemy = EnemyWatchdog.instance.pickEnemy();
        if (enemiesQueue.Count <= maxQueueLength)
        {
            enemiesQueue.Enqueue(currentEnemy);
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("The queue is full");
        }
    }
   
    /*
    private void fightEnemy()
    {
         
        if (state != encounterState.None)
        {
            return;
        }

        else if (enemiesQueue.Count != 0)
        {
            currentEnemy = enemiesQueue.Dequeue();
            state = encounterState.Fight;
            UnityEngine.SceneManagement.SceneManager.LoadScene(Player.FIGHTING_LEVEL);
        }
        else
        {
            print("The Queue is empty");
        }
    }
    */

    public GameObject removeEnemy()
    {      
        if(IsEmpty())
        {
            return EnemyWatchdog.instance.pickEnemy();
        }
        else return enemiesQueue.Dequeue();
    }

    public bool IsEmpty()
    {
        if (enemiesQueue.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int getSize()
    {
        return enemiesQueue.Count;
    }

    public void emptyQueue() {
        enemiesQueue.Clear();
    }
}
