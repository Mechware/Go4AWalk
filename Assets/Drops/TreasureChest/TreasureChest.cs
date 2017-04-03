using UnityEngine;
using System.Collections;

public class TreasureChest : MonoBehaviour {

	public GameObject drop;
    public ParticleSystem particles;
    public GameObject itemContainer;
    public AudioSource openSound;


	// Use this for initialization
	void Start () {

	}

	public IEnumerator openChest () {
        GetComponentInChildren<Animator>().SetTrigger("Open");
        yield return new WaitForSeconds(2);
        particles.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        particles.Stop();
	}

    public IEnumerator itemDrop(item booty, System.Action callWhenDone) {
        print(booty.name);
        // open chest
        GetComponentInChildren<Animator>().SetTrigger("Open");
        openSound.Play();
        yield return new WaitForSeconds(2);
        // shoot gold particles
        particles.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        particles.Stop();
        // shoot item
        GameObject item = GameObject.Instantiate(itemContainer, this.transform) as GameObject;
        print(item);
        item.GetComponent<ItemContainer>().setItem(booty);
        item.GetComponent<ItemContainer>().launchItem();
        yield return new WaitForSeconds(3f);
        callWhenDone();
        // shoot amount of gold gotten
        



    }
}
