using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attacking : MonoBehaviour {

    private Vector2 startPosition, swipe;
    public GameObject buff;
    public int swipingID;
    public IDictionary<int, Vector2> touchesToSwipes = new Dictionary<int,Vector2>();
    public string dotName = "EnemyDOTFire";

    public GameObject enemyObject;


    // Use this for initialization
    void Start () {
        enemyObject = EnemyWatchdog.instance.currentEnemy;
	}
	
	// Update is called once per frame
	void Update () {

        if(GameState.paused) {
            return;
        }


#if UNITY_EDITOR || UNITY_WEBGL

   
        if (Input.GetMouseButtonUp(0)) {
            checkAttack(startPosition, Input.mousePosition);

        } else if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            startPosition = mousePos;
        }
#else
        if (Input.touchCount > 0) {
            foreach(Touch t in Input.touches) {
                if (t.phase == TouchPhase.Ended) {
                    checkAttack(touchesToSwipes[t.fingerId], t.position);
                    touchesToSwipes.Remove(t.fingerId);
                } else if (t.phase == TouchPhase.Began) {
                    touchesToSwipes.Add(t.fingerId, t.position);
                }
            }
        }
#endif

        if (enemyObject.GetComponent<Enemy>().isDead && enemyObject.GetComponent<Enemy>() != null) {
            BuffManager.removeCurrentBuff(dotName);
        }

    }

    void checkAttack(Vector2 startPos, Vector2 endPos) {

        // First check swipe
        swipe = endPos - startPos;

        if (swipe.magnitude > Screen.width/4) {

            // Hit enemy with damage dictated by Player class
            if (enemyObject != null && enemyObject.GetComponentInChildren<Collider2D>() != null) {
                enemyObject.GetComponent<Enemy>().hit(Player.getCriticalAttack(swipe), true);
                Player.resetCrit();
            }

            return;
        }
        

        // Then check if tap
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((endPos)), Vector2.zero);

        // IF statement assumes enemy is only collider on screen
        if (hit.collider != null) {
            // Hit enemy with power dictated by Player class
            if(enemyObject != null) {
                enemyObject.GetComponent<Enemy>().hit(Player.getRegularAttack(), false);
                // BuffManager.instance.CreateDOT("EnemyDOTFire", BuffManager.BuffType.fire, 10, 5, 1, enemyObject);
                if (Player.isDOT) {
                    attackWithDOT(dotName, BuffManager.BuffType.fire, Player.dotHit, 5, 1, enemyObject);
                }
            }

        }

    }

    void attackWithDOT(string name, BuffManager.BuffType statType, int modifier, int duration, int frequency,GameObject target) {

        if(!BuffManager.buffs.ContainsKey(name) && !enemyObject.GetComponent<Enemy>().isDead) {
            BuffManager.instance.CreateDOT(name, statType, modifier, duration, frequency, target);
        }

    }
}
