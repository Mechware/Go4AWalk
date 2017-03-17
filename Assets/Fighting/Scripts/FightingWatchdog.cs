using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FightingWatchdog : MonoBehaviour {

    #region endingFights

    public SpriteRenderer background, treasureBackground;
    public TextMesh enemyKilledText;
    public GameObject treasureChest, critBarObject, healthBarObject;


    public void fadeOutStats() {
        foreach (SpriteRenderer sp in critBarObject.GetComponentsInChildren<SpriteRenderer>()) {
            StartCoroutine(FadingUtils.fadeSpriteRenderer(sp, 1, 1, 0));
        }
        foreach (TextMesh tm in critBarObject.GetComponentsInChildren<TextMesh>()) {
            StartCoroutine(FadingUtils.fadeTextMesh(tm, 1, 1, 0));
        }
        foreach (SpriteRenderer sp in healthBarObject.GetComponentsInChildren<SpriteRenderer>()) {
            StartCoroutine(FadingUtils.fadeSpriteRenderer(sp, 1, 1, 0));
        }
        foreach (TextMesh tm in healthBarObject.GetComponentsInChildren<TextMesh>()) {
            StartCoroutine(FadingUtils.fadeTextMesh(tm, 1, 1, 0));
        }
    }

    // Called when the enemy is killed
    public IEnumerator endRegularFight() {
        print("reg fight over");
        fadeOutStats();

        enemyKilledText.gameObject.SetActive(true);
        StartCoroutine(FadingUtils.fadeTextMesh(enemyKilledText, 1, 0, 1));
        yield return new WaitForSeconds(2);
        GameState.loadScene(GameState.scene.WALKING_LEVEL);
    }

    // Called when a boss is killed
    public IEnumerator questFightEnd() {
        print("boss fight over");
        fadeOutStats();

        StartCoroutine(FadingUtils.fadeSpriteRenderer(background, 1, 1, 0));
        StartCoroutine(FadingUtils.fadeSpriteRenderer(treasureBackground, 1, 0, 1));

        yield return new WaitForSeconds(1.2f);

        treasureChest.SetActive(true);
        StartCoroutine(treasureChest.GetComponent<TreasureChest>().openChest());

        Destroy(treasureChest, 5.1f);
        yield return new WaitForSeconds(5.1f);
        
        EnemyWatchdog.isBoss = false;
        Questing.endQuest(true);

    }

    #endregion

    #region updatePlayerStats
    public StatusBar healthBar, critBar;

    // Use this for initialization
    void Start() {
        Player.crit.OnValueChange += updateCritBar;
        Player.health.OnValueChange += updateHealthBar;
        updateCritBar();
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
    #endregion
}
