using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffManager : MonoBehaviour {

    public GameObject critParticles;
    public GameObject attackParticles;
    public GameObject healthParticles;

    public static GameObject critParticlesInstance;
    public static GameObject attackParticlesInstance;

    public enum BuffType {
        attack,
        fire,
        defense,
        crit
    }

    public static IDictionary<string, Buff> buffs = new Dictionary<string, Buff>();

    public static BuffManager instance;

    void Awake() {
        instance = this;
        critParticlesInstance = critParticles;
        attackParticlesInstance = attackParticles;

    }
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void healthParticlesPlay() {
        StartCoroutine(healthParticlesStart());
    }

    IEnumerator healthParticlesStart() {
        healthParticles.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        healthParticles.SetActive(false);
    }

    public void CreateBuff(string statName, BuffType statType, float modifier, int duration, GameObject target) {

        removeCurrentBuff(statName);
        GameObject buffGo = new GameObject();
        Buff buff = buffGo.AddComponent<Buff>();
        buff.setBuff(statName, statType, modifier, duration, target);
        buffs.Add(statName, buff);
    }

    public void CreateDOT(string statName, BuffType statType, float modifier, int duration, int frequency, GameObject target) {

        //removeCurrentBuff(statName);
        GameObject buffGo = new GameObject();
        Buff buff = buffGo.AddComponent<Buff>();
        buff.setBuff(statName, statType, modifier, frequency, duration, target);
        buffs.Add(statName, buff);
    }

    public static void removeCurrentBuff(string statName) {
        if (buffs.ContainsKey(statName) && buffs[statName] != null) {
            buffs[statName].endBuff();
            Destroy(buffs[statName].gameObject);
            buffs.Remove(statName);
        } else if (buffs.ContainsKey(statName) && buffs[statName] == null) {
            buffs.Remove(statName);
        }
    }
}
