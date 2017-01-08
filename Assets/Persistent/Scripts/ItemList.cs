using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemList {

    public const string HEALTH_POTION = "Health Potion";
    public const string CRIT_POTION = "Crit Potion";
    public const string MUNNY_POUCH = "Munny Pouch";
	public const string ATTACK_POTION = "Attack Potion";

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

        Texture2D texture = Resources.Load("Item Sprites/Health Potion") as Texture2D;
        healthPotion.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

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

        texture = Resources.Load("Item Sprites/Crit Potion") as Texture2D;
        critPotion.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        critPotion.useItem += () => {
            if (Player.crit.Value + 10 >= 100)
                Player.crit.Value = 100;
            else {
                Player.crit.Value += 10;
            }
            return true;
        };

        itemMasterList[CRIT_POTION] = critPotion;

        item munnyPouch = new item(name: "Munny Pouch",
            description: "$",
            price: 50,
            attributeValue: 50,
            otherInfo: null,
            type: itemType.Potion,
            useItem: null,
            icon: null);

        munnyPouch.useItem += () => {
            Player.giveGold(50);
            return true;
        };

        itemMasterList[MUNNY_POUCH] = munnyPouch;

		item attackPotion = new item (name: "Attack Potion", 
			description: "raises attack for a short amount of time",
			price: 100,
			attributeValue: 50,
			otherInfo: null,
			type: itemType.Potion,
			useItem: null,
			icon: null);

		texture = Resources.Load("Item Sprites/Crit Potion") as Texture2D;
		attackPotion.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

		attackPotion.useItem += () => {
			Player.giveAttack (50, 10);
			return true;
		};

		itemMasterList [ATTACK_POTION] = attackPotion;
    }
}
