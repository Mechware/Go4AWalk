using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Shop : MonoBehaviour {

    public GameObject[] itemButtons;
    public GameObject shopPanel;
    public GameObject listOfItemsViewer, itemAddedFeedbackText;

    public GameObject itemViewerPanel;
    public Text itemViewerTitle, itemViewerDescription;
    public Image itemViewerImage;

    public static Shop instance;

    item[] items;
    item currentItem;

    void Awake() {
        instance = this;
    }

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    public void openShop() {
        shopPanel.SetActive(true);
        //listOfItemsViewer.SetActive(true);
        //itemViewerPanel.SetActive(false);
        //setItems();
    }

    public void closeShop() {
        shopPanel.SetActive(false);
    }

    public void openItem(item it) {
        currentItem = it;
        itemViewerTitle.text = it.name;
        itemViewerDescription.text = it.description;
        itemViewerImage.sprite = it.icon;
        itemViewerPanel.SetActive(true);
    }

    public void acceptItem() {
        if (currentItem.Equals(ItemList.noItem) || currentItem.Equals(default(item))) {
            print("No item selected");
            return;
        }

        if(currentItem.price > Player.gold.Value) {
            StartCoroutine(showNotEnoughGold());
        } else {
            Player.takeGold(currentItem.price);
            Inventory.items.Add(currentItem);
            StartCoroutine(buyItemFeedback());
        }
    }

    public void ignoreItem() {
       itemViewerPanel.SetActive(false);
    }

    public IEnumerator buyItemFeedback() {
        GameObject addedPrefab = Instantiate(itemAddedFeedbackText, itemViewerPanel.transform, false)
            as GameObject;

        addedPrefab.SetActive(true);

        // Fade color
        Color startColor = addedPrefab.GetComponent<Text>().color;
        startColor.a = 0;
        addedPrefab.GetComponent<Text>().CrossFadeColor(startColor, 1, false, true);

        // drift up and right
        Vector2 startPosition = addedPrefab.transform.localPosition;
        float startTime = Time.time;
        while (startTime + 1 > Time.time) {
            startPosition.x += 20 * Time.deltaTime;
            startPosition.y += 20 * Time.deltaTime;
            addedPrefab.transform.localPosition = startPosition;
            yield return null;
        }

        Destroy(addedPrefab);
    }

    void setItems() {
        int sizeOfShop = 7;
        items = new item[sizeOfShop];
        int i;

        for (i = 0; i < sizeOfShop; i++) {
            items[i] = ItemList.noItem;
        }

        items[0] = ItemList.itemMasterList[ItemList.HEALTH_POTION];
        items[1] = ItemList.itemMasterList[ItemList.CRIT_POTION];
		items [2] = ItemList.itemMasterList [ItemList.ATTACK_POTION];
        items[3] = ItemList.itemMasterList[ItemList.BRONZE_SWORD];

        Action<Button, item> setOnClick = (Button itemButton, item it) => {
            itemButton.onClick.AddListener(() => { openItem(it); });
        };

        for(i=0 ; i < items.Length && !items[i].Equals(ItemList.noItem) ; i++) { 
            itemButtons[i].GetComponentInChildren<Text>().text = items[i].name + "\n" + items[i].description + "\nPrice: " + items[i].price;
            itemButtons[i].GetComponentsInChildren<Image>()[1].sprite = items[i].icon;
            setOnClick(itemButtons[i].GetComponent<Button>(), items[i]);
        }
        for(; i < itemButtons.Length ; i++) {
            itemButtons[i].SetActive(false);
        }
    }

    IEnumerator showNotEnoughGold() {
        string prev = itemViewerDescription.GetComponent<Text>().text;
        if (prev.Equals("Not enough gold...")) {
            yield break;
        }
            
        itemViewerDescription.GetComponent<Text>().text = "Not enough gold...";
        yield return new WaitForSeconds(2);
        itemViewerDescription.GetComponent<Text>().text = prev;
    }
}
