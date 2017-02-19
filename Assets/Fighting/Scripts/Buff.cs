using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour {

    public string name;
    public string statType;
    public float modifier;
    public int frequency;
    public int duration;
    public GameObject target;

    public void setBuff(string name, string statType, float modifier, int duration, GameObject target) {
        this.name = name;
        this.statType = statType;
        this.modifier = modifier;
        this.duration = duration;
        this.target = target;

        if (target.GetComponent<Player>() != null) {
            if (statType == "attack") {
                Player.attackModifier += modifier;
                StartCoroutine(timer(() => {
                    Player.attackModifier -= modifier;
                    return true;
                }));

            } else if (statType == "defense") {
                Player.defenseModifier += modifier;
                StartCoroutine(timer(() => {
                    Player.defenseModifier -= modifier;
                    return true;
                }));

            } else if (statType == "crit") {
                Player.critModifier += modifier;
                StartCoroutine(timer(() => {
                    Player.critModifier -= modifier;
                    return true;
                }));

            }
        } else {
            if (statType == "attack") {

                Enemy.attackModifierEnemy += modifier;                
                StartCoroutine(timer(() => {
                    Enemy.attackModifierEnemy -= modifier;
                    return true;
                }));
            } else if (statType == "defense") {
                Enemy.defenseModifierEnemy += modifier;
                StartCoroutine(timer(() => {
                    Enemy.defenseModifierEnemy -= modifier;
                    return true;
                }));
            }
        }
    }

    public void setBuff(string name, string statType, float modifier, int frequency, int duration, GameObject target) {
        this.name = name;
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
    }

    IEnumerator hurtEnemy(Enemy enemy) {
        for (int i = 0 ; i < duration ; i += frequency) {
            enemy.hit(Mathf.RoundToInt(modifier), false);
            yield return new WaitForSeconds(frequency);
        }
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
