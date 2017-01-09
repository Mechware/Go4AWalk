using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public GameObject damageIndicator, critIndicator, item;
    public int health, damage, goldToGive;
    public float timeBetweenAttacks;
    public PlayerFacade player;
    public int spawnRate;

    private StatusBar healthBar;
    private GameObject damageIndicatorParent;
    private int maxHealth;
    private EnemyWatchdog enemyWatchdog;
    

    // Use this for initialization
    void Start () {
        goldToGive = Mathf.RoundToInt(Random.Range(1f, 100f));
        damageIndicatorParent = GameObject.Find("DamageIndicators");
        healthBar = GetComponentInChildren<StatusBar>();
        maxHealth = health;
        enemyWatchdog = GameObject.Find("Watchdog").GetComponent<EnemyWatchdog>();
        player = GameObject.Find("Manager").GetComponent<PlayerFacade>();
        StartCoroutine("attack");
        
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void hit(int damage, bool isCrit) {
        if(health == 0) {
            return;
        }

        GameObject go;
        if (isCrit) {
            go = (GameObject) Instantiate(critIndicator, damageIndicatorParent.transform, false);
        } else {
            go = (GameObject) Instantiate(damageIndicator, damageIndicatorParent.transform, false);
        }
        if(damage == 0) {
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
        while(true) {
            yield return new WaitForSeconds(timeBetweenAttacks);
            GetComponentInChildren<Animator>().SetTrigger("attacking");
            player.hitPlayer(damage);
        }
    }

    void die() {
        StopCoroutine("attack");
        GetComponentInChildren<Animator>().SetBool("dying", true);
        Destroy(healthBar.transform.parent.gameObject);

        Player.giveGold(goldToGive);
        Destroy(GetComponent<Collider2D>());
        Destroy(gameObject, 15);
        spawnItems(Mathf.FloorToInt(Random.Range(0, 4)));
        enemyWatchdog.encounterIsOver();
        
    }

 
    void spawnItems(int numberOfItems) {

        GameObject[] items = new GameObject[numberOfItems];

        item healthPotion = ItemList.itemMasterList[ItemList.HEALTH_POTION];
        item critPotion = ItemList.itemMasterList[ItemList.CRIT_POTION];
		item attackPotion = ItemList.itemMasterList [ItemList.ATTACK_POTION];

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
