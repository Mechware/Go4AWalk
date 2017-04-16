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

    public static bool critActive = false;
    public static bool attackActive = false;


    

    public void setBuff(string statName, BuffManager.BuffType statType, float modifier, int duration, GameObject target) {
        this.statName = statName;
        this.statType = statType;
        this.modifier = modifier;
        this.duration = duration;
        this.target = target;

        if (target.GetComponent<Player>() != null) {
            if (statType == BuffManager.BuffType.attack) {
                Player.attackModifier += modifier;
                BuffManager.attackParticlesInstance.SetActive(true);
                attackActive = true;
                print(Player.attackModifier);
                StartCoroutine(timer(() => {
                    Player.attackModifier -= modifier;
                    if (BuffManager.attackParticlesInstance!=null) {
                        BuffManager.attackParticlesInstance.SetActive(false);
                    }             
                    Destroy(this.gameObject);
                    attackActive=false;
                    return true;
                }));

            } else if (statType == BuffManager.BuffType.defense) {
                Player.defenseModifier += modifier;
                StartCoroutine(timer(() => {
                    Player.defenseModifier -= modifier;
                    Destroy(this.gameObject);
                    return true;
                }));

            } else if (statType == BuffManager.BuffType.crit) {
                Player.critModifier += modifier;
                BuffManager.critParticlesInstance.SetActive(true);
                critActive = true;

                StartCoroutine(timer(() => {
                    Player.critModifier -= modifier;
                    if(BuffManager.critParticlesInstance != null) {

                        BuffManager.critParticlesInstance.SetActive(false);


                    }
                    Destroy(this.gameObject);
                    critActive=false;
                    return true;
                }));

            }
        } else {
            if (statType == BuffManager.BuffType.attack) {

                Enemy.attackModifierEnemy += modifier;
                StartCoroutine(timer(() => {
                    Enemy.attackModifierEnemy -= modifier;
                    Destroy(this.gameObject);
                    return true;
                }));
            } else if (statType == BuffManager.BuffType.defense) {
                Enemy.defenseModifierEnemy += modifier;
                StartCoroutine(timer(() => {
                    Enemy.defenseModifierEnemy -= modifier;
                    Destroy(this.gameObject);
                    return true;
                }));
            }
        }
    }

    public void setBuff(string statName, BuffManager.BuffType statType, float modifier, int frequency, int duration, GameObject target) {
        this.statName = statName;

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
        if (duration >= 0) {
            for (int i = 0 ; i < duration ; i += frequency) {

                Player.damage(Mathf.RoundToInt(modifier));
                yield return new WaitForSeconds(frequency);
            }

        } else {
            Player.damage(Mathf.RoundToInt(modifier));
            yield return new WaitForSeconds(frequency);
            if (Player.isHeal) {
                setBuff(statName, statType, modifier, frequency, duration, target);
            }
        }

        BuffManager.removeCurrentBuff(statName);

        Destroy(this.gameObject);
    }

    IEnumerator hurtEnemy(Enemy enemy) {
  
            for (int i = 0 ; i < duration ; i += frequency) {
 
                if (target != null) {
                    enemy.poison(Mathf.RoundToInt(modifier));
                    yield return new WaitForSeconds(frequency);
                } else
                    break;

            }
        

        BuffManager.removeCurrentBuff(statName);

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
                Player.attackModifier -= modifier;
            } else if (statType == BuffManager.BuffType.defense) {
                Player.defenseModifier -= modifier;
            } else if (statType == BuffManager.BuffType.crit) {
                Player.critModifier -= modifier;
            }
        } else {
            if (statType == BuffManager.BuffType.attack) {
                Enemy.attackModifierEnemy -= modifier;
            } else if (statType == BuffManager.BuffType.defense) {
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
