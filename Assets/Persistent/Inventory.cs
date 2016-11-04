/**********************************************************
 * Inventory.cs
 *  The class dedicated to keeping track of the user's 
 *  Inventory. 
 * 
 * 
 * 
 * *******************************************************/


using UnityEngine;
using System.Collections;
using System;
using Gamelogic;
using Gamelogic.Extensions;
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

    public static ObservedValue<List<item>> items;

	// Use this for initialization
    void Awake() {
        if (items == null)
            items = new ObservedValue<List<item>>(new List<item>());
    }

	void Start () {
        
	}

    public static void addItem(item it) {
        if(items == null) {
            items = new ObservedValue<List<item>>(new List<item>());
        }
        items.Value.Add(it);
    }

    public static void removeItem(item it) {
        if (items == null)
            return;
        else if (!items.Value.Contains(it)) {
            print("Inventory does not contain " + it.name);
            return;
        }
        items.Value.Remove(it);
    }

    public static void use(item it) {
        it.useItem();
    }
}
