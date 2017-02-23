using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour {

    public string statName;
    public BuffManager.BuffType statType;
    public float modifier;
    public int frequency;
    public int duration;
    public GameObject target;

    public void setBuff(string statName, BuffManager.BuffType statType, float modifier, int duration, GameObject target) {
        this.statName = statName;
        gameObject.tag = statName;
        this.statType = statType;
        this.modifier = modifier;
        this.duration = duration;
        this.target = target;

        if (target.GetComponent<Player>() != null) {
            if (statType == BuffManager.BuffType.attack) {
                Player.attackModifier += modifier;
                print("attack " + Player.attackModifier);
                StartCoroutine(timer(() => {
                    print("buff done");
                    Player.attackModifier -= modifier;
                    Destroy(this.gameObject);
                    return true;
                }));

            } else if (statType == BuffManager.BuffType.defense) {
                Player.defenseModifier += modifier;
                print("defense");
                StartCoroutine(timer(() => {
                    print("buff done");
                    Player.defenseModifier -= modifier;
                    Destroy(this.gameObject);
                    return true;
                }));

            } else if (statType == BuffManager.BuffType.crit) {
                Player.critModifier += modifier;
                print("crit");
                StartCoroutine(timer(() => {
                    print("buff done");
                    Player.critModifier -= modifier;
                    Destroy(this.gameObject);
                    return true;
                }));

            }
        } else {
            if (statType == BuffManager.BuffType.attack) {

                Enemy.attackModifierEnemy += modifier;
                print("enemy attack");
                StartCoroutine(timer(() => {
                    print("enemy buff done");
                    Enemy.attackModifierEnemy -= modifier;
                    Destroy(this.gameObject);
                    return true;
                }));
            } else if (statType == BuffManager.BuffType.defense) {
                Enemy.defenseModifierEnemy += modifier;
                print("enemy defense");
                StartCoroutine(timer(() => {
                    print("enemy buff done");
                    Enemy.defenseModifierEnemy -= modifier;
                    Destroy(this.gameObject);
                    return true;
                }));
            }
        }
    }

    public void setBuff(string statName, BuffManager.BuffType statType, float modifier, int frequency, int duration, GameObject target) {
        this.statName = statName;
        gameObject.tag = statName;
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
        Destroy(this.gameObject);
    }

    IEnumerator hurtEnemy(Enemy enemy) {
        for (int i = 0 ; i < duration ; i += frequency) {
            enemy.hit(Mathf.RoundToInt(modifier), false);
            yield return new WaitForSeconds(frequency);
        }
        Destroy(this.gameObject);
    }

    IEnumerator timer(Func<bool> whenDone) {
        
        yield return new WaitForSeconds(duration);
        whenDone();
    }

    public void endBuff() {
        
        StopCoroutine("timer");

        if (target == null || target.GetComponent<Player>() != null) {
            if (statType == BuffManager.BuffType.attack) {
                print("buff cancel");
                Player.attackModifier -= modifier;
            } else if (statType == BuffManager.BuffType.defense) {

                print("buff cancel");
                Player.defenseModifier -= modifier;
            } else if (statType == BuffManager.BuffType.crit) {
                print("buff cancel");
                Player.critModifier -= modifier;
            }
        } else {
            if (statType == BuffManager.BuffType.attack) {
                print("enemy buff cancel");
                Enemy.attackModifierEnemy -= modifier;
            } else if (statType == BuffManager.BuffType.defense) {
                print("enemy buff cancel");
                Enemy.defenseModifierEnemy -= modifier;
            }
        }
    }


    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
