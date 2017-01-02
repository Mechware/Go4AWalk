using UnityEngine;
using System.Collections;

public class ItemContainer : MonoBehaviour {

    public item containingItem;
    private float xforce, yforce;

    // Use this for initialization
    void Start() {


    }

    public void setItem(item it) {
        containingItem = it;
        GetComponent<TextMesh>().text = it.name;
    }

    public void launchItem() {
          xforce = Random.Range(-200f, 200f);
          yforce = Random.Range(-400f, 400f);
          this.GetComponent<Rigidbody2D>().AddForce(new Vector2(xforce, yforce));
    }

    // Update is called once per frame
    void Update() {
       
    }

    void OnMouseDown() {
        Inventory.addItem(containingItem);
        Destroy(gameObject);
    }

    public void onClick() {
        
    }
}
