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
        this.GetComponentInChildren<SpriteRenderer>().sprite = it.icon;

    }

    public void launchItem() {
        xforce = Random.Range(-200f, 200);
        yforce = Random.Range(-400f, 400);
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(xforce, yforce));
    }

    public void launchItemSlow() {
        xforce = Random.Range(-100f, 100f);
        yforce = Random.Range(-100f, 100f);
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(xforce, yforce));
    }

    // Update is called once per frame
    void Update() {
        CheckInput.checkTapOrMouseDown(itemSelected);
    }

    void itemSelected(Collider2D hitCollider) {

        if (hitCollider != this.GetComponentInChildren<Collider2D>())
            return;

        if (containingItem.type == itemType.Potion) {
            PotionInventory.addPotion(containingItem);
            Destroy(gameObject);
        } else {
            Inventory.items.Add(containingItem);
            Destroy(gameObject);
        }



        return;
    }
}
