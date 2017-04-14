using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public GameObject damageIndicator, critIndicator, poisonIndicator, expIndicator, goldIndicator, item;
    public int health, damage, goldToGive, expToGive;
    public AudioClip dieSound, attackSound;
    public AudioSource sound;
    // Armor and sturdy are modifiers that dictate how much reduced damage enemies take from normal and critical strikes. Small values = less damage (obvs)
    // its just kinda weird that "high" armor is actually a low number.
    // Also balance wise, it probably makes sense to make a highly armored enemy have low sturdy so that when you do land a critical strike it does extra damage
    public float armor = 1;
    public float sturdy = 1;
    public float timeBetweenAttacks;
    public int spawnRate;
    public string itemName;
    public int lootSpawnRate;

    private StatusBar healthBar;
    private GameObject damageIndicatorParent;
    private int maxHealth;
    public static float attackModifierEnemy = 1;
    public static float defenseModifierEnemy = 1;
    public bool isDead = false;



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
            // If hit is a crit modify damage by the enemies sturdiness, if normal attack modify damage by the enemies armor
            if (isCrit) {
                go.GetComponent<DamageIndicator>().setText(Mathf.RoundToInt(damage * sturdy).ToString());
            } else {
                go.GetComponent<DamageIndicator>().setText(Mathf.RoundToInt(damage * armor).ToString());
            }

        }

        GetComponentInChildren<Animator>().ResetTrigger("flinch");
        GetComponentInChildren<Animator>().SetTrigger("flinch");

        if (isCrit) {
            health -= Mathf.RoundToInt(damage * sturdy);
            if(dieSound != null) {
                sound.clip = dieSound;
                sound.Play();
            }
        } else {
            health -= Mathf.RoundToInt(damage * armor);
        }

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
            if(attackSound != null) {
                sound.clip = attackSound;
                sound.Play();
            }
            Player.damage(damage);
        }
    }

    void die() {
        GetComponentInChildren<Animator>().SetBool("dying", true);
        isDead = true;
        Destroy(healthBar.transform.parent.gameObject);
        if(dieSound != null) {
            sound.clip = dieSound;
            sound.Play();
        }

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
        int luckyNumberLoot = Random.Range(1, 100);

        if (lootSpawnRate >= luckyNumberLoot && itemName != null) {
            item lootDrop = ItemList.itemMasterList[itemName];
            items = (GameObject)Instantiate(item, new Vector3(0,-3, 0), Quaternion.identity);
            items.GetComponent<ItemContainer>().setItem(lootDrop);
            items.GetComponent<ItemContainer>().launchItem();
        }
        if (healthPotion.spawnRate >= luckyNumberH) {
            items = (GameObject)Instantiate(item, new Vector3(0, -3, 0), Quaternion.identity);
            items.GetComponent<ItemContainer>().setItem(healthPotion);
            items.GetComponent<ItemContainer>().launchItem();
            if (healthPotion.spawnRate >= luckyNumberH * 2) {
                items = (GameObject)Instantiate(item, new Vector3(0, -3, 0), Quaternion.identity);
                items.GetComponent<ItemContainer>().setItem(healthPotion);
                items.GetComponent<ItemContainer>().launchItem();
            }
        }

        if (critPotion.spawnRate >= luckyNumberC) {
            items = (GameObject)Instantiate(item, new Vector3(0, -3, 0), Quaternion.identity);
            items.GetComponent<ItemContainer>().setItem(critPotion);
            items.GetComponent<ItemContainer>().launchItem();
            if (critPotion.spawnRate >= luckyNumberC * 2) {
                items = (GameObject)Instantiate(item, new Vector3(0, -3, 0), Quaternion.identity);
                items.GetComponent<ItemContainer>().setItem(critPotion);
                items.GetComponent<ItemContainer>().launchItem();
            }
        }

        if (attackPotion.spawnRate >= luckyNumberA) {
            items = (GameObject)Instantiate(item, new Vector3(0, -3, 0), Quaternion.identity);
            items.GetComponent<ItemContainer>().setItem(attackPotion);
            items.GetComponent<ItemContainer>().launchItem();
            if (attackPotion.spawnRate >= luckyNumberA * 2) {
                items = (GameObject)Instantiate(item, new Vector3(0, -3, 0), Quaternion.identity);
                items.GetComponent<ItemContainer>().setItem(attackPotion);
                items.GetComponent<ItemContainer>().launchItem();
            }
        }
    }


}
