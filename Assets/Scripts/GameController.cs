﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public bool active = false;
    public int globalCounter;
    public ButtonsController VisualButtonController;
    public ButtonsController AudioButtonController;

    private ScreenOutput screenOutput;
    private bool screenOutputCoroutineIsFinished = false;

    // Use this for initialization
    void Start () {
        globalCounter = 0;
        screenOutput = GameObject.Find("Screen").GetComponent<ScreenOutput>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (active) {

            if(VisualButtonController.shouldDisplayWarningMessage || AudioButtonController.shouldDisplayWarningMessage)
            {
                screenOutput.MSText.text = "Please put your hands in the starting areas";
            }
            else
            {
                screenOutput.MSText.text = "";
            }

            if (globalCounter > 15) // END OF TESTS
            {
                // STOP GAME
                Debug.Log("End of test");
                VisualButtonController.active = false;
                AudioButtonController.active = false;
            }
            else if (VisualButtonController.countRepetitions() > 4) // BEGIN AUDIO TEST
            {
                globalCounter += VisualButtonController.counter;
                VisualButtonController.resetCounter();
                if (VisualButtonController.active)
                {
                    VisualButtonController.active = false;
                    //AudioButtonController.active = true;
                    StartCoroutine(WaitForScreenOutputART());
                }
                
            }
            else if (AudioButtonController.countRepetitions() > 4) // BEGIN VISUAL TEST
            {
                globalCounter += AudioButtonController.counter;
                VisualButtonController.resetCounter();
                if (!VisualButtonController.active)
                {
                    //VisualButtonController.active = true;
                    AudioButtonController.active = false;
                    StartCoroutine(WaitForScreenOutputVRT());
                }
                
            }
        }
        
	}

    public void BeginTest()
    {
        StartCoroutine(WaitForScreenOutputVRT());
    }

    IEnumerator WaitForScreenOutputVRT()
    {
        yield return StartCoroutine(screenOutput.TypeMainScreenText("Visual"));
        active = true;
        VisualButtonController.active = true;
    }

    IEnumerator WaitForScreenOutputART()
    {
        yield return StartCoroutine(screenOutput.TypeMainScreenText("Audio"));
        active = true;
        AudioButtonController.active = true;
    }



}
