using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tavern : MonoBehaviour {
    TownWatchdog townWatchdog;
    public GameObject restPopUp;
    public GameObject errorPopUp;
    public GameObject fadePopUp;
	// Use this for initialization
	void Start () {
        townWatchdog = GetComponent<TownWatchdog>();
	}

    public void rest() {
        if (Player.gold.Value >= 50)
        {
            Player.giveHealth(Player.maxHealth);
            Player.takeGold(50);
            fadePopUp.SetActive(true);
            StartCoroutine(fadeRestScreen());
                    
        }
        else
        {
            errorPopUp.SetActive(true);
            print("Not enough gold.");
        }
            
    }

    IEnumerator fadeRestScreen()
    {
        fadePopUp.SetActive(true);
        yield return FadingUtils.fadeImage(fadePopUp.GetComponent<Image>(), 0.5f, 0, 1);
        yield return new WaitForSeconds(1);
        restPopUp.SetActive(false);
        yield return FadingUtils.fadeImage(fadePopUp.GetComponent<Image>(), 0.5f, 1, 0);
        fadePopUp.SetActive(false);
    }

    public void openTavernPopUp()
    {
        townWatchdog.open(townWatchdog.tavern);
    }
	
    public void closeRestPopUp()
    {
        restPopUp.SetActive(false);
    }

    public void openRestPopUp()
    {
        restPopUp.SetActive(true);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
