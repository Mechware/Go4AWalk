﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class DialoguePopUp : MonoBehaviour {

    public static DialoguePopUp instance;
    public Image characterSprite;
    public GameObject[] popUpButtons;
    public GameObject popUpPanel, popUpTitle, popUpImage;

    private UnityEngine.Events.UnityAction closePopUp;

    void Awake() {
        instance = this;
        closePopUp = new UnityEngine.Events.UnityAction(() => {
            popUpPanel.SetActive(false);
        });
        
    }

    // Use this for initialization
    void Start() {
        resetButtons();
    }

    // Update is called once per frame
    void Update() {

    }

    public void showPopUp(string title, Image character, string[] buttonTitles, Action[] actionWhenButtonHit,
        bool[] closePopUpWhenHit = null) {

        int i;
        if (closePopUpWhenHit == null) {
            closePopUpWhenHit = new bool[buttonTitles.Length];
            for (i = 0 ; i < closePopUpWhenHit.Length ; i++) {
                closePopUpWhenHit[i] = true;
            }
        }

        UnityEngine.Assertions.Assert.IsFalse(
            buttonTitles.Length == 0,
            "No button titles provided");
        UnityEngine.Assertions.Assert.IsFalse(
            buttonTitles.Length != actionWhenButtonHit.Length,
            "Button and action array length must be the same");

        resetButtons();

        popUpTitle.GetComponent<Text>().text = title;

        for (i = 0 ; i < buttonTitles.Length ; i++) {
            popUpButtons[i].SetActive(true);
            popUpButtons[i].GetComponentInChildren<Text>().text = buttonTitles[i];
            UnityEngine.Events.UnityAction action = new UnityEngine.Events.UnityAction(actionWhenButtonHit[i]);
            if (closePopUpWhenHit[i])
                popUpButtons[i].GetComponent<Button>().onClick.AddListener(closePopUp);
            popUpButtons[i].GetComponent<Button>().onClick.AddListener(action);
        }
        while (i < popUpButtons.Length) {
            popUpButtons[i].SetActive(false);
            i++;
        }
        popUpPanel.SetActive(true);
    }

    void resetButtons() {
        for (int i = 0 ; i < popUpButtons.Length ; i++) {
            popUpButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}