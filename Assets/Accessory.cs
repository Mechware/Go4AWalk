using UnityEngine;
using System.Collections;
using System;

public class AccessoryUnequip {

    private Action unequipCallback;

    public AccessoryUnequip(Action accessoryUnequipAction) {
        
    }

    public Action getUnequipCallback() {
        return unequipCallback;
    }

}
