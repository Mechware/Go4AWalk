using UnityEngine;
using System.Collections;

public class Attacking : MonoBehaviour {

    private Transform enemy;
    private Vector2 startPosition, swipe;

    // Use this for initialization
    void Start () {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene() != UnityEngine.SceneManagement.SceneManager.GetSceneByName("FightingScene")) {
            Destroy(this);
        }
	}
	
	// Update is called once per frame
	void Update () {

#if !UNITY_EDITOR
        if (Input.touchCount > 0) {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Ended) {

                // Check swipe
                swipe = t.position - startPosition;

                if(swipe.magnitude > Screen.width/4) {

                    // TODO: Check if over player

                    // Hit enemy with damage dictated by Player class
                    EnemyWatchdog.currentEnemy.GetComponent<Enemy>().hit(Player.getSwipeAttack(swipe), true);

                    // TODO: Show animation
                    

                    return;
                }


                // Check if tap hit anything
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((t.position)), Vector2.zero);

                // IF statement assumes enemy is only collider on screen
                if (hit.collider != null) { 
                    // Hit enemy with power dictated by Player class
                    EnemyWatchdog.currentEnemy.GetComponent<Enemy>().hit(Player.getRegularAttack(), false);
                }

            } else if (t.phase == TouchPhase.Began) {
                startPosition = t.position;
            }

        }
#else

        if (Input.GetMouseButtonUp(0)) {

            // Check swipe
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            swipe = mousePos - startPosition;

            if (swipe.magnitude > Screen.width/4) {

                // TODO: Check if over player

                // Hit enemy with damage dictated by Player class
                EnemyWatchdog.currentEnemy.GetComponent<Enemy>().hit(Player.getSwipeAttack(swipe), true);

                // TODO: Show animation


                return;
            }


            // Check if tap hit anything
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((mousePos)), Vector2.zero);

            // IF statement assumes enemy is only collider on screen
            if (hit.collider != null) {
                // Hit enemy with power dictated by Player class
                EnemyWatchdog.currentEnemy.GetComponent<Enemy>().hit(Player.getRegularAttack(), false);
            }

        } else if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            startPosition = mousePos;
        }

#endif
    }
}
