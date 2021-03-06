﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class DamageIndicator :MonoBehaviour {
    public int value;
    
    public float xForcePeakMagnitude = 200;
    public float yForcePeakMagnitude = 200;
    public float xStartPosition = 0;
    public float yStartPosition = 0;

    public float fadeTime = 1f;
    public float startTime;

    private Rigidbody2D rigid;
    private float xforce;
    private float yforce;

    private AudioSource sliceSound;


    // Get damage from damage class
    //damage = damagefromotherplace
    // Use this for initialization
    void Start () {
        damageIdicator();
    }

    public void setText(string text) {
        GetComponent<TextMesh>().text = text;       
    }

    public void damageIdicator() {
        transform.position = new Vector3(xStartPosition, yStartPosition, 0);

        xforce = Random.Range(-1 * xForcePeakMagnitude, xForcePeakMagnitude);
        yforce = Random.Range(yForcePeakMagnitude / 4, yForcePeakMagnitude);

        rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(xforce, yforce));

        // plays sound effect that gets higher pitch as the players crit increases
        sliceSound = GetComponent<AudioSource>();
        sliceSound.pitch = 1f + (((float)Player.getCrit()) / 200f);
        sliceSound.Play();

        StartCoroutine(fade());
    }
   
    IEnumerator fade() {
        yield return null;
        float yVelocity = rigid.velocity.y;
        while (yVelocity > 0) {
            yield return null;
            yVelocity = rigid.velocity.y;
        }

        yield return FadingUtils.fadeTextMesh(GetComponent<TextMesh>(), 1, 1, 0);

        Destroy(this.gameObject);

    }
}
