using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public GameObject damageIndicator, critIndicator, item;
    public int health, damage, goldToGive;
    public float timeBetweenAttacks;
    public PlayerFacade player;

    private StatusBar healthBar;
    private GameObject canvas;
    private int maxHealth;
    private EnemyWatchdog ew;
    

    // Use this for initialization
    void Start () {
        goldToGive = Mathf.RoundToInt(Random.Range(1f, 100f));
        canvas = GameObject.Find("MainCanvas");
        healthBar = GetComponentInChildren<StatusBar>();
        maxHealth = health;
        ew = GameObject.Find("Watchdog").GetComponent<EnemyWatchdog>();
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
            go = (GameObject) Instantiate(critIndicator, canvas.transform, false);
        } else {
            go = (GameObject) Instantiate(damageIndicator, canvas.transform, false);
        }
        if(damage == 0) {
            go.GetComponent<DamageIndicator>().setText("MISS");
        } else {
            go.GetComponent<DamageIndicator>().setText(""+damage);
        }

        GetComponent<Animator>().SetTrigger("flinch");
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
            GetComponent<Animator>().SetTrigger("attacking");
            player.hitPlayer(damage);
        }
    }

    void die() {
        StopCoroutine("attack");
        GetComponent<Animator>().SetBool("dying", true);
        Destroy(healthBar.transform.parent.gameObject);

        Player.giveGold(goldToGive);
        Destroy(GetComponent<Collider2D>());
        Destroy(gameObject, 5);
        spawnItems(2);
        ew.encounterIsOver();
        
    }

    void spawnItems(int numberOfItems) {

        GameObject[] items = new GameObject[numberOfItems];

        item healthPotion = ItemList.itemMasterList["Health Potion"];

        for (int i = 0 ; i < numberOfItems ; i++) { 
            items[i] = (GameObject) Instantiate(item, new Vector2(1,1), Quaternion.identity);
            items[i].GetComponent<ItemContainer>().setItem(healthPotion);
            items[i].GetComponent<ItemContainer>().launchItem();
        }

        
        


    }
}
