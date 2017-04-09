using UnityEngine;
using System.Collections;

public class CreditsButton : MonoBehaviour {
    public GameObject creditsPanel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void showCredits() {
        creditsPanel.SetActive(true);
    }
    public void hideCredits() {
        creditsPanel.SetActive(false);
    }
}
