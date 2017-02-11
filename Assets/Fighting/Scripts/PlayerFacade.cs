/*********************************************************************
 * PlayerFacade.cs
 * This is the class that the Enemy in "Fighting" scene interacts with
 * 
 *********************************************************************/

using UnityEngine;
using System.Collections;

public class PlayerFacade : MonoBehaviour {

    public StatusBar healthBar, critBar;

	// Use this for initialization
	void Start () {
        Player.crit.OnValueChange += updateCritBar;
        updateCritBar();
        updateHealthBar();
	}

    public void hitPlayer(int amount) {   

        Player.damage(amount);
        updateHealthBar();
    }

    // Used for update the health bar
    void updateHealthBar() {
        healthBar.updateBar(Player.getMaxHealth(), Player.getCurrentHealth());
    }

    // Used for updating the critical bar
    void updateCritBar() {
        critBar.updateBar(100, Player.getCrit());
    }
}
