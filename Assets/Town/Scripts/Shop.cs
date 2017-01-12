using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

    public GameObject itemButton0, itemButton1, itemButton2, itemButton3, itemButton4, itemButton5, itemButton6;
    public GameObject itemBackPanel, itemQuests;
    public GameObject itemInfoPanel, itemContentPanel, itemAddedPrefab;
    public Text itemPageTitle, itemPageOverview;
    public Image itemImage;

    item[] items;
    int currentItemNum = -1;

    public TownWatchdog townWatchdog;
    // Use this for initialization
    void Start() {
        townWatchdog = GetComponent<TownWatchdog>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void openShop() {
        setItems();
        townWatchdog.open(townWatchdog.shop);
    }

    public void openItem(item it) {
        itemPageTitle.text = it.name;
        itemPageOverview.text = it.description;
        itemImage.sprite = it.icon;
        itemInfoPanel.SetActive(true);
    }

    public void acceptItem() {
        if (currentItemNum == -1) {
            print("No item selected");
            return;
        }

        if(items[currentItemNum].price > Player.gold.Value) {
            StartCoroutine(showNotEnoughGold());
        } else {
            Player.takeGold(items[currentItemNum].price);
            Inventory.addItem(items[currentItemNum]);
            StartCoroutine(buyItemFeedback());
        }
    }

    IEnumerator buyItemFeedback() {
        GameObject addedPrefab = Instantiate(itemAddedPrefab, itemContentPanel.transform, false)
            as GameObject;

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

    public void ignoreItem() {
        itemInfoPanel.SetActive(false);
    }

    public void selectItem(int buttonNum) {
        openItem(items[buttonNum]);
        currentItemNum = buttonNum;
    }

    void setItems() {
        int sizeOfShop = 7;
        items = new item[sizeOfShop];

        for (int i = 0; i < sizeOfShop; i++)
        {
            items[i] = ItemList.noItem;
        }

        items[0] = ItemList.itemMasterList[ItemList.HEALTH_POTION];
        items[1] = ItemList.itemMasterList[ItemList.CRIT_POTION];
		items [2] = ItemList.itemMasterList [ItemList.ATTACK_POTION];
        items[3] = ItemList.itemMasterList[ItemList.BRONZE_SWORD];
              

        itemButton0.GetComponentInChildren<Text>().text = items[0].name + "\n" + items[0].description;
        itemButton1.GetComponentInChildren<Text>().text = items[1].name + "\n" + items[1].description;
        itemButton2.GetComponentInChildren<Text>().text = items[2].name + "\n" + items[2].description;
        itemButton3.GetComponentInChildren<Text>().text = items[3].name + "\n" + items[3].description;
        itemButton4.GetComponentInChildren<Text>().text = items[4].name + "\n" + items[4].description;
        itemButton5.GetComponentInChildren<Text>().text = items[5].name + "\n" + items[5].description;
        itemButton6.GetComponentInChildren<Text>().text = items[6].name + "\n" + items[6].description;
        itemButton0.GetComponentsInChildren<Image>()[1].sprite = items[0].icon;
        itemButton1.GetComponentsInChildren<Image>()[1].sprite = items[1].icon;
        itemButton2.GetComponentsInChildren<Image>()[1].sprite = items[2].icon;
        itemButton3.GetComponentsInChildren<Image>()[1].sprite = items[3].icon;
        itemButton4.GetComponentsInChildren<Image>()[1].sprite = items[4].icon;
        itemButton5.GetComponentsInChildren<Image>()[1].sprite = items[5].icon;
        itemButton6.GetComponentsInChildren<Image>()[1].sprite = items[6].icon;
    }

    IEnumerator showNotEnoughGold() {
        string prev = itemPageOverview.GetComponent<Text>().text;
        if (prev.Equals("Not enough gold...")) {
            yield break;
        }
            
        itemPageOverview.GetComponent<Text>().text = "Not enough gold...";
        yield return new WaitForSeconds(2);
        itemPageOverview.GetComponent<Text>().text = prev;
    }
}
