using UnityEngine;
using System.Collections;

public class ItemContainer : MonoBehaviour {

    public item containingItem;
    private float xforce, yforce;
    private int gravity = 0;

    // Use this for initialization
    void Start() {


    }

    public void setItem(item it) {
        containingItem = it;
        GetComponent<TextMesh>().text = it.name;
    }

    public void launchItem() {
          xforce = Random.Range(-40f, 40f);
          yforce = Random.Range(-40f, 40f);
        //gameObject.AddComponent<Rigidbody2D>().gravityScale = gravity;
          this.GetComponent<Rigidbody2D>().AddForce(new Vector2(xforce, yforce));
        print(xforce + yforce);
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
