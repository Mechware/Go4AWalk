using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class DamageIndicator : MonoBehaviour {
    public int damage;
    public float xforce;
    public float yforce;
    public float fadeTime = 1f;
    public float startTime;

    // Get damage from damage class
    //damage = damagefromotherplace
	// Use this for initialization
	void Start () {
        xforce = Random.Range(-400f, 400f);
        yforce = Random.Range(900f, 1100f);
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(xforce, yforce));

        startTime = Time.time;
    }

    public void setText(string text) {
        GetComponent<Text>().text = text;
    }
   
    bool hasFaded = false;
	// Update is called once per frame
	void Update () {
        
        if (Time.time - startTime > 0.1f && hasFaded == false)
        {
            float yVelocity = this.GetComponent<Rigidbody2D>().velocity.y;
            //fade once text is falling
            if (yVelocity < 0)
            {
                GetComponent<Text>().CrossFadeAlpha(0, fadeTime, false);
                Destroy(this.gameObject, fadeTime);
                hasFaded = true;
            }


        }


    }
}
