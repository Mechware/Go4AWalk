using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour {

    public string statType;
    public float modifier;
    public int frequency;
    public int duration;
    public GameObject target;

    public void setBuff(string statType, float modifier, int duration, GameObject target) {
        this.statType = statType;
        this.modifier = modifier;
        this.duration = duration;
        this.target = target;

        if (target.GetComponent<Player>() != null) {
            if (statType == "attack") {
                Player.attackModifier += modifier;
                print("attack");
                StartCoroutine(timer(() => {
                    print("buff done");
                    Player.attackModifier -= modifier;
                    Destroy(this);
                    return true;
                }));

            } else if (statType == "defense") {
                Player.defenseModifier += modifier;
                print("defense");
                StartCoroutine(timer(() => {
                    print("buff done");
                    Player.defenseModifier -= modifier;
                    Destroy(this);
                    return true;
                }));

            } else if (statType == "crit") {
                Player.critModifier += modifier;
                print("crit");
                StartCoroutine(timer(() => {
                    print("buff done");
                    Player.critModifier -= modifier;
                    Destroy(this);
                    return true;
                }));

            }
        } else {
            if (statType == "attack") {

                Enemy.attackModifierEnemy += modifier;
                print("enemy attack");
                StartCoroutine(timer(() => {
                    print("enemy buff done");
                    Enemy.attackModifierEnemy -= modifier;
                    Destroy(this);
                    return true;
                }));
            } else if (statType == "defense") {
                Enemy.defenseModifierEnemy += modifier;
                print("enemy defense");
                StartCoroutine(timer(() => {
                    print("enemy buff done");
                    Enemy.defenseModifierEnemy -= modifier;
                    Destroy(this);
                    return true;
                }));
            }
        }
    }

    public void setBuff(string statType, float modifier, int frequency, int duration, GameObject target) {
        this.statType = statType;
        this.modifier = modifier;
        this.frequency = frequency;
        this.duration = duration;
        this.target = target;

        if (target.GetComponent<Player>() != null) {
            StartCoroutine(hurtPlayer());
        } else {
            StartCoroutine(hurtEnemy(target.GetComponent<Enemy>()));
        }
    }

    IEnumerator hurtPlayer() {
        for(int i = 0 ; i < duration ; i += frequency) {
            Player.damage(Mathf.RoundToInt(modifier));
            yield return new WaitForSeconds(frequency);
        }
        Destroy(this);
    }

    IEnumerator hurtEnemy(Enemy enemy) {
        for (int i = 0 ; i < duration ; i += frequency) {
            enemy.hit(Mathf.RoundToInt(modifier), false);
            yield return new WaitForSeconds(frequency);
        }
        Destroy(this);
    }

    IEnumerator timer(Func<bool> whenDone) {
        
        yield return new WaitForSeconds(duration);
        whenDone();
    }


    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this.gameObject);
      
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
