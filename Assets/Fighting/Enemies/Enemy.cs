using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public GameObject damageIndicator, critIndicator;
    public int health, damage, goldToGive;
    public float timeBetweenAttacks;

    private StatusBar healthBar;
    private GameObject canvas;
    private int maxHealth;
    // Use this for initialization
	void Start () {
        goldToGive = Mathf.RoundToInt(Random.Range(1f, 100f));
        canvas = GameObject.Find("MainCanvas");
        healthBar = GetComponentInChildren<StatusBar>();
        maxHealth = health;
	}
	
	// Update is called once per frame
	void Update () {
        attack();
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

    // For attacking player
    private float lastAttack;

    void attack() {
        if (Time.time - lastAttack > timeBetweenAttacks) {
            GetComponent<Animator>().SetTrigger("attacking");
            lastAttack = Time.time;
            Player.damage(damage);
        }
    }

    void die() {
        GetComponent<Animator>().SetBool("dying", true);
        Player.giveGold(goldToGive);
        Destroy(this.gameObject, 2);
    }
}
