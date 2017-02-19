using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attacking : MonoBehaviour {

    private Vector2 startPosition, swipe;
    public GameObject buff;
    public int swipingID;
    public IDictionary<int, Vector2> touchesToSwipes = new Dictionary<int,Vector2>();

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

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
    }

    void checkAttack(Vector2 startPos, Vector2 endPos) {

        // First check swipe
        swipe = endPos - startPos;

        if (swipe.magnitude > Screen.width/4) {

            // Hit enemy with damage dictated by Player class
            if (EnemyWatchdog.currentEnemy != null && EnemyWatchdog.currentEnemy.GetComponentInChildren<Collider2D>() != null) {
                EnemyWatchdog.currentEnemy.GetComponent<Enemy>().hit(Player.getCriticalAttack(swipe), true);
                Player.resetCrit();
                CreateDOT("EnemyDOTFire", "fire", 5, 5, 1, EnemyWatchdog.currentEnemy.gameObject);
            }

            return;
        }
        

        // Then check if tap
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((endPos)), Vector2.zero);

        // IF statement assumes enemy is only collider on screen
        if (hit.collider != null) {
            // Hit enemy with power dictated by Player class
            if(EnemyWatchdog.currentEnemy != null) {
                EnemyWatchdog.currentEnemy.GetComponent<Enemy>().hit(Player.getRegularAttack(), false);
                CreateDOT("EnemyDOTFire","fire", 10, 5, 1, EnemyWatchdog.currentEnemy.gameObject);
            }
                
        }
    }

    void CreateBuff(string tag, string type, float modifier, int duration, GameObject target) {
        Destroy(GameObject.FindGameObjectWithTag(tag));
        print(tag + " buff Destroyed");
        GameObject thisIsABuff = Instantiate(buff) as GameObject;
        thisIsABuff.GetComponent<Buff>().setBuff(name, type, modifier, duration, target);        
    }

    void CreateDOT(string tag, string type, float modifier, int duration, int frequency, GameObject target) {
        Destroy(GameObject.FindGameObjectWithTag(tag));
        print(tag + " buff Destroyed");
        GameObject thisIsABuff = Instantiate(buff) as GameObject;
        thisIsABuff.GetComponent<Buff>().setBuff(name, type, modifier, frequency, duration, target);
    }
}
