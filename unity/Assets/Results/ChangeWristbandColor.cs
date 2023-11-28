using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMLD.MixedReality.Network;
using System;


public class ChangeWristbandColor : MonoBehaviour
{
    public bool confirmPressed = false;
    public bool fwdButtonPressed = false;
    public bool bwdButtonPressed = false;

    //debug
    public Color newColor = Color.red;

    // set these when AppStatechanges are received
    public enum AppStates
    {
        Default = 0,
        Weather = 1,
        Graph = 2,
        Documents = 3
    }
    public AppStates currentAppState;
    public int currentAppInt = 0;

    private int s1MsgCount = 0;

    void Start()
    {
        GetComponent<MeshRenderer>().material.color = new Color(0.0f, 0.0f, 0.0f);
        NetworkServer.Instance.RegisterMessageHandler(MessageContainer.MessageType.Sensor0, HandleSensor0Data);
        NetworkServer.Instance.RegisterMessageHandler(MessageContainer.MessageType.Sensor1, HandleSensor1Data);
        NetworkServer.Instance.RegisterMessageHandler(MessageContainer.MessageType.Sensor2, HandleSensor2Data);
        NetworkServer.Instance.RegisterMessageHandler(MessageContainer.MessageType.Sensor3, HandleSensor3Data);
        NetworkServer.Instance.RegisterMessageHandler(MessageContainer.MessageType.Sensor4, HandleSensor4Data);
    }

    //
    // this sensor corresponds to bwd
    public void HandleSensor0Data(MessageContainer container)
    {
        var messageS0 = MsgBinUintS0.Unpack(container);
        uint data0 = messageS0.Data;
        Debug.Log("recv Sensor0 data: " + data0);
        s1MsgCount++;
        
        // // debug
        // SetAppState(0);
        // implement a timer for double tap
        fwdButtonPressed = true;
        HandleButtonPress();

    }

    public void HandleSensor1Data(MessageContainer container)
    {
            var msg1 = MsgBinUintS1.Unpack(container);
    }

    // Messagehandler for middle Sensor equivalent to confirm

    public void HandleSensor2Data(MessageContainer container)
    {
        // if you want to doe something with the data received
        var messageS2 = MsgBinUintS2.Unpack(container);
        uint data2 = messageS2.Data;
        Debug.Log("recv Sensor2 data: " + data2);

        confirmPressed = true;
        HandleButtonPress();
    }

    public void HandleSensor3Data(MessageContainer container)
    {
        // @TODO
    }

    public void HandleSensor4Data(MessageContainer container)
    {
        var messageS4 = MsgBinUintS4.Unpack(container);
        uint data4 = messageS4.Data;
        Debug.Log("recv Sensor4 data: " + data4);

        bwdButtonPressed = true;
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






    // verbose piece of shit 
    //
    // confirm button behaviour selector
    public void HandleButtonPress()
    {
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
            // Reset
            confirmPressed = false;
        }
        else if (fwdButtonPressed & !bwdButtonPressed)
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
        else if (bwdButtonPressed & !fwdButtonPressed)
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
    }


    // @TODO implement specific behaviour
    public void HandleDefaultConfirmPress()
    {
        // do nothing for now
    }

    public void HandleDocumentsAppConfirmPress()
    {
        // do nothing for now
    }
    public void HandleGraphAppConfirmPress()
    {
       // do nothing for now
    }
    public void HandleWeatherAppConfirmPress()
    {
       // do nothing for now
    }

    //
    // Forward Button Pressed Handlers
    //
    public void HandleDefaultFWDButtonPress()
    {
        // do nothing for now
    }

    public void HandleDocumentsAppFWDPress()
    {
        // do nothing for now
    }

    public void HandleWeatherAppFWDPress()
    {
        // do nothing for now
    }
    public void HandleGraphAppFWDPress()
    {
        // do nothing for now
    }

    //
    // Backward Button Press Handlers
    //
    public void HandleDefaultBWDButtonPress()
    {
        // do nothing for now
    }

    public void HandleDocumentsAppBWDPress()
    {
        // do nothing for now
    }

    public void HandleWeatherAppBWDPress()
    {
       // do nothing for now
    }
    public void HandleGraphAppBWDPress()
    {
        // do nothing for now
    }

    //
    // Both (forward and backward) Button Press Handlers
    //
    public void HandleDefaultBothButtonPress()
    {
        // do nothing for now
    }

    public void HandleDocumentsAppBothPress()
    {
        // do nothing for now
    }

    public void HandleWeatherAppBothPress()
    {
        // do nothing for now
    }
    public void HandleGraphAppBothPress()
    {
        // do nothing for now
    }

    // this thing exist :)
    void Update()
    {
        if(currentAppInt != StateChanges.getState())
        {
           currentAppInt = StateChanges.getState(); 
           SetAppState(currentAppInt);
        }
    }
}
