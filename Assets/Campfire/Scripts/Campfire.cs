using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Campfire : MonoBehaviour {

    public GameObject fadePopUp;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void goToSleep() {
        Player.giveHealth(Player.getMaxHealth());
        // WATCH AD HERE!
        toForest();
    }

    public void toForest() {  
        GameState.loadScene(GameState.scene.WALKING_LEVEL);
    }

        
        /*
        int goldReward = 0;
        int xpReward = 0;
        float timeToComplete = -1;
        float distance = -1;
        float difficulty = 1;
        quest thisQuest = new quest("Test Quest", "You're exploring in the forest", "Find new enemies and get experience and gold!", goldReward, xpReward, null, timeToComplete, distance, difficulty);
        thisQuest.active = true;
        Questing.startQuest(thisQuest);
        */
    

    public void openSleepPopUp() {
        //restPopUp.SetActive(true);
        PopUp.instance.showPopUp("WOULD YOU LIKE TO REST FOR THE NIGHT?",
            new string[] { "YES", "NO" },
            new Action[] { goToSleep, () => { } });
    }

    IEnumerator fadeRestScreen() {
        fadePopUp.SetActive(true);
        yield return FadingUtils.fadeImage(fadePopUp.GetComponent<Image>(), 0.5f, 0, 1);
        yield return new WaitForSeconds(1);
        yield return FadingUtils.fadeImage(fadePopUp.GetComponent<Image>(), 0.5f, 1, 0);
        fadePopUp.SetActive(false);
    }
}
