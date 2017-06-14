﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour {

    public Timer timer;
    public bool active;
    protected GameObject indicatorObject;
    public TextMesh reactionTimeUI;
    public List<float> reactionTimes;
    public List<string> hands;
    private int counter = 0;

	// Use this for initialization
	void Start () {
        reactionTimes = new List<float>();
        timer = new Timer();
        active = false;
	}

	// Update is called once per frame
	protected virtual void Update () {
        if (timer.hasStarted)
        {
            if (timer.currentDuration() > 3000f)
            {
                Debug.Log("MISSION FAILED");
                End();
            }
        }
    }

    public virtual void Begin() {

    }

    public void End() {
        timer.StopTimer();
        //indicatorObject.GetComponent<MeshRenderer>().material.color = Color.red;
        if (reactionTimeUI) {
            reactionTimeUI.text = timer.duration.ToString();
        }
        storeRT(timer.duration);
        Debug.Log(reactionTimes[reactionTimes.Count - 1]);
        active = false;
    }


    public void storeRT(float RT) {
        reactionTimes.Add(RT);
        counter++;
    }


    // --- Counter
    public void resetCounter()
    {
        counter = 0;
    }

    public int getCounter()
    {
        return counter;
    }
    // ---
}
