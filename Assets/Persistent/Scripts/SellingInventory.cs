using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SellingInventory : MonoBehaviour {

    public static bool currentlySellingItems;
    public GameObject itemsPanel;
    public GameObject sellButton;
    public static SellingInventory instance;
    public static item itemToSell;
    public AudioSource audio;

    void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnSellItemsToggle() {        
        currentlySellingItems = !currentlySellingItems;
        if (currentlySellingItems) {
            StartCoroutine(checkIfSellingShouldStop());
        }
    }

    IEnumerator checkIfSellingShouldStop() {
        while(instance.itemsPanel.activeInHierarchy && currentlySellingItems) {
            yield return 0;
        }
        currentlySellingItems = false;
        instance.sellButton.GetComponent<Toggle>().isOn = false;
    }

    public static void sell(item item) {

        /*
        if (Player.equippedWeapon.Equals(itemToSell) 
            || Player.equippedAccessory.Equals(itemToSell) 
            || Player.equippedArmor.Equals(itemToSell)) {
            PopUp.instance.showPopUp("Cannot sell currently equipped item", new string[] { "Okay" });
            return;
        }*/

        itemToSell = item;
        //~~~~~~ If we can get unique identifiers on objects, and we want to remove making equipped items dissapear 
        /*if (itemToSell.name == Player.equippedAccessory.name || itemToSell.name == Player.equippedWeapon.name ||itemToSell.name == Player.equippedArmor.name) {
            PopUp.instance.showPopUp("Can't sell equipped items. Please unequip and try again later",
                new string[] { "Okay"},
                new Action[] {
                new Action(() => { })

                                }
            );
        } else {*/

            PopUp.instance.showPopUp("Are you sure you want to sell this item for " + item.price + " gold?",
                new string[] { "No", "Yes" },
                new Action[] {
                new Action(() => { }),
                new Action(confirmSell)          
                }
            );

      //  }

    }

    public static void confirmSell() {

        Player.giveGold(itemToSell.price);
        Inventory.removeItem(itemToSell);
        instance.audio.Play();
    }
}
