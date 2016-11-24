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
    public int attributeValue;
    // Just other info that was may need 
    public object otherInfo;
    // The item type
    public itemType type;
    // Method to call to use item
    public Action useItem;

    public item(string name, string description, int price, int attributeValue, object otherInfo, itemType type, Action useItem) {
        this.name = name;
        this.description = description;
        this.price = price;
        this.attributeValue = attributeValue;
        this.otherInfo = otherInfo;
        this.type = type;
        this.useItem = useItem;
    }
}

/// <summary>
/// All the different item types that exist in our game.
/// </summary>
public enum itemType {
    Potion,
    Poison,
    Equipment,
}

public class Inventory : MonoBehaviour {

    public static item noItem = new item("", "", 0, 0, null, itemType.Equipment, () => { });
    public const int INVENTORY_SIZE = 6;
    public static Action onValueChanged;
    private static item[] items;

    // Use this for initialization
    void Awake() {
        initalizeInventory();
        onValueChanged = null;
        onValueChanged += () => { };
    }

    static void initalizeInventory() {
        if (items != null)
            return;
        items = new item[INVENTORY_SIZE];
        for(int i = 0 ; i < INVENTORY_SIZE ; i++) {
            items[i] = noItem;
        }
    }

    public static void addItem(item it, int pos) {
        initalizeInventory();
        items[pos] = it;
        onValueChanged.Invoke();
    }

    public static void addItem(item it) {
        initalizeInventory();

        for(int i = 0 ; i < INVENTORY_SIZE ; i++) {
            if (items[i].Equals(noItem)) {
                addItem(it, i);
                return;
            }
        }
    }

    public static void removeItem(int pos) {

        initalizeInventory();

        if (items[pos].Equals(noItem)) {
            print("Inventory does not contain an item at position: " + pos);
            return;
        }
        
        items[pos] = noItem;
        onValueChanged.Invoke();
    }

    private static void Items_OnValueChange() {
        print("Okay now it's being thronw?");
    }

    public static void use(item it) {
        it.useItem();
    }

    public static void use(int number) {
        print("Using item: " + number);
        items[number].useItem();
        removeItem(number);
    }

    public void useItem(int number) {
        print("Using item: " + number);
        items[number].useItem();
        removeItem(number);
    }

    public static item getItem(int pos) {
        return items[pos];
    }
}
