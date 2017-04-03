﻿using UnityEngine;
using System.Collections;

public class ItemContainer : MonoBehaviour {

    public item containingItem;
    public AudioSource sound;
    public AudioClip pickUpPotionSound;
    public AudioClip pickUpItemSound;
    public AudioClip shootItemSound;
    private float xforce, yforce;
    private bool alreadyGotThatOne = false;

    // Use this for initialization
    void Start() {
        sound.clip = shootItemSound;
        sound.Play();

    }

    public void setItem(item it) {
        containingItem = it;
        this.GetComponentInChildren<SpriteRenderer>().sprite = it.icon;

    }

    public void launchItem() {
        xforce = Random.Range(-200f, 200);
        yforce = Random.Range(-400f, 400);
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(xforce, yforce));
    }

    public void launchItemSlow() {
        xforce = Random.Range(-100f, 100f);
        yforce = Random.Range(-100f, 100f);
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(xforce, yforce));
    }

    // Update is called once per frame
    void Update() {
        CheckInput.checkTapOrMouseDown((Collider2D hitCollider) => {
            StartCoroutine(itemSelected(hitCollider));
        });
    }

    IEnumerator itemSelected(Collider2D hitCollider) {

        if (hitCollider != this.GetComponentInChildren<Collider2D>())
            yield break;

        if (containingItem.type == itemType.Potion && alreadyGotThatOne == false) {
            PotionInventory.addPotion(containingItem);
            sound.clip = pickUpPotionSound;
            sound.Play();
            GetComponentInChildren<SpriteRenderer>().sprite = null;
            alreadyGotThatOne = true;
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        } else if (alreadyGotThatOne == false){
            Inventory.items.Add(containingItem);
            sound.clip = pickUpItemSound;
            sound.Play();
            GetComponentInChildren<SpriteRenderer>().sprite = null;
            alreadyGotThatOne = true;
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
        yield break;
    }


    }

