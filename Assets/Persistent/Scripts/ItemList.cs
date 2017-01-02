using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemList {

    public const string HEALTH_POTION = "Health Potion";
    public const string CRIT_POTION = "Crit Potion";

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
        itemMasterList[HEALTH_POTION] = healthPotion;

        item critPotion = new item(name: "Crit Potion",
            description: "Used to gain crit points",
            price: 10,
            attributeValue: 10,
            otherInfo: null,
            type: itemType.Potion,
            useItem: null,
            icon: null);

        critPotion.useItem += () => {
            if (Player.crit.Value + 10 >= 100)
                Player.crit.Value = 100;
            else {
                Player.crit.Value += 10;
            }
            return true;
        };

        itemMasterList[CRIT_POTION] = critPotion;
    }
}
