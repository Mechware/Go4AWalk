using UnityEngine;
using System.Collections;

public class WalkingMode : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void sleepMode() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
