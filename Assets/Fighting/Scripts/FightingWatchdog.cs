using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FightingWatchdog : MonoBehaviour {

    public Text critBarText, healthBarText, enemyHealthBar;
    public StatusBar healthBar, critBar;
    public float lineUpdateSpeed = 0.005f;
    private LineRenderer lineRenderer;
    private int i;
    private bool swiping = false;
    


    void Awake() {
        
    }

    // Use this for initialization
    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        
        updateHealthBar();
        updateCritBar();

       #if UNITY_EDITOR

        if (Input.GetMouseButtonUp(0)) {
            // Mouse button released

        } else if (Input.GetMouseButtonDown(0)) {
            // Mouse button released
            InvokeRepeating("AddPoint", lineUpdateSpeed, lineUpdateSpeed);
            swiping = true;
        } else if (Input.GetMouseButton(0)) {
            // Swiping
        } else if (swiping) {
            CancelInvoke("AddPoint");
            lineRenderer.SetVertexCount(0);
            i = 0;
            swiping = false;
        }

       #else
            
        if (Input.touchCount > 0) {
            if (Input.touches[0].phase == TouchPhase.Began) {
                InvokeRepeating("AddPoint", lineUpdateSpeed, lineUpdateSpeed);
                swiping = true;
            }
        } else if (swiping) {
            CancelInvoke("AddPoint");
            lineRenderer.SetVertexCount(0);
            i = 0;
            swiping = false;
        }

       #endif
    }

    void updateHealthBar() {
        healthBar.updateBar(Player.maxHealth, Player.health);
    }

    void updateCritBar() {
        critBar.updateBar(100, Player.crit);
    }


    // Used for drawing attack lines while doing criticals
    public void AddPoint() {
        Vector3 tempPos;

        #if UNITY_EDITOR

        if (!Input.GetMouseButton(0))
            return;
        tempPos = Input.mousePosition;

        #else

        print("Unity editor");
        if (Input.touchCount == 0)
            return;
        tempPos = Input.touches[0].position;

        #endif

        tempPos.z = 10;
        tempPos = Camera.main.ScreenToWorldPoint(tempPos);
        lineRenderer.SetVertexCount(++i);
        lineRenderer.SetPosition(i-1, tempPos);

        // Check if swipe has taken too long
        if (i*lineUpdateSpeed > 0.5) {
            i = 0;
            lineRenderer.SetVertexCount(0);
            CancelInvoke("AddPoint");
        }
    }
}
