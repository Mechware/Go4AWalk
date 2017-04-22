using UnityEngine;
using System.Collections;

public class LevelUpAnimation : MonoBehaviour {

    public GameObject levelUpSprite;
    private static int lastLevel = -1;

	// Use this for initialization
	IEnumerator Start () {
        if (lastLevel == -1) {
            lastLevel = Player.level.Value;
        }
        if (Player.level.Value == lastLevel) {
            yield break;
        }
        
        levelUpSprite.SetActive(true);
        yield return FadingUtils.fadeSpriteRenderer(levelUpSprite.GetComponent<SpriteRenderer>(), 2, 0, 1);
        yield return FadingUtils.fadeSpriteRenderer(levelUpSprite.GetComponent<SpriteRenderer>(), 2, 1, 0);
        levelUpSprite.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
