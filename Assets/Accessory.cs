using UnityEngine;
using System.Collections;
using System;

public class Accessory {

    private Action unequipCallback;
    private Action equipCallback;

    public Accessory(Action accessoryUnequipAction, Action equipAction) {
        unequipCallback = accessoryUnequipAction;
        equipCallback = equipAction;
    }

    public Action getUnequipCallback() {
        return unequipCallback;
    }

    public Action getEquipCallback() {
        return equipCallback;
    }

}
