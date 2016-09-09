﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    #region nonstatic
    void Awake() {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("FightingScene")) {
            print("Fighting");
            fighting = true;
            walking = false;
            inTown = false;
        } else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("TownScreen")) {
            print("In town");
            fighting = false;
            walking = false;
            inTown = true;
        } else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("WalkingScreen")) {
            print("Walking");
            fighting = false;
            walking = true;
            inTown = false;
        } else {
            print("Unexpected scene was loaded");
        }

        thisGo = this;
    }

    void Update() {
        
    }

    #endregion

    #region static

    // General variables
    public static bool fighting = false, walking = false, inTown = false;
    public static Player thisGo;



    #region combat
    public static int maxHealth = 100;
    public static int health = 100;
    public static int attackStrength = 5;
    public static int critFactor = 4;
    public static int crit = 0;

    public static void damage(int amount) {
        health -= amount;
        if(health <= 0) {
            combatDie();
        }
    }

    public static int updateCrit(int randFactor) {

        if (crit == 100) return 100;
        
        // Updates crit and returns updated value
        float rand = Random.Range(0.0f, 1.0f);

        if (rand+0.30f > 1f/16000f * (float)(crit*crit)) {
            crit += critFactor + randFactor;
            if (crit > 100) crit = 100;
        } else {
            crit = 0;
        }

        return crit;
    }

    public static void resetCrit() {
        crit = 0;
    }

    public static int getCrit() {
        return crit;
    }

    private static void combatDie() {
        health = 0;
    }

    void dilensABitch() {
        print("Dilen's is az bitch");
    }

    public static int getRegularAttack() {
        float randFactor = Random.Range(-1.0f, 1.0f);
        float fAttackStrength = (float) attackStrength;

        // Update crit to some value
        updateCrit(Mathf.RoundToInt(randFactor));

        return attackStrength + Mathf.RoundToInt(randFactor * fAttackStrength); ;
    }

    public static int getSwipeAttack(Vector2 swipe) {

        //Debug.Log("Swipe length: " + swipe.magnitude);
        //Debug.Log("Swipe angle: " + Mathf.Atan2(swipe.y, swipe.x));

        float fAttack;
        int attack;
        int regAttack = getRegularAttack();

        while(regAttack < attackStrength) {
            regAttack = getRegularAttack();
        }

        if (crit != 100) {
            fAttack = (((float) crit)/10f)*regAttack;
            attack = Mathf.RoundToInt(fAttack);
        } else {
            attack = (int) (15f * regAttack);
        }

        // Reset crit to 0
        resetCrit();

        return attack;
    }
    #endregion 
    #region Walking and questing

    private static float lastTimeGettingDistance;
    public static float totalDistance;
    public static int gold, experience, level;

    public static void updateDistance(float deltaDistance) {
        giveExperience(Mathf.RoundToInt(deltaDistance));
        totalDistance += deltaDistance;
        Questing.move(deltaDistance);
    }



    #endregion
    #region gettersAndSetters
    public static void giveGold(int amount) {
        gold += amount;
    }


    public static int lastLevel = 0;
    public static void giveExperience(int amount) {
        experience += amount;
        if(experience > lastLevel + 100) {
            level++;
            lastLevel = experience;
        }
    }
    #endregion
    #endregion

}
