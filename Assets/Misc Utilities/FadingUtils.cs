using UnityEngine;
using System.Collections;

public class FadingUtils : MonoBehaviour {

    public static IEnumerator fadeSpriteRenderer(SpriteRenderer sprite, float duration, float beginningAlpha, float endingAlpha) {
        float incrementsPerSecond = (endingAlpha - beginningAlpha) / duration;
        float timeElapsed = 0;
        Color spriteColor = sprite.color;
        spriteColor.a = beginningAlpha;

        while (timeElapsed < duration) {
            float increment = incrementsPerSecond * Time.deltaTime;
            spriteColor.a += increment;
            sprite.color = spriteColor;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    public static IEnumerator fadeTextMesh(TextMesh text, float duration, float beginningAlpha, float endingAlpha) {
        float incrementsPerSecond = (endingAlpha - beginningAlpha) / duration;
        float timeElapsed = 0;
        Color textColor = text.color;
        textColor.a = beginningAlpha;

        while (timeElapsed < duration) {
            float increment = incrementsPerSecond * Time.deltaTime;
            textColor.a += increment;
            text.color = textColor;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
