using UnityEngine;
using System.Collections;

public class TownButtonController : MonoBehaviour {

    TownWatchdog town;
    BountyBoard bountyBoard;
    Shop shop;
    Tavern tavern;
    public Collider2D bountyBoardCollider, tavernCollider, shopCollider, forestCollider;

	// Use this for initialization
	void Start () {
        town = GetComponent<TownWatchdog>();
        bountyBoard = GetComponent<BountyBoard>();
        shop = GetComponent<Shop>();
        tavern = GetComponent<Tavern>();
	}
	
	// Update is called once per frame
	void Update () {

        CheckInput.checkTapOrMouseDown(buttonHit);
    }

    void buttonHit(Collider2D collider) {

        if (town.menus.activeSelf == true)
            return;

        if (collider == shopCollider) {
            shop.openShop();
        } else if (collider == bountyBoardCollider) {
            bountyBoard.openBounties();
        } else if (collider == tavernCollider) {
            tavern.openTavern();
        } else if (collider == forestCollider) {
            town.showForestPopUp();
        } else {

        }

        return;
    }
}
