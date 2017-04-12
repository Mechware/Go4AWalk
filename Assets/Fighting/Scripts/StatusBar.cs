using UnityEngine;
using System.Collections;

public class StatusBar : MonoBehaviour {

    private float healthpercent;
    private Vector3 originalScale;

	// Use this for initialization
	void Awake () {
        originalScale = transform.localScale;
	}

    public void updateBar(int max, int current) {
        UnityEngine.Assertions.Assert.IsTrue(current <= max, "Max is: " + max + " while current is: " + current);
        healthpercent = ((float) current) / ((float) max);
        healthpercent = healthpercent > 1 ? 1 : healthpercent;
        transform.localScale = new Vector3(healthpercent*originalScale.x, originalScale.y, originalScale.z);
    } 
}
