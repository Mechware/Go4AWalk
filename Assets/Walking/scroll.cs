using UnityEngine;
using System.Collections;

public class scroll : MonoBehaviour {
    public float speed = 0.5f;
    public Renderer scrollRenderer;
	// Use this for initialization
	void Start () {
        scrollRenderer = GetComponent<Renderer>();

    }
	
	// Update is called once per frame
	void Update () {
        Vector2 offset = new Vector2(Time.time * speed, 0);
        scrollRenderer.material.mainTextureOffset = offset;
	}
}
