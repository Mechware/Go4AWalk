using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemList {

    public static IDictionary<string, item> itemMasterList;
    public static bool initialized = false;


    public static void initialize() {
        if (initialized) return;

        itemMasterList = new Dictionary<string, item>();

        // Health Potion
        item healthPotion = new item("Health Potion", "Used to regain health", 10, 10, null, itemType.Potion, null, null);
        healthPotion.useItem += () => {
            Player.giveHealth(10);
        };
        itemMasterList["Health Potion"] = healthPotion;
    }
}
