using UnityEngine;
using System.Collections;

public class IntroToMainGame : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(2);
        if (PlayerPrefs.HasKey("Done Intro")) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("WalkingScreen");
        } else {
            PlayerPrefs.SetString("Done Intro", bool.TrueString);
            PlayerPrefs.Save();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Volcano");
        }
    }
}
