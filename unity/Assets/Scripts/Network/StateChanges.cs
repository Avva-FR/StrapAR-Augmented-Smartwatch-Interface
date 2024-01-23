using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
using System;

public static class StateChanges
{
    public static int currentAppState;
    public static string appOpened = "";
    public static string rotationDirection = "";

    static void Start()
    {
        // do nothing
    }

    /*  If you send an int from the watch representing the state like we do in the Arduinopart
     *  Call This function to set the State so, HandleConfirmButtonPress() selects the correct behaviour
     */
    public static void SetAppState(string state)
    {
        resetRotation();
        switch (state){
            case "0":
            case "1":
            case "2":
            case "3":
                appOpened = "";
                currentAppState = int.Parse(state); 
                break;
            case "a1":
            case "a2":
            case "a3":
                appOpened = state; 
                break; 
            case "cw":
            case "ccw":
                rotationDirection = state;
                break;
        }
    }

    public static int getState()
    {
        return currentAppState;
    }

    public static string getRotation()
    {
        return rotationDirection;
    }

    public static void resetRotation()
    {
        rotationDirection = "";
    }

    public static string getOpenedApp()
    {
        return appOpened;
    }

    // this thing exist :)
    static void Update()
    {

    }
}