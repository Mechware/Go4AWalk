﻿using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements; // Using the Unity Ads namespace.

public class Ads : MonoBehaviour {

#if !UNITY_ADS // If the Ads service is not enabled...
    public static string gameId; // Set this value from the inspector.
    public static bool enableTestMode = true;
#endif

    public static Ads instance;

    void Awake() {
        instance = this;
    }

    void Start() {



        StartCoroutine(initializeAds());
    }

    public static IEnumerator initializeAds() {

#if !UNITY_ADS // If the Ads service is not enabled...
        if (Advertisement.isSupported) { // If runtime platform is supported...
            Advertisement.Initialize(gameId, enableTestMode); // ...initialize.
        }
#endif

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