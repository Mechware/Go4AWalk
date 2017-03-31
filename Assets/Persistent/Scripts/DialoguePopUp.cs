using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;


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

    or

    showDialog(
            dialog: "Hello this is a test, oh yes it is.Hello this is a test, oh yes it is.Hello this is a test, oh yes it is. Hello this is a test, oh yes it is.Hello this is a test, oh yes it is.Hello this is a test, oh yes it is.Hello this is a test, oh yes it is.Hello this is a test, oh yes it is.Hello this is a test, oh yes it is.",
            characterName: "Moik",
            characterSprite: Resources.Load<Sprite>("Item Sprites/Armor-Bronze"),
            functionToCallWhenDialogFinished: new Action(() => { print("Done!"); })
            );
 * 
 */

public class DialoguePopUp : MonoBehaviour {

    public static DialoguePopUp instance;
    public Image characterSpriteHolder;
    public GameObject[] dialogOptions;
    public GameObject dialogPanel;
    public Text dialogText, characterNameText;
    public int charactersPerPage = 100;

    private UnityEngine.Events.UnityAction closeDialog;
    //private List<string> currentMessagesToShow;

    void Awake() {
        instance = this;
        closeDialog = new UnityEngine.Events.UnityAction(() => {
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

    // Used for showing large amounts of dialog. Will show dialog in multiple chunks
    public void showDialog(string dialog, 
        string characterName, 
        Sprite characterSprite,
        Action functionToCallWhenDialogFinished) {
        
        string[] words = dialog.Split(new char[] { ' ' });
        List<string> messages = new List<string>();
        string message = "";
        int i;

        for(i = 0 ; i < words.Length ; i++) {
            if(words[i].Length+message.Length > charactersPerPage) {
                messages.Add(message + words[i]);
                message = "";
            } else {
                message += words[i] + " ";
            }
        }
        if(!message.Equals("")) {
            messages.Add(message);
        }
        
        i = 0;

        showDialog(
            dialog: messages[0],
            buttonsText: new string[] { "Next" },
            buttonsAction: new Action[] { () => {
                i++;
                if(i >= messages.Count) {
                    closeDialog();
                    functionToCallWhenDialogFinished();
                    return;
                }
                
                dialogText.text = messages[i];
                
            } },
            characterName: characterName,
            characterSprite: characterSprite,
            closePopUpWhenHit: new bool[] { false });
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
                dialogOptions[i].GetComponent<Button>().onClick.AddListener(closeDialog);
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
