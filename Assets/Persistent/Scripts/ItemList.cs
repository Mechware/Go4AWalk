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
    Armor
}
public class ItemList {

    
    public const string HEALTH_POTION = "Health Potion";
    public const string CRIT_POTION = "Crit Potion";
    public const string MUNNY_POUCH = "Munny Pouch";
	public const string ATTACK_POTION = "Attack Potion";

    public const string BRONZE_SWORD = "Bronze Sword";
    public const string BRONZE_SABER = "Bronze Saber";
    public const string WOOD_SWORD = "Wood Sword";
    public const string IRON_SWORD = "Iron Sword";
    public const string STEEL_SWORD = "Steel Sword";
    public const string ORICH_SWORD = "Oricalcum Sword";
    public const string SAND_SWORD = "Sand Steel Sword";
    public const string JADE_SWORD = "Jade Sword";
    public const string OBSIDIAN_SWORD = "Macuahuitl";
    public const string STEEL_RAPIER = "Steel Rapier";
    public const string MYTHRIL_SWORD = "Mythril Sword";

    public const string BRONZE_ARMOR = "Bronze Armor";
    public const string IRON_ARMOR = "Iron Armor";
    public const string STEEL_ARMOR = "Steel Armor";
    public const string ORICH_ARMOR = "Orichalcum Armor";
    public const string SAND_ARMOR = "Sand Steel Armor";
    public const string MYTHRIL_ARMOR = "Mythril Armor";
    public const string JADE_ARMOR = "Jade Armor";
    public const string OBSIDIAN_ARMOR = "Obsidian Armor";

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
    public static item noItem = new item("", "", 0, 0, null, itemType.Equipment, () => { return false; }, null, 0, 0, 1, 1);

