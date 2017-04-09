using UnityEngine;
using System.Collections;

public class TreasureChest : MonoBehaviour {

	public GameObject drop;
    public ParticleSystem particles;
    public GameObject itemContainer;
    public AudioSource openSound;
    public AudioSource goldSound;
   


	// Use this for initialization
	void Start () {

	}

	
	

    public IEnumerator itemDrop(item booty, System.Action callWhenDone) {
        print(booty.name);
        // open chest
        GetComponentInChildren<Animator>().SetTrigger("Open");
        openSound.Play();
        yield return new WaitForSeconds(2);
        // shoot gold particles
        particles.gameObject.SetActive(true);
        particles.Play();
        yield return new WaitForSeconds(1f);
        particles.Stop();
        goldSound.Stop();
        // shoot item
        GameObject item = GameObject.Instantiate(itemContainer, this.transform) as GameObject;
        print(item);
        item.GetComponent<ItemContainer>().setItem(booty);
        item.GetComponent<ItemContainer>().chestItem();
        yield return new WaitForSeconds(3f);
        callWhenDone();
        // shoot amount of gold gotten
        



    }
}
