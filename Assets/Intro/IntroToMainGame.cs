using UnityEngine;
using System.Collections;

public class IntroToMainGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(switchScenes());
	}
	
	IEnumerator switchScenes() {
        yield return new WaitForSeconds(2);
        UnityEngine.SceneManagement.SceneManager.LoadScene("WalkingScreen");
    }
}
