﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public int numberOfTest = 10;
    public bool active = false;
    public int globalCounter;
    public ButtonsController VisualButtonController;
    public ButtonsController AudioButtonController;

    public float ARTMean;
    public float VRTMean;

    private ScreenOutput screenOutput;
    private bool screenOutputCoroutineIsFinished = false;

    public GameObject rightHandStartingArea;
    public GameObject leftHandStartingArea;

    // Use this for initialization
    void Start () {
        globalCounter = 0;
        screenOutput = GameObject.Find("Screen").GetComponent<ScreenOutput>();
	}
	
	// Update is called once per frame
	void Update () {
        if (active) {

            if (globalCounter == numberOfTest) // END OF TESTS
            {
                // STOP GAME
                Debug.Log("End of test");
                VisualButtonController.active = false;
                AudioButtonController.active = false;
                VisualButtonController.shouldDisplayWarningMessage = false;
                AudioButtonController.shouldDisplayWarningMessage = false;
                active = false;
            }
            else // TEST CONTINUES
            {
                // Display Starting areas
                if (!rightHandStartingArea.activeSelf && !leftHandStartingArea.activeSelf)
                {
                    rightHandStartingArea.SetActive(true);
                    leftHandStartingArea.SetActive(true);
                }

                if (VisualButtonController.shouldDisplayWarningMessage || AudioButtonController.shouldDisplayWarningMessage)
                {
                    screenOutput.MSText.text = "Please put your hands in the starting areas";
                }
                else if (globalCounter < numberOfTest)
                {
                    screenOutput.MSText.text = "";
                }


                if (VisualButtonController.countRepetitions() > 4 && globalCounter <= numberOfTest) // BEGIN AUDIO TEST
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
                else if (AudioButtonController.countRepetitions() > 4 && globalCounter <= numberOfTest) // BEGIN VISUAL TEST
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

            
        else // Hide starting areas
        {
            if (rightHandStartingArea.activeSelf && leftHandStartingArea.activeSelf)
            {
                rightHandStartingArea.SetActive(false);
                leftHandStartingArea.SetActive(false);
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

    public void ShowResultsOnScreen()
    {
        if (ARTMean != 0 & VRTMean != 0)
        {
            screenOutput.showResults(VRTMean, ARTMean);
        }
        else
        {
            Debug.Log("Error processing Data");
        }
    }


}