    public static void initialize() {

        if (initialized) return;

        _itemMasterList = new Dictionary<string, item>();

        #region Consumables
        // Health Potion
        item healthPotion = new item(name: HEALTH_POTION,
            description: "Used to regain health.",
            price: 10,
            attributeValue: 35,
            otherInfo: null,
            type: itemType.Potion,
            useItem: null,
            icon: null,
            spawnRate: 60,
            baseAttack: 0,
            attackModifier: 1,
            critModifier: 1);

        Texture2D texture = Resources.Load("Item Sprites/Health Potion") as Texture2D;
        healthPotion.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        healthPotion.useItem += () => {
            Player.giveHealth(Mathf.RoundToInt((float)Player.getMaxHealth()*0.35f));
            BuffManager.instance.healthParticlesPlay();
            return true;
        };
        itemMasterList[HEALTH_POTION] = healthPotion;
// ***
        item critPotion = new item(name: CRIT_POTION,
            description: "When used the critical bar will not reset for a short amount of time.",
            price: 10,
            attributeValue: 10,
            otherInfo: null,
            type: itemType.Potion,
            useItem: null,
            icon: null,
            spawnRate: 40,
            baseAttack: 0,
            attackModifier: 1,
            critModifier: 1);

        texture = Resources.Load("Item Sprites/Crit Potion") as Texture2D;
        critPotion.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        critPotion.useItem += () => {
            BuffManager.instance.CreateBuff("PlayerCritBoost", BuffManager.BuffType.crit, 1f, 5, Player.instance.gameObject);
            return true;
        };

        itemMasterList[CRIT_POTION] = critPotion;
        // ***
        item munnyPouch = new item(name: MUNNY_POUCH,
            description: "$",
            price: 50,
            attributeValue: 50,
            otherInfo: null,
            type: itemType.Potion,
            useItem: null,
            icon: null,
            spawnRate: 30,
            baseAttack: 0,
            attackModifier: 1,
            critModifier: 1);

        munnyPouch.useItem += () => {
            Player.giveGold(50);
            return true;
        };

        itemMasterList[MUNNY_POUCH] = munnyPouch;
// ***
        item attackPotion = new item (name: ATTACK_POTION, 
			description: "Raises attack for a short amount of time.",
			price: 100,
			attributeValue: 50,
			otherInfo: null,
			type: itemType.Potion,
			useItem: null,
			icon: null,
            spawnRate: 15,
            baseAttack: 0,
            attackModifier: 1,
            critModifier: 1);

		texture = Resources.Load("Item Sprites/Attack Potion") as Texture2D;
		attackPotion.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

		attackPotion.useItem += () => {
            BuffManager.instance.CreateBuff("PlayerAttackBoost",BuffManager.BuffType.attack, 0.25f, 10, Player.instance.gameObject);
			return true;        
        };
        itemMasterList[ATTACK_POTION] = attackPotion;

        #endregion

        #region Weapons


        // ***
        item bronzeSword = new item(name: BRONZE_SWORD,
            description: "A trusty sword made of bronze",
            price: 100,
            attributeValue: 0,
            otherInfo: null,
            type: itemType.Weapon,
            useItem: null,
            icon: null,
            spawnRate: 0,
            baseAttack: 10,
            attackModifier: 1,
            critModifier: 1);

        texture = Resources.Load("Item Sprites/Sword-Bronze") as Texture2D;
        bronzeSword.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        bronzeSword.useItem += () => {
            Player.instance.equipWeapon(bronzeSword);
            return true;
        };

        itemMasterList[BRONZE_SWORD] = bronzeSword;
// ***
        item bronzeSaber = new item(name: BRONZE_SABER,
                    description: "A duelists' saber made of bronze, deals less damage per strike but allows for more critical hits.",
                    price: 100,
                    attributeValue: 0,
                    otherInfo: null,
                    type: itemType.Weapon,
                    useItem: null,
                    icon: null,
                    spawnRate: 0,
                    baseAttack: 10,
                    attackModifier: 0.9f,
                    critModifier: 1.05f);

        texture = Resources.Load("Item Sprites/Saber-Bronze") as Texture2D;
        bronzeSaber.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        bronzeSaber.useItem += () => {
            Player.instance.equipWeapon(bronzeSaber);
            return true;
        };

        itemMasterList[BRONZE_SABER] = bronzeSaber;
        // ***
        item woodSword = new item(name: WOOD_SWORD,
                    description: "A sword made of wood. Okay, its just a stick shaped like a sword. Okay, its just a stick.",
                    price: 100,
                    attributeValue: 0,
                    otherInfo: null,
                    type: itemType.Weapon,
                    useItem: null,
                    icon: null,
                    spawnRate: 0,
                    baseAttack: 1,
                    attackModifier: 1f,
                    critModifier: 1f);

        texture = Resources.Load("Item Sprites/Sword-Wood") as Texture2D;
        woodSword.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        woodSword.useItem += () => {
            Player.instance.equipWeapon(woodSword);
            return true;
        };

        itemMasterList[WOOD_SWORD] = woodSword;

        // ***
        item ironSword = new item(name: IRON_SWORD,
                    description: "A sturdy sword forged from iron.",
                    price: 100,
                    attributeValue: 0,
                    otherInfo: null,
                    type: itemType.Weapon,
                    useItem: null,
                    icon: null,
                    spawnRate: 0,
                    baseAttack: 20,
                    attackModifier: 1f,
                    critModifier: 1f);

        texture = Resources.Load("Item Sprites/Sword-Iron") as Texture2D;
        ironSword.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        ironSword.useItem += () => {
            Player.instance.equipWeapon(ironSword);
            return true;
        };

        itemMasterList[IRON_SWORD] = ironSword;

        // ***
        item steelSword = new item(name: STEEL_SWORD,
                    description: "A quality blade made out of steel. #Carbon",
                    price: 100,
                    attributeValue: 0,
                    otherInfo: null,
                    type: itemType.Weapon,
                    useItem: null,
                    icon: null,
                    spawnRate: 0,
                    baseAttack: 30,
                    attackModifier: 1f,
                    critModifier: 1f);

        texture = Resources.Load("Item Sprites/Sword-Steel") as Texture2D;
        steelSword.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        steelSword.useItem += () => {
            Player.instance.equipWeapon(steelSword);
            return true;
        };

        itemMasterList[STEEL_SWORD] = steelSword;

        // ***
        item orichalcumSword = new item(name: ORICH_SWORD,
                    description: "A sword made from a mysterious ancient alloy, and apparently modern metallurgy can't compete. Huh.",
                    price: 100,
                    attributeValue: 0,
                    otherInfo: null,
                    type: itemType.Weapon,
                    useItem: null,
                    icon: null,
                    spawnRate: 0,
                    baseAttack: 50,
                    attackModifier: 1f,
                    critModifier: 1f);

        texture = Resources.Load("Item Sprites/Sword-Orichalcum") as Texture2D;
        orichalcumSword.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        orichalcumSword.useItem += () => {
            Player.instance.equipWeapon(orichalcumSword);
            return true;
        };

        itemMasterList[ORICH_SWORD] = orichalcumSword;

        // ***
        item sandSteelSword = new item(name: SAND_SWORD,
                    description: "A sword forged from legendary sand steel. The secret to its strength is heavily guarded. Its probably love.",
                    price: 100,
                    attributeValue: 0,
                    otherInfo: null,
                    type: itemType.Weapon,
                    useItem: null,
                    icon: null,
                    spawnRate: 0,
                    baseAttack: 70,
                    attackModifier: 1f,
                    critModifier: 1f);

        texture = Resources.Load("Item Sprites/Sword-SandSteel") as Texture2D;
        sandSteelSword.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        sandSteelSword.useItem += () => {
            Player.instance.equipWeapon(sandSteelSword);
            return true;
        };

        itemMasterList[SAND_SWORD] = sandSteelSword;

        // ***
        item jadeSword = new item(name: JADE_SWORD,
                    description: "A beautiful ornamental sword made out of the jade. Wait why are you fighting with this?",
                    price: 100,
                    attributeValue: 0,
                    otherInfo: null,
                    type: itemType.Weapon,
                    useItem: null,
                    icon: null,
                    spawnRate: 0,
                    baseAttack: 85,
                    attackModifier: 1f,
                    critModifier: 1f);

        texture = Resources.Load("Item Sprites/Sword-Jade") as Texture2D;
        jadeSword.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        jadeSword.useItem += () => {
            Player.instance.equipWeapon(jadeSword);
            return true;
        };

        itemMasterList[JADE_SWORD] = jadeSword;

        // ***
        item obsidianSword = new item(name: OBSIDIAN_SWORD,
                    description: "A wooden club with obsidian blades. Did you know obsidian blades can be as sharp as 3 nanometers? Well now you do.",
                    price: 100,
                    attributeValue: 0,
                    otherInfo: null,
                    type: itemType.Weapon,
                    useItem: null,
                    icon: null,
                    spawnRate: 0,
                    baseAttack: 125,
                    attackModifier: 1f,
                    critModifier: 1f);

        texture = Resources.Load("Item Sprites/Sword-Obsidian") as Texture2D;
        obsidianSword.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        obsidianSword.useItem += () => {
            Player.instance.equipWeapon(obsidianSword);
            return true;
        };

        itemMasterList[OBSIDIAN_SWORD] = obsidianSword;

        // ***
        item steelRapier = new item(name: STEEL_RAPIER,
                    description: "A delicate blade that sacrifices damage for being able to find the enemies' weak spots.",
                    price: 100,
                    attributeValue: 0,
                    otherInfo: null,
                    type: itemType.Weapon,
                    useItem: null,
                    icon: null,
                    spawnRate: 0,
                    baseAttack: 30,
                    attackModifier: 0.6f,
                    critModifier: 1.1f);

        texture = Resources.Load("Item Sprites/Rapier") as Texture2D;
        steelRapier.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        steelRapier.useItem += () => {
            Player.instance.equipWeapon(steelRapier);
            return true;
        };

        itemMasterList[STEEL_RAPIER] = steelRapier;

        // ***
        item mythrilSword = new item(name: MYTHRIL_SWORD,
                    description: "A magnificent sword made from the mythical metal mythril. Many muse the metal is magical, maybe.",
                    price: 100,
                    attributeValue: 0,
                    otherInfo: null,
                    type: itemType.Weapon,
                    useItem: null,
                    icon: null,
                    spawnRate: 0,
                    baseAttack: 100,
                    attackModifier: 1f,
                    critModifier: 1f);

        texture = Resources.Load("Item Sprites/Sword-Mythril") as Texture2D;
        mythrilSword.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        mythrilSword.useItem += () => {
            Player.instance.equipWeapon(mythrilSword);
            return true;
        };

        itemMasterList[MYTHRIL_SWORD] = mythrilSword;

        #endregion

        #region Armor
        // ***
        item bronzeArmor = new item(name: BRONZE_ARMOR,
            description: "Armor made from bronze.",
            price: 100,
            attributeValue: 0.95f,
            otherInfo: null,
            type: itemType.Armor,
            useItem: null,
            icon: null,
            spawnRate: 0,
            baseAttack: 0,
            attackModifier: 1,
            critModifier: 1);

        texture = Resources.Load("Item Sprites/Armor-Bronze") as Texture2D;
        bronzeArmor.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        bronzeArmor.useItem += () => {
            Player.instance.equipArmor(bronzeArmor);
            return true;
        };

        itemMasterList[BRONZE_ARMOR] = bronzeArmor;

        // ***
        item ironArmor = new item(name: IRON_ARMOR,
            description: "Armor made from iron.",
            price: 100,
            attributeValue: 0.9f,
            otherInfo: null,
            type: itemType.Armor,
            useItem: null,
            icon: null,
            spawnRate: 0,
            baseAttack: 0,
            attackModifier: 1,
            critModifier: 1);

        texture = Resources.Load("Item Sprites/Armor-Iron") as Texture2D;
        ironArmor.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        ironArmor.useItem += () => {
            Player.instance.equipArmor(ironArmor);
            return true;
        };

        itemMasterList[IRON_ARMOR] = ironArmor;

        // ***
        item steelArmor = new item(name: STEEL_ARMOR,
            description: "Armor made from steel.",
            price: 100,
            attributeValue: 0.85f,
            otherInfo: null,
            type: itemType.Armor,
            useItem: null,
            icon: null,
            spawnRate: 0,
            baseAttack: 0,
            attackModifier: 1,
            critModifier: 1);

        texture = Resources.Load("Item Sprites/Armor-Steel") as Texture2D;
        steelArmor.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        steelArmor.useItem += () => {
            Player.instance.equipArmor(steelArmor);
            return true;
        };

        itemMasterList[STEEL_ARMOR] = steelArmor;

        // ***
        item sandSteelArmor = new item(name: SAND_ARMOR,
            description: "Armor made from sand steel.",
            price: 100,
            attributeValue: 0.75f,
            otherInfo: null,
            type: itemType.Armor,
            useItem: null,
            icon: null,
            spawnRate: 0,
            baseAttack: 0,
            attackModifier: 1,
            critModifier: 1);

        texture = Resources.Load("Item Sprites/Armor-SandSteel") as Texture2D;
        sandSteelArmor.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        sandSteelArmor.useItem += () => {
            Player.instance.equipArmor(sandSteelArmor);
            return true;
        };

        itemMasterList[SAND_ARMOR] = sandSteelArmor;

        // ***
        item orchalcumArmor = new item(name: ORICH_ARMOR,
            description: "Armor made from orichalcum.",
            price: 100,
            attributeValue: 0.80f,
            otherInfo: null,
            type: itemType.Armor,
            useItem: null,
            icon: null,
            spawnRate: 0,
            baseAttack: 0,
            attackModifier: 1,
            critModifier: 1);

        texture = Resources.Load("Item Sprites/Armor-Orichalcum") as Texture2D;
        orchalcumArmor.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        orchalcumArmor.useItem += () => {
            Player.instance.equipArmor(orchalcumArmor);
            return true;
        };

        itemMasterList[ORICH_ARMOR] = orchalcumArmor;

        // ***
        item mythrilArmor = new item(name: MYTHRIL_ARMOR,
            description: "Armor made from mythril.",
            price: 100,
            attributeValue: 0.70f,
            otherInfo: null,
            type: itemType.Armor,
            useItem: null,
            icon: null,
            spawnRate: 0,
            baseAttack: 0,
            attackModifier: 1,
            critModifier: 1);

        texture = Resources.Load("Item Sprites/Armor-Mythril") as Texture2D;
        mythrilArmor.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        mythrilArmor.useItem += () => {
            Player.instance.equipArmor(mythrilArmor);
            return true;
        };

        itemMasterList[MYTHRIL_ARMOR] = mythrilArmor;

        // ***
        item jadeArmor = new item(name: JADE_ARMOR,
            description: "Armor made from jade.",
            price: 100,
            attributeValue: 0.65f,
            otherInfo: null,
            type: itemType.Armor,
            useItem: null,
            icon: null,
            spawnRate: 0,
            baseAttack: 0,
            attackModifier: 1,
            critModifier: 1);

        texture = Resources.Load("Item Sprites/Armor-Jade") as Texture2D;
        jadeArmor.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        jadeArmor.useItem += () => {
            Player.instance.equipArmor(jadeArmor);
            return true;
        };

        itemMasterList[JADE_ARMOR] = jadeArmor;

        // ***
        item obsidianArmor = new item(name: OBSIDIAN_ARMOR,
            description: "Armor made from obsidian.",
            price: 100,
            attributeValue: 0.60f,
            otherInfo: null,
            type: itemType.Armor,
            useItem: null,
            icon: null,
            spawnRate: 0,
            baseAttack: 0,
            attackModifier: 1,
            critModifier: 1);

        texture = Resources.Load("Item Sprites/Armor-Obsidian") as Texture2D;
        obsidianArmor.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        obsidianArmor.useItem += () => {
            Player.instance.equipArmor(obsidianArmor);
            return true;
        };

        itemMasterList[OBSIDIAN_ARMOR] = obsidianArmor;
        #endregion
    }

}
