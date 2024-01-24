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
    public static string rotationDirection1 = "";
    public static string rotationDirection2 = "";

    static void Start()
    {
        // do nothing
    }

    /*  If you send an int from the watch representing the state like we do in the Arduinopart
     *  Call This function to set the State so, HandleConfirmButtonPress() selects the correct behaviour
     */
    public static void SetAppState(string state)
    {
        resetRotation1();
        resetRotation2();
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
                rotationDirection1 = state;
                rotationDirection2 = state;
                break;
        }
    }

    public static int getState()
    {
        return currentAppState;
    }

    public static string getRotation1()
    {
        return rotationDirection1;
    }

    public static void resetRotation1()
    {
        rotationDirection1 = "";
    }

    public static string getRotation2()
    {
        return rotationDirection2;
    }

    public static void resetRotation2()
    {
        rotationDirection2 = "";
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