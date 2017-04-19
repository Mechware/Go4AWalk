/**********************************************************
 * Inventory.cs
 *  The class dedicated to keeping track of the user's 
 *  Inventory. 
 * 
 * 
 * 
 * *******************************************************/


using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// A structure that is an item. I left the "otherInfo" object
/// for future use, incase we want to implement something else
/// that I couldn't think of right now.
/// </summary>
public struct item {
    // Name of item (for display)
    public string name;
    // Description of item (for display)
    public string description;
    // Price.. self explanatory
    public int price;
    // The amount of stuff it gives you, ex:
    // Amount of health given make, duration of
    // lure/repel, amount of attack boost, etc.
    public float attributeValue;
    // Just other info that was may need 
    public object otherInfo;
    // The item type
    public itemType type;
    // Method to call to use item. 
    // Returns whether or not the item should be removed
    public Func<bool> useItem;
    // Item's icon
    public Sprite icon;
    // Item's spawnrate (based on the item not on the enemy, which might be a different thing to add later)
    public int spawnRate;
    // Stats of weapons
    public int baseAttack;
    public float attackModifier;
    public float critModifier;

    public item(string name, string description, int price, float attributeValue, object otherInfo, itemType type, Func<bool> useItem, Sprite icon, int spawnRate, int baseAttack, float attackModifier, float critModifier) {
        this.name = name;
        this.description = description;
        this.price = price;
        this.attributeValue = attributeValue;
        this.otherInfo = otherInfo;
        this.type = type;
        this.useItem = useItem;
        this.icon = icon;
        this.spawnRate = spawnRate;
        this.baseAttack = baseAttack; ;
        this.attackModifier = attackModifier;
        this.critModifier = critModifier;
    }
}


public class Inventory : MonoBehaviour {


    public const int INVENTORY_SIZE = 10;
    private static List<item> _items;

    public static List<item> items {
        get {
            if (_items == null)
                _items = new List<item>();
            return _items;
        }
    }

    public static void addItem(item item) {
        if (items == null)
            _items = new List<item>();
        items.Add(item);
        save();
    }

    public static string[] getInventory() {
        List<string> itemNames = new List<string>();
        foreach(item it in items) {
            itemNames.Add(it.name);
        }
        return itemNames.ToArray();
    }

    public static void removeItem(item item) {
        items.Remove(item);
        items.TrimExcess();
        PersistentUIElements.instance.updateItems();
        save();
    }

    public static void setInventory(string[] itemNames) {
        foreach(string itemName in itemNames) {
            items.Add(ItemList.itemMasterList[itemName]);
        }
    }

    public static void save() {
        
        string[] itemNames = getInventory();
        int i;
        for (i = 0 ; i < itemNames.Length ; i++) {
            PlayerPrefs.SetString("Inventory_" + i, itemNames[i]);
        }
        while(PlayerPrefs.HasKey("Inventory_"+i)) {
            PlayerPrefs.DeleteKey("Inventory_" + i);
            i++;
        }
        PlayerPrefs.Save();
    }

    public static void load() {
        List<string> itemNames = new List<string>();
        int i = 0;
        while (true) {
            if (!PlayerPrefs.HasKey("Inventory_" + i)) {
                break;
            }
            itemNames.Add(PlayerPrefs.GetString("Inventory_" + i));
            i++;
        }
        if (itemNames.Count > 0) {
            _items = new List<item>();
            setInventory(itemNames.ToArray());
        }

    }

    public static void clear() {
        _items = null;
    }
}
