using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class DamageIndicator : MonoBehaviour {
    public int damage;
    
    public float xForcePeakMagnitude = 200;
    public float yForcePeakMagnitude = 200;
    public float xStartPosition = 0;
    public float yStartPosition = 0;

    public float fadeTime = 1f;
    public float startTime;

    private Rigidbody2D rigid;
    private float xforce;
    private float yforce;

    // Get damage from damage class
    //damage = damagefromotherplace
    // Use this for initialization
    void Start () {
        transform.position = new Vector3(xStartPosition, yStartPosition, 0);

        xforce = Random.Range(-1*xForcePeakMagnitude, xForcePeakMagnitude);
        yforce = Random.Range(yForcePeakMagnitude/4, yForcePeakMagnitude);
        
        rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(xforce, yforce));

        StartCoroutine(fade());
    }

    public void setText(string text) {
        GetComponent<TextMesh>().text = text;       
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
