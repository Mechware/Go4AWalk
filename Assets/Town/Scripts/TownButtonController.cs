﻿using UnityEngine;
using System.Collections;

public class TownButtonController : MonoBehaviour {

    TownWatchdog town;
    BountyBoard bountyBoard;
    Shop shop;
    public Collider2D bountyBoardCollider, tavernCollider, shopCollider, forestCollider;

	// Use this for initialization
	void Start () {
        town = GetComponent<TownWatchdog>();
        bountyBoard = GetComponent<BountyBoard>();
        shop = GetComponent<Shop>();
	}
	
	// Update is called once per frame
	void Update () {

#if DEBUG || UNITY_WEBGL
        if(Input.GetMouseButtonDown(0)) {
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            // RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
            if (hitInfo) {
                buttonHit(hitInfo.collider);
            }
        }
#else
        for (var i = 0 ; i < Input.touchCount ; ++i) {
            if (Input.GetTouch(i).phase == TouchPhase.Began) {
                RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);
                // RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
                if (hitInfo) {
                    buttonHit(hitInfo.collider);
                }
            }
        }
#endif
    }

    void buttonHit(Collider2D collider) {

        if (shop.tw.menus.activeSelf == true)
            return;

        if (collider == shopCollider) {
            shop.openShop();
        } else if (collider == bountyBoardCollider) {
            bountyBoard.openBounties();
        } else if (collider == tavernCollider) {
            
        } else if (collider == forestCollider) {
            town.showForestPopUp();
        } else {

        }
    }
}