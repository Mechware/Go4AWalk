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
	
	// Update is called once per frame
	void Update () {
	
	}

    public void hitPlayer(int amount) {
        

        Player.health -= amount;
        if (Player.health <= 0) {
            Player.health = 0;
            Player.died = true;
            Questing.endQuest(false);
        }
        updateHealthBar();
    }

    // Used for update the health bar
    void updateHealthBar() {
        healthBar.updateBar(Player.maxHealth, Player.health);
    }

    // Used for updating the critical bar
    void updateCritBar() {
        critBar.updateBar(100, Player.getCrit());
    }
}
