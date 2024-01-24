using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMLD.MixedReality.Network;
using System;

public class HandleSensorData : MonoBehaviour
{
    public bool fwdButtonPressed = false;
    public bool fwdDoublePress = false;
    public int fwdTapCount = 0;

    public bool confirmPressed = false;
    public bool confirmDoublePress = false;
    public int confirmTapCount = 0;

    public bool bwdButtonPressed = false;
    public bool bwdDoublePress = false;
    public int bwdTapCount = 0;

    public float doubleTapTimer = 0.5f;
    public bool isCoroutineRunning = false;
    
    public int currentAppInt = 0;
    public static string appOpened = "";
    public static string rotationDirection = "";

    // set these when AppStatechanges are received
    public enum AppStates
    {
        Default = 0,
        Weather = 1,
        Graph = 2,
        Documents = 3
    }
    public AppStates currentAppState;

    public virtual void Start()
    {
        NetworkServer.Instance.RegisterMessageHandler(MessageContainer.MessageType.Sensor0, HandleSensor0Data);
        NetworkServer.Instance.RegisterMessageHandler(MessageContainer.MessageType.Sensor2, HandleSensor2Data);
        NetworkServer.Instance.RegisterMessageHandler(MessageContainer.MessageType.Sensor4, HandleSensor4Data);
    }

    private IEnumerator InputCoroutine(int context)
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(doubleTapTimer);
        isCoroutineRunning = false;

