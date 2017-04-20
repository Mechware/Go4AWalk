using UnityEngine;
using System.Collections;
using System;

public class VolcanoScript : MonoBehaviour {
    public bool blowLoad = false;
    Texture2D texture;
    Sprite oldMan;

    public AudioClip explosion;
    public AudioSource sounds;
    // Use this for initialization
    IEnumerator Start() {
        yield return new WaitForSeconds(2);
        blowVolcano();
    }


    void blowVolcano() {
        texture = Resources.Load("Characters/Old Man") as Texture2D; // Old Guy
        oldMan = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        DialoguePopUp.instance.showDialog("What a wonderful day! A day like this makes you feel like nothing could go wrong!", "Old Man", oldMan, () => {
            
            GetComponent<Animator>().SetBool("blowLoad", true);
            StartCoroutine(waitThenStartNextPopUp());
        });
    }

    private IEnumerator waitThenStartNextPopUp() {
        yield return new WaitForSeconds(1.75f);
        sounds.clip = explosion;
        sounds.Play();
        yield return new WaitForSeconds(4.25f);
        aftermath();
    }

    void aftermath() {
        texture = Resources.Load("Characters/Old Man") as Texture2D; // Old Guy
        oldMan = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        texture = Resources.Load("Characters/QuestionMark") as Texture2D; // You
        Sprite you = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        DialoguePopUp.instance.showDialog("Wow! Did you see that? Mt. Flume just blew its top! And it looks like something flew out of it too!", "Old Man", oldMan, () => {
            DialoguePopUp.instance.showDialog("Yeah! That thing that just flew out looked cool! I'm going to go fight it!", "You", you, () => {
                UnityEngine.SceneManagement.SceneManager.LoadScene("WalkingScreen");
            });
        });
    }
}
