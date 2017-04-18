using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class PopUp : MonoBehaviour {

    public static PopUp instance;
    public GameObject[] popUpButtons;
    public GameObject popUpPanel, popUpTitle;
    Queue<PopUpParameters> popUpList;

    private UnityEngine.Events.UnityAction closePopUp;
    private bool isCurrentlyOpen = false;

    void Awake() {
        instance = this;
        popUpList = new Queue<PopUpParameters>();
        closePopUp = new UnityEngine.Events.UnityAction(() => {
            popUpPanel.SetActive(false);
            isCurrentlyOpen = false;
            openNext();
        });
    }

    void openNext() {

        if (popUpList.Count == 0) {
            return;
        }
            
        PopUpParameters parameters = popUpList.Dequeue();
        showPopUp(parameters.title,
            parameters.buttonTitles,
            parameters.actionWhenButtonHit,
            parameters.closePopUpWhenHit);
        
    }

	// Use this for initialization
	void Start () {
        resetButtons();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void saveParameters(string title,
        string[] buttonTitles,
        Action[] actionWhenButtonHit,
        bool[] closePopUpWhenHit) {
        popUpList.Enqueue(new PopUpParameters(
            title,
            buttonTitles,
            actionWhenButtonHit,
            closePopUpWhenHit));

    }

    public void showPopUp(string title, 
        string[] buttonTitles, 
        Action[] actionWhenButtonHit = null, 
        bool[] closePopUpWhenHit = null) {

        if(isCurrentlyOpen) {
            saveParameters(title, 
                buttonTitles, 
                actionWhenButtonHit, 
                closePopUpWhenHit);
            return;
        }

        int i;
        if (closePopUpWhenHit == null) {
            closePopUpWhenHit = new bool[buttonTitles.Length];
            for (i = 0 ; i < closePopUpWhenHit.Length ; i++) {
                closePopUpWhenHit[i] = true;
            }
        }

        if(actionWhenButtonHit == null) {
            actionWhenButtonHit = new Action[buttonTitles.Length];
            for(i = 0 ; i < actionWhenButtonHit.Length ; i++) {
                actionWhenButtonHit[i] = () => { };
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
            if(closePopUpWhenHit[i]) {
                popUpButtons[i].GetComponent<Button>().onClick.AddListener(closePopUp);
            }
                
            popUpButtons[i].GetComponent<Button>().onClick.AddListener(action);
        }
        while (i < popUpButtons.Length) {
            popUpButtons[i].SetActive(false);
            i++;
        }
        popUpPanel.SetActive(true);
        isCurrentlyOpen = true;
    }

    void resetButtons() {
        for (int i = 0 ; i < popUpButtons.Length ; i++) {
            popUpButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    private class PopUpParameters {
        public string title;
        public string[] buttonTitles;
        public Action[] actionWhenButtonHit = null;
        public bool[] closePopUpWhenHit = null;

        public PopUpParameters(string title,
            string[] buttonTitles,
            Action[] actionWhenButtonHit = null,
            bool[] closePopUpWhenHit = null) {

            this.title = title;
            this.buttonTitles = buttonTitles;
            this.actionWhenButtonHit = actionWhenButtonHit;
            this.closePopUpWhenHit = closePopUpWhenHit;
        }
    }
}


