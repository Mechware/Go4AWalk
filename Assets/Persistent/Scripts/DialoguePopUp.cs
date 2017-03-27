using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


/***** EXAMPLE USAGE:
 * 
 * 
    DialoguePopUp.instance.showDialog(
        dialog: "Hello! This is a h*ckin test", 
        buttonsText: new string[] { "Really??", "Cool!", "Ugh, lame" },
        buttonsAction: new Action[] {
            () => {
                DialoguePopUp.instance.showDialog(
                    dialog: "Test :D", 
                    buttonsText: new string[] {"WOO!" }, 
                    buttonsAction: new Action[] { ()=> { } }, 
                    characterName: "Moik", 
                    characterSprite: characterSprite);
            },
            () => { },
            () => { } }, 
        characterName: "Moik", 
        characterSprite: characterSprite, 
        closePopUpWhenHit: new bool[] { false, true, true });
 * 
 */

public class DialoguePopUp : MonoBehaviour {

    public static DialoguePopUp instance;
    public Image characterSpriteHolder;
    public GameObject[] dialogOptions;
    public GameObject dialogPanel;
    public Text dialogText, characterNameText;

    private UnityEngine.Events.UnityAction closePopUp;

    void Awake() {
        instance = this;
        closePopUp = new UnityEngine.Events.UnityAction(() => {
            dialogPanel.SetActive(false);
        });
        
    }

    // Use this for initialization
    void Start() {
        resetButtons();
    }

    // Update is called once per frame
    void Update() {

    }

    public void showDialog(string dialog, string[] buttonsText, Action[] buttonsAction, 
        string characterName, Sprite characterSprite, bool[] closePopUpWhenHit = null) {
        
        int i;

        // Make sure input is valid
        UnityEngine.Assertions.Assert.IsFalse(
            buttonsText.Length == 0,
            "No button titles provided");
        UnityEngine.Assertions.Assert.IsFalse(
            buttonsText.Length != buttonsAction.Length,
            "Button and action array length must be the same");

        if (closePopUpWhenHit == null) {
            closePopUpWhenHit = new bool[buttonsText.Length];
            for (i = 0 ; i < closePopUpWhenHit.Length ; i++) {
                closePopUpWhenHit[i] = true;
            }
        }

        // Reset the buttons of the pop up
        resetButtons();

        // Set the dialog text
        dialogText.text = dialog;

        // Set character name
        characterNameText.text = characterName;

        // Set buttons
        for (i = 0 ; i < buttonsText.Length ; i++) {
            dialogOptions[i].SetActive(true);
            dialogOptions[i].GetComponentInChildren<Text>().text = buttonsText[i];
            UnityEngine.Events.UnityAction action = new UnityEngine.Events.UnityAction(buttonsAction[i]);
            if (closePopUpWhenHit[i])
                dialogOptions[i].GetComponent<Button>().onClick.AddListener(closePopUp);
            dialogOptions[i].GetComponent<Button>().onClick.AddListener(action);
        }

        // Set unused buttons invisible
        while (i < dialogOptions.Length) {
            dialogOptions[i].SetActive(false);
            i++;
        }

        // Set sprite
        characterSpriteHolder.sprite = characterSprite;

        dialogPanel.SetActive(true);
    }

    void resetButtons() {
        for (int i = 0 ; i < dialogOptions.Length ; i++) {
            dialogOptions[i].GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}
