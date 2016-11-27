using UnityEngine;
using System.Collections;

public class Attacking : MonoBehaviour {

    private Vector2 startPosition, swipe;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WebGLPlayer) {
            if (Input.GetMouseButtonUp(0)) {

                checkAttack(startPosition, Input.mousePosition);

            } else if (Input.GetMouseButtonDown(0)) {
                Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                startPosition = mousePos;
            }
        } else {

            if (Input.touchCount > 0) {
                Touch t = Input.GetTouch(0);

                if (t.phase == TouchPhase.Ended) {

                    checkAttack(startPosition, t.position);

                } else if (t.phase == TouchPhase.Began) {
                    startPosition = t.position;
                }
            }
        }
    }

    void checkAttack(Vector2 startPos, Vector2 endPos) {

        // First check swipe
        swipe = endPos - startPos;

        if (swipe.magnitude > Screen.width/4) {

            // Hit enemy with damage dictated by Player class
            if (EnemyWatchdog.currentEnemy != null && EnemyWatchdog.currentEnemy.GetComponent<Collider2D>() != null) {
                //EnemyWatchdog.currentEnemy.GetComponent<Enemy>().hit(Player.getCriticalAttack(swipe), true);
                EnemyWatchdog.currentEnemy.GetComponent<Enemy>().hit(Player.getCriticalAttack(swipe), true);
                Player.resetCrit();
            }

            return;
        }
        

        // Then check if tap
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((endPos)), Vector2.zero);

        // IF statement assumes enemy is only collider on screen
        if (hit.collider != null) {
            // Hit enemy with power dictated by Player class
            if(EnemyWatchdog.currentEnemy != null)
                EnemyWatchdog.currentEnemy.GetComponent<Enemy>().hit(Player.getRegularAttack(), false);
        }
    }
}
