using UnityEngine;
using System.Collections;

public class TreasureChest : MonoBehaviour {

	public GameObject drop;
    public ParticleSystem ps;

	// Use this for initialization
	void Start () {

	}

	public void openChest () {
        GetComponent<Animator>().SetTrigger("Open");
        Invoke("makeGoldFly", 1);
	}

    void makeGoldFly() {
        ps.gameObject.SetActive(true);
    }
}
