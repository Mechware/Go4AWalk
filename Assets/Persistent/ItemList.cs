using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemList {

    private static IDictionary<string, item> _itemMasterList = null;
    public static IDictionary<string, item> itemMasterList {
        get {
            if (_itemMasterList == null) {
                initialize();
            }
            return _itemMasterList;
        }
    }
    public static bool initialized = false;


    public static void initialize() {
        if (initialized) return;

        _itemMasterList = new Dictionary<string, item>();

        // Health Potion
        item healthPotion = new item(name: "Health Potion", 
            description: "Used to regain health",
            price: 10, 
            attributeValue: 10, 
            otherInfo: null, 
            type: itemType.Potion, 
            useItem: null, 
            icon: null);

        healthPotion.useItem += () => {
            Player.giveHealth(10);
            return true;
        };
        itemMasterList["Health Potion"] = healthPotion;
    }
}
