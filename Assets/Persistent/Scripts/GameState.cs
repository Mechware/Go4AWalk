using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameState : MonoBehaviour {

    private const string TOWN_LEVEL = "TownScreen";
    private const string FIGHTING_LEVEL = "FightingScene";
    private const string WALKING_LEVEL = "WalkingScreen";
    private const string CAMPSITE = "Campsite";

    public enum scene {
        TOWN_LEVEL,
        FIGHTING_LEVEL,
        WALKING_LEVEL,
        CAMPSITE
    }
    
    public static bool fighting {
        get {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(FIGHTING_LEVEL);
        }
    }
    public static bool walking {
        get {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(WALKING_LEVEL);
        }
    }
    public static bool inTown {
        get {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(TOWN_LEVEL);
        }
    }
    public static bool atCamp {
        get {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(CAMPSITE);
        }
    }

    public GameObject levelFader;
    private static GameState _instance;

    // Use this for stopping
    void Awake() {
        _instance = this;
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(fadeIn());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void fadeOut() {

    }

    private IEnumerator fadeIn() {
        levelFader.SetActive(true);
        yield return FadingUtils.fadeImage(levelFader.GetComponent<Image>(), 1f, 1f, 0f);
        levelFader.SetActive(false);
    }

    public IEnumerator fadeToLoadScene(scene scene) {
        levelFader.SetActive(true);
        yield return FadingUtils.fadeImage(levelFader.GetComponent<Image>(), 1f, 0f, 1f);
        string sceneName = getLevelName(scene);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public static void loadScene(scene scene) {
        _instance.StartCoroutine(_instance.fadeToLoadScene(scene));
    }

    private static string getLevelName(scene scene) {
        if (scene == scene.FIGHTING_LEVEL) {
            return FIGHTING_LEVEL;
        } else if (scene == scene.TOWN_LEVEL) {
            return TOWN_LEVEL;
        } else if (scene == scene.WALKING_LEVEL) {
            return WALKING_LEVEL;
        } else if (scene == scene.CAMPSITE) {
            return CAMPSITE;
        } else {
            throw new System.Exception("Tried to load non-existent scene");
        }
    }
}
