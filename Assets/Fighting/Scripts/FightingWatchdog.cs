using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FightingWatchdog : MonoBehaviour {

    public float lineUpdateSpeed = 0.005f;
    private LineRenderer lineRenderer;
    private int i;
    private bool swiping = false;

    void Awake() {

    }

    // Use this for initialization
    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update() {

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WebGLPlayer) {
            // If using editor, you want to use a mouse
            if (Input.GetMouseButtonUp(0) && swiping) {
                // Mouse button released
                CancelInvoke("AddPoint");
                lineRenderer.SetVertexCount(0);
                i = 0;
                swiping = false;
            } else if (Input.GetMouseButtonDown(0)) {
                // Mouse button released
                InvokeRepeating("AddPoint", lineUpdateSpeed, lineUpdateSpeed);
                swiping = true;
            } else if (Input.GetMouseButton(0)) {
                // Swiping
            } else if (swiping) {
                print("Still swiping but no released");
            }
        } else {
            // If not using editor, find touches    
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
        }
    }


    // Used for drawing attack lines while doing criticals
    public void AddPoint() {
        Vector3 inputPoint;

        
        if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WebGLPlayer) {
            // If using computer, make click attack
            if (!Input.GetMouseButton(0))
                return;
            inputPoint = Input.mousePosition;
        } else {
            // If using phone use touch
            if (Input.touchCount == 0)
                return;
            inputPoint = Input.touches[0].position;
        }

        // Change input point to 
        inputPoint.z = 10;
        inputPoint = Camera.main.ScreenToWorldPoint(inputPoint);
        lineRenderer.SetVertexCount(++i);
        lineRenderer.SetPosition(i-1, inputPoint);

        // Check if swipe has taken too long
        if (i*lineUpdateSpeed > 0.5) {
            i = 0;
            lineRenderer.SetVertexCount(0);
            CancelInvoke("AddPoint");
        }
    }

    public Graphic background, treasureBackground, enemyKilled;
    public GameObject treasureChest, critBarObject, healthBarObject;

    // Called when the enemy is killed
    public void endRegularFight() {
        foreach (Graphic g in critBarObject.GetComponentsInChildren<Graphic>()) {
            g.CrossFadeAlpha(0, 1, false);
        }
        foreach (Graphic g in healthBarObject.GetComponentsInChildren<Graphic>()) {
            g.CrossFadeAlpha(0, 1, false);
        }
        enemyKilled.gameObject.SetActive(true);
        enemyKilled.CrossFadeAlpha(255, 1, false);
    }

    // Called when a boss is killed
    public IEnumerator questFightEnd() {
        foreach(Graphic g in critBarObject.GetComponentsInChildren<Graphic>()) {
            g.CrossFadeAlpha(0, 1, false);
        }
        foreach (Graphic g in healthBarObject.GetComponentsInChildren<Graphic>()) {
            g.CrossFadeAlpha(0, 1, false);
        }

        background.CrossFadeAlpha(0, 1, false);
        treasureBackground.CrossFadeAlpha(255, 1, false);

        yield return new WaitForSeconds(1.1f);

        treasureChest.SetActive(true);
        StartCoroutine(treasureChest.GetComponent<TreasureChest>().openChest());

        Destroy(treasureChest, 5.1f);

        yield return new WaitForSeconds(5);

        Questing.endQuest(true);
    }
}
