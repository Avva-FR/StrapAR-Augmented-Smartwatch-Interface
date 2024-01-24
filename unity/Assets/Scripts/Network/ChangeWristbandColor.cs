using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMLD.MixedReality.Network;
using System;


public class ChangeWristbandColor : HandleSensorData
{
    //debug
    public Color newColor = Color.red;

    public int currentAppInt = 0;

    public override void Start()
    {
        base.Start();
        GetComponent<MeshRenderer>().material.color = new Color(0.0f, 0.0f, 0.0f);
    }


    /*  If you send an int from the watch representing the state like we do in the Arduinopart
     *  Call This function to set the State so, HandleConfirmButtonPress() selects the correct behaviour
     */
    public override void SetAppState(int state)
    {
        switch (state)
        {
            case 0:
                currentAppState = AppStates.Default;
                GetComponent<MeshRenderer>().material.color = new Color(0.0f, 0.0f, 0.0f);
                break;
            case 1:
                currentAppState = AppStates.Weather;
                GetComponent<MeshRenderer>().material.color = new Color(0.910f, 0.639f, 0.090f);
                break;
            case 2:
                currentAppState = AppStates.Graph;
                GetComponent<MeshRenderer>().material.color = new Color(0.494f, 0.098f, 0.106f);
                break;
            case 3:
                currentAppState = AppStates.Documents;
                GetComponent<MeshRenderer>().material.color = new Color(0.043f, 0.4f, 0.137f);
                break;
            default:
                // unknown state
                break;
        }
    }

    public override void Update()
    {
        if (currentAppInt != StateChanges.getState())
        {
            currentAppInt = StateChanges.getState();
            SetAppState(currentAppInt);
        }
    }
}