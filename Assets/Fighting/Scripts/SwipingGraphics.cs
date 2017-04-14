using UnityEngine;
using System.Collections;

public class SwipingGraphics : MonoBehaviour {

    public float secondsPerLineUpdate = 0.005f;
    public float maxLineDurationInSeconds = 0.5f;
    private LineRenderer lineRenderer;
    private int numOfVerticesOnLine;
    private bool swiping = false;
    int swipingId;

    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update() {

#if UNITY_WEBGL || DEBUG

        // If using editor, you want to use a mouse
        if (Input.GetMouseButtonUp(0) && swiping) {
            endSwiping();
        } else if (Input.GetMouseButtonDown(0)) {
            // Mouse button released
            StartCoroutine(invokeRepeatingImmediate(AddPoint, secondsPerLineUpdate));
            swiping = true;
        } 
#else
        // If not using editor, find touches   
        if (Input.touchCount > 0 && !swiping) {
            foreach(Touch t in Input.touches) {
                if(t.phase == TouchPhase.Began) {
                    StartCoroutine(invokeRepeatingImmediate(AddPoint, secondsPerLineUpdate));
                    swipingId = t.fingerId;
                    swiping = true;
                }
            }
        }
#endif
    }

    // Used for drawing attack lines while doing criticals
    public void AddPoint() {
        Vector3 inputPoint = new Vector3();

#if UNITY_WEBGL || DEBUG
        if (!Input.GetMouseButton(0))
            return;
        inputPoint = Input.mousePosition;

#else
        
        if (Input.touchCount == 0) {
            endSwiping();
            return;
        }
            

        bool foundTouch = false;
        foreach(Touch t in Input.touches) {
            if (t.fingerId == swipingId) {
                inputPoint = t.position;
                foundTouch = true;
                break;
            }     
        }

        if(!foundTouch) {
            endSwiping();
            return;
        }
#endif
        // Change input point to 
        inputPoint.z = 10;
        inputPoint = Camera.main.ScreenToWorldPoint(inputPoint);
        lineRenderer.SetVertexCount(++numOfVerticesOnLine);
        lineRenderer.SetPosition(numOfVerticesOnLine-1, inputPoint);

    }

    private void endSwiping() {
        stopInvokingRepeating();
        lineRenderer.SetVertexCount(0);
        numOfVerticesOnLine = 0;
        swiping = false;
    }

    private bool stopInvoking = false;
    void stopInvokingRepeating() {
        stopInvoking = true;
    }

    IEnumerator invokeRepeatingImmediate(System.Action method, float repeatTime) {
        if (stopInvoking)
            stopInvoking = false;
        while (!stopInvoking) {
            method();
            yield return new WaitForSeconds(repeatTime);
        }
    }
}
