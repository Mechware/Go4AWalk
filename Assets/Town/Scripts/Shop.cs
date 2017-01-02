﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

    public GameObject itemButton0, itemButton1, itemButton2, itemButton3, itemButton4, itemButton5, itemButton6;
    public GameObject itemBackPanel, itemQuests;
    public GameObject itemInfoPanel;
    public Text itemPageTitle, itemPageOverview;

    item[] items;
    int currentItemNum = -1;

    public TownWatchdog tw;
    // Use this for initialization
    void Start() {
        tw = GetComponent<TownWatchdog>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void openShop() {
        setItems();
        tw.menus.SetActive(true);
        tw.shop.SetActive(true);
        tw.exitButton.SetActive(true);
    }

    public void openItem(item it) {
        itemPageTitle.text = it.name;
        itemPageOverview.text = it.description;
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
            itemInfoPanel.SetActive(false);
        }
        
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
        

        items[0] = ItemList.itemMasterList["Health Potion"];
        items[1] = ItemList.itemMasterList["Crit Potion"];

        for(int i = 2 ; i < sizeOfShop ; i++) {
            items[i] = Inventory.noItem;
        }

        itemButton0.GetComponentInChildren<Text>().text = items[0].name + "\n" + items[0].description;
        itemButton1.GetComponentInChildren<Text>().text = items[1].name + "\n" + items[1].description;
        itemButton2.GetComponentInChildren<Text>().text = items[2].name + "\n" + items[2].description;
        itemButton3.GetComponentInChildren<Text>().text = items[3].name + "\n" + items[3].description;
        itemButton4.GetComponentInChildren<Text>().text = items[4].name + "\n" + items[4].description;
        itemButton5.GetComponentInChildren<Text>().text = items[5].name + "\n" + items[5].description;
        itemButton6.GetComponentInChildren<Text>().text = items[6].name + "\n" + items[6].description;
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