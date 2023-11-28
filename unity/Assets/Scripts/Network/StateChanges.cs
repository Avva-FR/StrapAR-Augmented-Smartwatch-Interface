using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
using System;

public static class StateChanges
{
    public static int currentAppState;

    static void Start()
    {
        // do nothing
    }

    /*  If you send an int from the watch representing the state like we do in the Arduinopart
     *  Call This function to set the State so, HandleConfirmButtonPress() selects the correct behaviour
     */
    public static void SetAppState(int state)
    {
        currentAppState = state;
    }

    public static int getState()
    {
        return currentAppState;
    }

    // this thing exist :)
    static void Update()
    {

    }
}