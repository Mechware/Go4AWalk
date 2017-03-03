﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public GameObject damageIndicator, critIndicator, poisonIndicator, expIndicator, goldIndicator, item;
    public int health, damage, goldToGive, expToGive;
    public float timeBetweenAttacks;
    public int spawnRate;

    private StatusBar healthBar;
    private GameObject damageIndicatorParent;
    private int maxHealth;
    public static float attackModifierEnemy = 1;
    public static float defenseModifierEnemy = 1;


    // Use this for initialization
    void Start() {
        //give the player gold equal the base amount plus/minus 25%
        goldToGive = goldToGive + Mathf.RoundToInt((goldToGive * (Random.Range(-0.25f, 0.25f))));
        //gives the player exp equal to base amound plus/minus 10%
        expToGive = expToGive + Mathf.RoundToInt((expToGive * (Random.Range(-0.1f, 0.1f))));
        damageIndicatorParent = GameObject.Find("DamageIndicators");
        healthBar = GetComponentInChildren<StatusBar>();
        maxHealth = health;
        StartCoroutine(attack());

    }

    // Update is called once per frame
    void Update() {

    }
    
    public void poison(int damage) { 
            GameObject poison;
            poison = (GameObject)Instantiate(poisonIndicator, damageIndicatorParent.transform, false);
            poison.GetComponent<DamageIndicator>().setText(damage.ToString());       
    }

    public void hit(int damage, bool isCrit) {
        if (health == 0) {
            return;
        }
        GameObject go;        
        if (isCrit) {
            go = (GameObject)Instantiate(critIndicator, damageIndicatorParent.transform, false);
        } else {
            go = (GameObject)Instantiate(damageIndicator, damageIndicatorParent.transform, false);
        }
        if (damage == 0) {
            go.GetComponent<DamageIndicator>().setText("MISS");
        } else {
            go.GetComponent<DamageIndicator>().setText(damage.ToString());
        }

        GetComponentInChildren<Animator>().SetTrigger("flinch");
        health -= damage;

        if (health <= 0) {
            health = 0;
            die();
        }

        healthBar.updateBar(maxHealth, health);
    }

    // Attack player a set number of times
    IEnumerator attack() {
        while (true) {
            yield return new WaitForSeconds(timeBetweenAttacks);
            if (GetComponentInChildren<Animator>().GetBool("dying"))
                break;
            GetComponentInChildren<Animator>().SetTrigger("attacking");
            Player.damage(damage);
        }
    }

    void die() {
        GetComponentInChildren<Animator>().SetBool("dying", true);
        Destroy(healthBar.transform.parent.gameObject);
   
        Player.giveLootGold(goldToGive);
        Player.giveExperience(expToGive);
        Destroy(GetComponent<Collider2D>());
        Destroy(gameObject, 15);
        spawnItems();
        GameObject exp = (GameObject)Instantiate(expIndicator, damageIndicatorParent.transform, false);
        exp.GetComponent<DamageIndicator>().setText("+" + expToGive.ToString() + " XP");
        GameObject gold = (GameObject)Instantiate(goldIndicator, damageIndicatorParent.transform, false);
        gold.GetComponent<DamageIndicator>().setText("+" + goldToGive.ToString() + " GOLD");
        StartCoroutine(EnemyWatchdog.instance.enemyHasDied(EnemyWatchdog.isBoss));

    }

    /* Checks for spawning items, no longer takes an input       
       Rolls a random number and compares it to the spawn rate of the item, if its smaller than the spawn rate it spawns it.
       If the randomnumber*2 is still smaller than the spawn rate it spawns 2 of the item. Spawn rates of different items are independent.
       Spawn rates for 1 and 2 items:
       Health pot: 60%, 30%
       Crit pot:   40%, 20%
       Attack pot: 30%, 15%
    */
    void spawnItems() {

        GameObject items = new GameObject();

        item healthPotion = ItemList.itemMasterList[ItemList.HEALTH_POTION];
        item critPotion = ItemList.itemMasterList[ItemList.CRIT_POTION];
        item attackPotion = ItemList.itemMasterList[ItemList.ATTACK_POTION];

        int luckyNumberH = Random.Range(1, 100);
        int luckyNumberC = Random.Range(1, 100);
        int luckyNumberA = Random.Range(1, 100);

        if (healthPotion.spawnRate >= luckyNumberH) {
            items = (GameObject)Instantiate(item, new Vector2(1, 1), Quaternion.identity);
            items.GetComponent<ItemContainer>().setItem(healthPotion);
            items.GetComponent<ItemContainer>().launchItem();
            if (healthPotion.spawnRate >= luckyNumberH * 2) {
                items = (GameObject)Instantiate(item, new Vector2(1, 1), Quaternion.identity);
                items.GetComponent<ItemContainer>().setItem(healthPotion);
                items.GetComponent<ItemContainer>().launchItem();
            }
        }

        if (critPotion.spawnRate >= luckyNumberC) {
            items = (GameObject)Instantiate(item, new Vector2(1, 1), Quaternion.identity);
            items.GetComponent<ItemContainer>().setItem(critPotion);
            items.GetComponent<ItemContainer>().launchItem();
            if (critPotion.spawnRate >= luckyNumberC * 2) {
                items = (GameObject)Instantiate(item, new Vector2(1, 1), Quaternion.identity);
                items.GetComponent<ItemContainer>().setItem(critPotion);
                items.GetComponent<ItemContainer>().launchItem();
            }
        }

        if (attackPotion.spawnRate >= luckyNumberA) {
            items = (GameObject)Instantiate(item, new Vector2(1, 1), Quaternion.identity);
            items.GetComponent<ItemContainer>().setItem(attackPotion);
            items.GetComponent<ItemContainer>().launchItem();
            if (attackPotion.spawnRate >= luckyNumberA * 2) {
                items = (GameObject)Instantiate(item, new Vector2(1, 1), Quaternion.identity);
                items.GetComponent<ItemContainer>().setItem(attackPotion);
                items.GetComponent<ItemContainer>().launchItem();
            }
        }
            /*
            for (int i = 0; i < numberOfItems; i++) { 
                int randItem = Mathf.FloorToInt (Random.Range (0, 3));
                if (randItem == 0) {
                    items [i] = (GameObject)Instantiate (item, new Vector2 (1, 1), Quaternion.identity);
                    items [i].GetComponent<ItemContainer> ().setItem (critPotion);
                    items [i].GetComponent<ItemContainer> ().launchItem ();
                } else if (randItem == 1) {

                    items [i] = (GameObject)Instantiate (item, new Vector2 (1, 1), Quaternion.identity);
                    items [i].GetComponent<ItemContainer> ().setItem (healthPotion);
                    items [i].GetComponent<ItemContainer> ().launchItem ();
                } else {
                }
                items [i] = (GameObject)Instantiate (item, new Vector2 (1, 1), Quaternion.identity);
                items [i].GetComponent<ItemContainer> ().setItem (attackPotion);
                items [i].GetComponent<ItemContainer> ().launchItem ();
            }
              /*  if(i % 2 == 0) {
                    items[i] = (GameObject)Instantiate(item, new Vector2(1, 1), Quaternion.identity);
                    items[i].GetComponent<ItemContainer>().setItem(critPotion);
                    items[i].GetComponent<ItemContainer>().launchItem();
                    continue;
                }
                items[i] = (GameObject) Instantiate(item, new Vector2(1,1), Quaternion.identity);
                items[i].GetComponent<ItemContainer>().setItem(healthPotion);
                items[i].GetComponent<ItemContainer>().launchItem();
            }*/


        }
    }
