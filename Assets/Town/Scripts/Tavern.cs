using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Tavern : MonoBehaviour {
    TownWatchdog townWatchdog;
    public GameObject fadePopUp;
    // Use this for initialization
    void Start() {
        townWatchdog = GetComponent<TownWatchdog>();
    }

    public void rest() {
        if (Player.gold.Value >= 50) {
            Player.giveHealth(Player.getMaxHealth());
            Player.takeGold(50);
            StartCoroutine(fadeRestScreen());
        } else {
            PopUp.instance.showPopUp(
                "Not enough gold.", 
                new string[] { "Ok" }, 
                new Action[] { () => { } });
        }

    }

    IEnumerator fadeRestScreen() {
        fadePopUp.SetActive(true);
        yield return FadingUtils.fadeImage(fadePopUp.GetComponent<Image>(), 0.5f, 0, 1);
        yield return new WaitForSeconds(1);
        yield return FadingUtils.fadeImage(fadePopUp.GetComponent<Image>(), 0.5f, 1, 0);
        fadePopUp.SetActive(false);
    }

    public void openTavern() {
        townWatchdog.open(townWatchdog.tavern);
    }

    public void openRestPopUp() {
        //restPopUp.SetActive(true);
        PopUp.instance.showPopUp("Would you like to rest for the night?\n\nPrice: 50 gold",
            new string[] { "Yes", "No" },
            new Action[] { rest, () => { } });
    }
    // Update is called once per frame
    void Update() {

    }
}