        switch (context)
        {
            case 0:
                if (fwdTapCount >= 2)
                {
                    fwdDoublePress = true;
                } else
                {
                    fwdButtonPressed = true;
                }
                fwdTapCount = 0;
                break;
            case 2:
                if (confirmTapCount >= 2)
                {
                    confirmDoublePress = true;
                }
                else
                {
                    confirmPressed = true;
                }
                confirmTapCount = 0;
                break;
                
            case 4:
                if (bwdTapCount >= 2)
                {
                    bwdDoublePress = true;
                }
                else
                {
                    bwdButtonPressed = true;
                }
                bwdTapCount = 0;
                break;
        }    
}

    // Sensor 1 recv Handler
    public void HandleSensor0Data(MessageContainer container)
    {
        var messageS0 = MsgBinUintS0.Unpack(container);
        uint data0 = messageS0.Data;
        Debug.Log("recv Sensor0 data: " + data0);
        fwdTapCount++;
        if (!isCoroutineRunning)
        {
            StartCoroutine(InputCoroutine(0));
        }
        HandleButtonPress();
    }

    // Messagehandler for middle Sensor equivalent to confirm
    public void HandleSensor2Data(MessageContainer container)
    {
        // if you want to doe something with the data received
        var messageS2 = MsgBinUintS2.Unpack(container);
        uint data2 = messageS2.Data;
        Debug.Log("recv Sensor2 data: " + data2);
        confirmTapCount++;
        if (!isCoroutineRunning)
        {
            StartCoroutine(InputCoroutine(2));
        }
        HandleButtonPress();
    }

    public void HandleSensor4Data(MessageContainer container)
    {
        var messageS4 = MsgBinUintS4.Unpack(container);
        uint data4 = messageS4.Data;
        Debug.Log("recv Sensor4 data: " + data4);
        bwdTapCount++;
        if (!isCoroutineRunning)
        {
            StartCoroutine(InputCoroutine(4));
        }
        HandleButtonPress();
    }

    /*  If you send an int from the watch representing the state like we do in the Arduinopart
     *  Call This function to set the State so, HandleConfirmButtonPress() selects the correct behaviour
     */
    public void SetAppState(int state)
    {
        switch (state)
        {
            case 0:
                currentAppState = AppStates.Default;
                break;
            case 1:
                currentAppState = AppStates.Weather;
                break;
            case 2:
                currentAppState = AppStates.Graph;
                break;
            case 3:
                currentAppState = AppStates.Documents;
                break;
            default:
                // unknown state
                break;
        }
    }

    public void SetRotation(string rotationDirection)
    {
        switch (rotationDirection)
        {
            case "cw":
                HandleButtonPress();
                break;
            case "ccw":
                HandleButtonPress();
                break;
            default:
                // unknown state
                break;
        }
    }

    // opens the first page of the corresponding app
    // "enters the app-menu"
    public void SetOpenedApp(string appOpened)
    {
        switch (appOpened)
        {
            case "a1":
                currentAppState = AppStates.Weather;
                break;
            case "a2":
                currentAppState = AppStates.Graph;
                break;
            case "a3":
                currentAppState = AppStates.Documents;
                break;
            default:
                // unknown state
                break;
        }
    }

    // confirm button behaviour selector
    public void HandleButtonPress()
    {
        // confirm button presses
        if (confirmPressed)
        {
            switch (currentAppState)
            {
                case AppStates.Weather:
                    HandleWeatherAppConfirmPress();
                    break;
                case AppStates.Graph:
                    HandleGraphAppConfirmPress();
                    break;
                case AppStates.Documents:
                    HandleDocumentsAppConfirmPress();
                    break;
                default:
                    HandleDefaultConfirmPress();
                    break;
            }
            confirmPressed = false;
        }
        else if (confirmDoublePress & !bwdButtonPressed & !fwdButtonPressed)
        {
            switch (currentAppState)
            {
                case AppStates.Weather:
                    HandleWeatherAppConfirmDoublePress();
                    break;
                case AppStates.Graph:
                    HandleGraphAppConfirmDoublePress();
                    break;
                case AppStates.Documents:
                    HandleDocumentsAppConfirmDoublePress();
                    break;
                default:
                    HandleDefaultConfirmDoublePress();
                    break;
            }
            confirmDoublePress = false;
        }
        //  forward button presses
        else if (fwdButtonPressed & !bwdButtonPressed & !confirmPressed)
        {
            switch (currentAppState)
            {
                case AppStates.Weather:
                    HandleWeatherAppFWDPress();
                    break;
                case AppStates.Graph:
                    HandleGraphAppFWDPress();
                    break;
                case AppStates.Documents:
                    HandleDocumentsAppFWDPress();
                    break;
                default:
                    HandleDefaultFWDButtonPress();
                    break;
            }
            fwdButtonPressed = false;
        }
        else if (fwdDoublePress & !confirmPressed & !bwdButtonPressed)
        {
            switch (currentAppState)
            {
                case AppStates.Weather:
                    HandleWeatherAppFWDDoublePress();
                    break;
                case AppStates.Graph:
                    HandleGraphAppFWDDoublePress();
                    break;
                case AppStates.Documents:
                    HandleDocumentsAppFWDDoublePress();
                    break;
                default:
                    HandleDefaultFWDDoubleButtonPress();
                    break;
            }
            fwdDoublePress = false;
        }
        // backwards button presses
        else if (bwdButtonPressed & !fwdButtonPressed & !confirmPressed)
        {
            switch (currentAppState)
            {
                case AppStates.Weather:
                    HandleWeatherAppBWDPress();
                    break;
                case AppStates.Graph:
                    HandleGraphAppBWDPress();
                    break;
                case AppStates.Documents:
                    HandleDocumentsAppBWDPress();
                    break;
                default:
                    HandleDefaultBWDButtonPress();
                    break;
            }
            bwdButtonPressed = false;
        }
        else if (bwdDoublePress & !confirmPressed & !fwdButtonPressed)
        {
            switch (currentAppState)
            {
                case AppStates.Weather:
                    HandleWeatherAppBWDDoublePress();
                    break;
                case AppStates.Graph:
                    HandleGraphAppBWDDoublePress();
                    break;
                case AppStates.Documents:
                    HandleDocumentsAppBWDDoublePress();
                    break;
                default:
                    HandleDefaultBWDButtonDoublePress();
                    break;
            }
            bwdDoublePress = false;
        }
        // unused
        else if (fwdButtonPressed & bwdButtonPressed)
        {
            switch (currentAppState)
            {
                case AppStates.Weather:
                    HandleWeatherAppBothPress();
                    break;
                case AppStates.Graph:
                    HandleGraphAppBothPress();
                    break;
                case AppStates.Documents:
                    HandleDocumentsAppBothPress();
                    break;
                default:
                    HandleDefaultBothButtonPress();
                    break;
            }
            bwdButtonPressed = false;
            fwdButtonPressed = false;
        }
        else if (rotationDirection.Equals("cw"))
        {
            switch (currentAppState)
            {
                case AppStates.Weather:
                    HandleWeatherAppCWRotation();
                    break;
                case AppStates.Graph:
                    HandleGraphAppCWRotation();
                    break;
                case AppStates.Documents:
                    HandleDocumentsAppCWRotation();
                    break;
                default:
                    HandleDefaultCWRotation();
                    break;
            }
            rotationDirection = "";
        }
        else if (rotationDirection.Equals("ccw"))
        {
            switch (currentAppState)
            {
                case AppStates.Weather:
                    HandleWeatherAppCCWRotation();
                    break;
                case AppStates.Graph:
                    HandleGraphAppCCWRotation();
                    break;
                case AppStates.Documents:
                    HandleDocumentsAppCCWRotation();
                    break;
                default:
                    HandleDefaultCCWRotation();
                    break;
            }
            rotationDirection = "";
        }
    }

    // Confirm Double Taps
    // use this to exit input modalities
    public void HandleWeatherAppConfirmDoublePress()
    {
        //@TODO
    }
    public void HandleGraphAppConfirmDoublePress()
    {
        //@TODO
    }
    public void HandleDefaultConfirmDoublePress()
    {
        //@TODO
    }
    public void HandleDocumentsAppConfirmDoublePress()
    {
        //@TODO
    }
    // Forward Double Taps
    public void HandleWeatherAppFWDDoublePress()
    {
        //@TODO
    }
    public void HandleGraphAppFWDDoublePress()
    {
        //@TODO
    }
    public void HandleDocumentsAppFWDDoublePress()
    {
        //@TODO
    }
    public void HandleDefaultFWDDoubleButtonPress()
    {
        //@TODO
    }
    public void HandleWeatherAppBWDDoublePress()
    {
        //@TODO
    }
    public void HandleGraphAppBWDDoublePress()
    {
        //@TODO
    }
    public void HandleDocumentsAppBWDDoublePress()
    {
        //@TODO
    }
    public void HandleDefaultBWDButtonDoublePress()
    {
        //@TODO
    }

    // Confirm Button Press Handler
    public void HandleDefaultConfirmPress()
    {
        //@TODO
    }

    public void HandleDocumentsAppConfirmPress()
    {
        //@TODO
    }
    public void HandleGraphAppConfirmPress()
    {
        //@TODO
    }
    public void HandleWeatherAppConfirmPress()
    {
       //@TODO
    }

    //
    // Forward Button Pressed Handlers
    //
    public void HandleDefaultFWDButtonPress()
    {
        //@TODO
    }

    public void HandleDocumentsAppFWDPress()
    {
        //@TODO
    }

    public void HandleWeatherAppFWDPress()
    {
        //@TODO
    }
    public void HandleGraphAppFWDPress()
    {
        //@TODO
    }

    //
    // Backward Button Press Handlers
    //
    public void HandleDefaultBWDButtonPress()
    {
        //@TODO
    }

    public void HandleDocumentsAppBWDPress()
    {
        //@TODO
    }

    public void HandleWeatherAppBWDPress()
    {
        //@TODO
    }
    public void HandleGraphAppBWDPress()
    {
        //@TODO
    }

    //
    // Both (forward and backward) Button Press Handlers
    // leave this as is for now or implement timer based detection
    public void HandleDefaultBothButtonPress()
    {
        //@TODO
    }

    public void HandleDocumentsAppBothPress()
    {
         //@TODO
    }

    public void HandleWeatherAppBothPress()
    {
        //@TODO
    }
    public void HandleGraphAppBothPress()
    {
        //@TODO
    }

    //
    // CW Rotation Handlers
    //
    public void HandleDefaultCWRotation()
    {
        //@TODO
    }

    public void HandleDocumentsAppCWRotation()
    {
        //@TODO
    }

    public void HandleWeatherAppCWRotation()
    {
        //@TODO
    }

    public void HandleGraphAppCWRotation()
    {
        //@TODO
    }

    //
    // CCW Rotation Handlers
    //
    public void HandleDefaultCCWRotation()
    {
        //@TODO
    }

    public void HandleDocumentsAppCCWRotation()
    {
        //@TODO
    }

    public void HandleWeatherAppCCWRotation()
    {
        //@TODO
    }

    void Update()
    {
        if(currentAppInt != StateChanges.getState())
        {
           currentAppInt = StateChanges.getState(); 
           SetAppState(currentAppInt);
        }
        if(!appOpened.Equals(StateChanges.getOpenedApp()))
        {
           appOpened = StateChanges.getOpenedApp(); 
           SetOpenedApp(appOpened);
        }
        if(!rotationDirection.Equals(StateChanges.getRotation()))
        {
            Debug.Log(rotationDirection);
            rotationDirection = StateChanges.getRotation();
            StateChanges.resetRotation();
            SetRotation(rotationDirection);
        }
    }
}
