using UnityEngine;
using System.Collections;


// all buffs use the same base
public class Buff {

	public GameObject target;
	public int duration;
	public int modifier;

	public Buff(GameObject t, int d, int m) {
		this.target = t;
		this.duration = d;
		this.modifier = d; 
	}

}


public class StatusEffect : MonoBehaviour {


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	//timer takes two values, total duration and remaining duration
	IEnumerator timer(int totalDuration, int remainingDuration) {
		if (remainingDuration == 0) {
			yield return new WaitForSeconds (0);
		}
		yield return new WaitForSeconds (0);
	}

	//public Buff AttackBoost(){
	//}

}

//BuffList
//Buff type -> good or bad
//Buff timer persistent through change of scene
//Buff types -> attack boosts/reduction, D.O.T, Defense boosts/reduction
//When buff is called it must accept a target, it must have a duration, it must have a value for what it changes
