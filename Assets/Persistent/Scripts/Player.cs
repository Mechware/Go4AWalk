using UnityEngine;
using System.Collections;
using Gamelogic.Extensions;

public class Player : MonoBehaviour {

    public const string TOWN_LEVEL = "TownScreen";
    public const string FIGHTING_LEVEL = "FightingScene";
    public const string WALKING_LEVEL = "WalkingScreen";

    public static bool fighting = false, walking = false, inTown = false, died = false;
    public static Player instance;

    public static float totalDistance = 0;
    public static ObservedValue<int> gold, experience, level, lootGold, distance;
    public static int experienceOfLastLevel = 0;
    
    public static int maxHealth = 100;
    public static int health = 100;

    public static ObservedValue<int> crit;
    private static int attackStrength = 5;
    private static int critFactor = 4;
    public static float attackModifier = 1;
    public static float critModifier = 1; 
    public static item equippedWeapon;

    #region nonstatic

    void Awake() {
        if (gold == null) {
            gold = new ObservedValue<int>(0);
            lootGold = new ObservedValue<int>(0);
            experience = new ObservedValue<int>(0);
            level = new ObservedValue<int>(1);
            distance = new ObservedValue<int>(0);
        } else {
            gold = new ObservedValue<int>(gold.Value);
            lootGold = new ObservedValue<int>(lootGold.Value);
            experience = new ObservedValue<int>(experience.Value);
            level = new ObservedValue<int>(level.Value);
            distance = new ObservedValue<int>(Mathf.RoundToInt(totalDistance));
        }
        crit = new ObservedValue<int>(0);
        attackStrength = 5 + level.Value;
        maxHealth = 100 + 10 * level.Value;

        equippedWeapon = ItemList.noItem;
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(FIGHTING_LEVEL)) {
            //print("Fighting");
            fighting = true;
            walking = false;
            inTown = false;
        } else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(TOWN_LEVEL)) {
            //print("In town");
            fighting = false;
            walking = false;
            inTown = true;
            gold = new ObservedValue<int>(lootGold.Value + gold.Value);
            lootGold = new ObservedValue<int>(0);
        } else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(WALKING_LEVEL)) {
            //print("Walking");
            fighting = false;
            walking = true;
            inTown = false;
        } else {
            print("Unexpected scene was loaded");
        }

        if (died) {
            health = 50;
            lootGold = new ObservedValue<int>(0);
            died = false;
        }

        instance = this;
    }

    void Update() {
        
    }


    #endregion

    #region static

    #region combat
    public static void damage(int amount) {
        health -= amount;
        if(health <= 0) {
            die();
        }
    }

    public static void equipWeapon(item newItem)
    {
        UnityEngine.Assertions.Assert.AreEqual(newItem.type, itemType.Weapon, "Trying to equip something that is not a weapon.");

        attackStrength -= equippedWeapon.baseAttack;                  
        equippedWeapon = newItem;
        attackStrength += equippedWeapon.baseAttack;           
        
    }

    public static int updateCrit(int randFactor) {

        if (crit.Value == 100) return 100;
        
        // Updates crit and returns updated value
        float rand = Random.Range(0.0f, 1.0f);

        if ((rand+0.30f)*critModifier > 1f/16000f * (float)(crit.Value*crit.Value)) {
            crit.Value += critFactor + randFactor;
            if (crit.Value > 100) crit.Value = 100;
        } else {
            crit.Value = 0;
        }

        return crit.Value;
    }

    public static void resetCrit() {
        crit.Value = 0;
    }

    public static int getCrit() {
        return crit.Value;
    }

    private static void die() {      
        UnityEngine.SceneManagement.SceneManager.LoadScene(TOWN_LEVEL);
        died = true;
    }

    // Returns a regular random attack
    public static int getRegularAttack() {
        float randFactor = Random.Range(-1.0f, 1.0f);
        float fAttackStrength = (float) attackStrength;

        // Update crit to some value
        updateCrit(Mathf.RoundToInt(randFactor));

        return Mathf.RoundToInt(fAttackStrength*attackModifier) + Mathf.RoundToInt(randFactor * fAttackStrength); ;
    }

    /// <summary>
    /// Get the damage from a swipe
    /// </summary>
    /// <param name="swipe">Vector2 of the swipe the was performed</param>
    /// <returns></returns>
    public static int getCriticalAttack(Vector2 swipe) {

        //Debug.Log("Swipe length: " + swipe.magnitude);
        //Debug.Log("Swipe angle: " + Mathf.Atan2(swipe.y, swipe.x));

        float fAttack;
        int attack;

        if (crit.Value < 100) {
            fAttack = (((float) crit.Value)/5f)*attackStrength*attackModifier;
            attack = Mathf.RoundToInt(fAttack);
        } else {
            attack = (int) (150f/5f * attackStrength*attackModifier);
        }

        return attack;
    }
    #endregion 

    #region gettersAndSetters
    /// <summary>
    /// Give a certain amount of gold to the player
    /// </summary>
    /// <param name="amount">Amount of gold to give</param>
    public static void giveGold(int amount) {
        gold.Value += amount;
    }

    public static void giveLootGold(int amount)
    {
        lootGold.Value += amount;
    }

    /// <summary>
    /// Take gold from the player
    /// </summary>
    /// <param name="amount">Amount of gold to be taken</param>
    public static void takeGold(int amount) {
        gold.Value -= amount;
    }

    public static void takeLootGold(int amount)
    {
        lootGold.Value -= amount;
    }

    /// <summary>
    /// Give a certain amount of experience to the player
    /// </summary>
    /// <param name="amount">Amount of gold to give</param>
    public static void giveExperience(int amount) {
        experience.Value += amount;
        if(experience.Value > experienceOfLastLevel + 100) {
            level.Value++;
            experienceOfLastLevel = experience.Value;
        }
    }

    public static void giveHealth(int amount) {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
        else if (health <= 0)
            die();
    }

	public static void giveAttack(int amount, int duration) {
		//gives player amount of bonus attack for duration (s) 
		int regularAttack = attackStrength; 
		attackStrength = attackStrength + amount;
		//start buff countdown timer 
		instance.StartCoroutine(instance.buffTimer(duration,regularAttack));

	}

	public static void resetAttack(int attack) { 
		attackStrength = attack;
	}

	IEnumerator buffTimer(int duration,int attack) { 
		//buff countdown timer. 
		yield return new WaitForSeconds (duration);
		resetAttack (attack);
		print ("Buff ended");

	}


    #endregion
    #endregion

}
