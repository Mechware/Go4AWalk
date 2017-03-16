using UnityEngine;
using System.Collections;

public class TreasureChest : MonoBehaviour {

	public GameObject drop;
    public ParticleSystem ps;

	// Use this for initialization
	void Start () {

	}

	public IEnumerator openChest () {
        GetComponentInChildren<Animator>().SetTrigger("Open");
        yield return new WaitForSeconds(2);
        ps.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        ps.Stop();
	}
}
