using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements; // Using the Unity Ads namespace.

public class Ads : MonoBehaviour {

    public static string gameId = "1391347"; // Set this value from the inspector.
    public static bool enableTestMode = false;

    public static Ads instance;

    void Awake() {
        instance = this;
    }

    void Start() {
        StartCoroutine(initializeAds());
    }

    public static IEnumerator initializeAds() {

        if (Advertisement.isSupported) { // If runtime platform is supported...
            Advertisement.Initialize(gameId, enableTestMode); // ...initialize.
        }

        // Wait until Unity Ads is initialized,
        //  and the default ad placement is ready.
        while (!Advertisement.isInitialized || !Advertisement.IsReady()) {
            yield return new WaitForSeconds(0.5f);
        }
    }

    public static void showAd() {
        // Show the default ad placement.
        Advertisement.Show();
    }
}