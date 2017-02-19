using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// All the different item types that exist in our game.
/// </summary>
public enum itemType
{
    Potion,
    Poison,
    Equipment,
    Weapon,
}
public class ItemList {



    public const string HEALTH_POTION = "Health Potion";
    public const string CRIT_POTION = "Crit Potion";
    public const string MUNNY_POUCH = "Munny Pouch";
	public const string ATTACK_POTION = "Attack Potion";
    public const string BRONZE_SWORD = "Bronze Sword";

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
    public static item noItem = new item("", "", 0, 0, null, itemType.Equipment, () => { return false; }, null, 0, 1, 1);

    public static void initialize() {

        if (initialized) return;

        _itemMasterList = new Dictionary<string, item>();

        // Health Potion
        item healthPotion = new item(name: "Health Potion", 
            description: "Used to regain health",
            price: 10, 
            attributeValue: 35, 
            otherInfo: null, 
            type: itemType.Potion, 
            useItem: null, 
            icon: null,
            baseAttack: 0,
            attackModifier: 1,
            critModifier: 1);

        Texture2D texture = Resources.Load("Item Sprites/Health Potion") as Texture2D;
        healthPotion.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        healthPotion.useItem += () => {
            Player.giveHealth(35);
            return true;
        };
        itemMasterList[HEALTH_POTION] = healthPotion;
// ***
        item critPotion = new item(name: "Crit Potion",
            description: "Used to gain crit points",
            price: 10,
            attributeValue: 10,
            otherInfo: null,
            type: itemType.Potion,
            useItem: null,
            icon: null,
            baseAttack: 0,
            attackModifier: 1,
            critModifier: 1);

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
// ***
        item munnyPouch = new item(name: "Munny Pouch",
            description: "$",
            price: 50,
            attributeValue: 50,
            otherInfo: null,
            type: itemType.Potion,
            useItem: null,
            icon: null,
            baseAttack: 0,
            attackModifier: 1,
            critModifier: 1);

        munnyPouch.useItem += () => {
            Player.giveGold(50);
            return true;
        };

        itemMasterList[MUNNY_POUCH] = munnyPouch;
// ***
        item attackPotion = new item (name: "Attack Potion", 
			description: "raises attack for a short amount of time",
			price: 100,
			attributeValue: 50,
			otherInfo: null,
			type: itemType.Potion,
			useItem: null,
			icon: null,
            baseAttack: 0,
            attackModifier: 1,
            critModifier: 1);

		texture = Resources.Load("Item Sprites/Attack Potion") as Texture2D;
		attackPotion.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

		attackPotion.useItem += () => {
            Player.instance.CreateBuff("attack", 0.25f, 10, Player.instance.gameObject);
			return true;
		};

		itemMasterList [ATTACK_POTION] = attackPotion;
// ***
        item bronzeSword = new item(name: "Bronze Sword",
            description: "A trusty sword made of bronze",
            price: 100,
            attributeValue: 0,
            otherInfo: null,
            type: itemType.Weapon,
            useItem: null,
            icon: null,
            baseAttack: 10,
            attackModifier: 1,
            critModifier: 1);

        texture = Resources.Load("Item Sprites/Bronze Sword") as Texture2D;
        bronzeSword.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        bronzeSword.useItem += () => {
            Player.equipWeapon(bronzeSword);
            return true;
        };

        itemMasterList[BRONZE_SWORD] = bronzeSword;
// ***
        item bronzeSaber = new item(name: "Bronze Saber",
                    description: "A duelists' saber made of bronze, deals less damage per strike but allows for more critical hits.",
                    price: 100,
                    attributeValue: 0,
                    otherInfo: null,
                    type: itemType.Weapon,
                    useItem: null,
                    icon: null,
                    baseAttack: 10,
                    attackModifier: 0.9f,
                    critModifier: 1.05f);

        texture = Resources.Load("Item Sprites/Bronze Saber") as Texture2D;
        bronzeSaber.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        bronzeSaber.useItem += () => {
            Player.equipWeapon(bronzeSaber);
            return true;
        };

        itemMasterList[BRONZE_SWORD] = bronzeSaber;
    }
}
