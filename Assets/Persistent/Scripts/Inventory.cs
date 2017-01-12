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
    // Method to call to use item. 
    // Returns whether or not the item should be removed
    public Func<bool> useItem;
    // Item's icon
    public Sprite icon;
    // Stats of weapons
    public int baseAttack;
    public float attackModifier;
    public float critModifier;

    public item(string name, string description, int price, int attributeValue, object otherInfo, itemType type, Func<bool> useItem, Sprite icon, int baseAttack, float attackModifier, float critModifier) {
        this.name = name;
        this.description = description;
        this.price = price;
        this.attributeValue = attributeValue;
        this.otherInfo = otherInfo;
        this.type = type;
        this.useItem = useItem;
        this.icon = icon;
        this.baseAttack = baseAttack; ;
        this.attackModifier = attackModifier;
        this.critModifier = critModifier;
    }
}


public class Inventory : MonoBehaviour {

    
    public const int INVENTORY_SIZE = 6;
    public static Action onValueChanged;
    private static item[] items;

    // Use this for initialization
    void Awake() {
        initalizeInventory();
        onValueChanged = null;
    }

    static void initalizeInventory() {
        if (items != null)
            return;
        items = new item[INVENTORY_SIZE];
        for(int i = 0 ; i < INVENTORY_SIZE ; i++) {
            items[i] = ItemList.noItem;
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
            if (items[i].Equals(ItemList.noItem)) {
                addItem(it, i);
                return;
            }
        }
    }

    public static void removeItem(int pos) {

        initalizeInventory();

        if (items[pos].Equals(ItemList.noItem)) {
            print("Inventory does not contain an item at position: " + pos);
            return;
        }
        
        items[pos] = ItemList.noItem;
        onValueChanged.Invoke();
    }

    private static void Items_OnValueChange() {
        print("Okay now it's being thronw?");
    }

    public static void use(item it) {
        it.useItem();
    }

    public static void use(int number) {
        bool shouldRemoveItem = items[number].useItem();
        if(shouldRemoveItem)
            removeItem(number);
    }

    // Non-static method for inventory buttons to use
    public void useItem(int number) {
        use(number);
    }

    public static item getItem(int pos) {
        return items[pos];
    }
}
