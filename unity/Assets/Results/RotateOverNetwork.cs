using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMLD.MixedReality.Network;
using System;


public class RotateOverNetwork : MonoBehaviour
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
        Calendar = 1,
        Graph = 2,
        Weather = 3
    }
    public AppStates currentAppState;

    private int s1MsgCount = 0;

    void Start()
    {
        NetworkServer.Instance.RegisterMessageHandler(MessageContainer.MessageType.Sensor0, HandleSensor0Data);
        NetworkServer.Instance.RegisterMessageHandler(MessageContainer.MessageType.Sensor1, HandleSensor1Data);
        NetworkServer.Instance.RegisterMessageHandler(MessageContainer.MessageType.Sensor2, HandleSensor2Data);
        NetworkServer.Instance.RegisterMessageHandler(MessageContainer.MessageType.Sensor3, HandleSensor3Data);
        NetworkServer.Instance.RegisterMessageHandler(MessageContainer.MessageType.Sensor4, HandleSensor3Data);
    }

    //
    // this sensor corresponds to bwd
    public void HandleSensor0Data(MessageContainer container)
    {
        var messageS0 = MsgBinUintS0.Unpack(container);
        uint data0 = messageS0.Data;
        Debug.Log("recv Sensor0 data: " + data0);
        s1MsgCount++;
        
        // debug
        SetAppState(0);
        // implement a timer for double tap
        fwdButtonPressed = true;
        HandleButtonPress();

    }

    public void HandleSensor1Data(MessageContainer container)
    {
        // @TODO        
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
    public void SetAppState(int receivedAppState)
    {
        switch (receivedAppState)
        {
            case 0:
                currentAppState = AppStates.Default;
                break;
            case 1:
                currentAppState = AppStates.Calendar;
                break;
            case 2:
                currentAppState = AppStates.Graph;
                break;
            case 3:
                currentAppState = AppStates.Weather;
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
                case AppStates.Calendar:
                    HandleCalenderAppConfirmPress();
                    break;
                case AppStates.Graph:
                    HandleGraphAppConfirmPress();
                    break;
                case AppStates.Weather:
                    HandleWeatherAppConfirmPress();
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
                case AppStates.Calendar:
                    HandleCalenderAppFWDPress();
                    break;
                case AppStates.Graph:
                    HandleGraphAppFWDPress();
                    break;
                case AppStates.Weather:
                    HandleWeatherAppFWDPress();
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
                case AppStates.Calendar:
                    HandleCalenderAppBWDPress();
                    break;
                case AppStates.Graph:
                    HandleGraphAppBWDPress();
                    break;
                case AppStates.Weather:
                    HandleWeatherAppBWDPress();
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
                case AppStates.Calendar:
                    HandleCalenderAppBothPress();
                    break;
                case AppStates.Graph:
                    HandleGraphAppBothPress();
                    break;
                case AppStates.Weather:
                    HandleWeatherAppBothPress();
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
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.0f, 0.0f, 0.0f);
        }
    }

    public void HandleCalenderAppConfirmPress()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.196f, 0.803f, 0.196f);
        }
    }
    public void HandleGraphAppConfirmPress()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.722f, 0.059f, 0.039f);
        }
    }
    public void HandleWeatherAppConfirmPress()
    {
       if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.988f, 0.886f, 0.020f);
        }
    }

    //
    // Forward Button Pressed Handlers
    //
    public void HandleDefaultFWDButtonPress()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.2f, 0.2f);
        }
    }

    public void HandleCalenderAppFWDPress()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.604f, 0.804f, 0.196f);
        }
    }

    public void HandleWeatherAppFWDPress()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.980f, 0.855f, 0.369f);
        }
    }
    public void HandleGraphAppFWDPress()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.706f, 0.216f, 0.341f);
        }
    }

    //
    // Backward Button Press Handlers
    //
    public void HandleDefaultBWDButtonPress()
    {
        // MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        // // Make sure the MeshRenderer component exists
        // if (meshRenderer != null)
        // {
        //     // Create a new material instance to avoid changing the original prefab material
        //     Material material = new Material(meshRenderer.sharedMaterial);

        //     // Set the new color to the material
        //     material.color = newColor;

        //     // Assign the new material to the MeshRenderer
        //     meshRenderer.material = material;
        // }

       if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.4f, 0.4f, 0.4f);
        } 
    }

    public void HandleCalenderAppBWDPress()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.4f, 0.804f, 0.667f);
        } 
    }

    public void HandleWeatherAppBWDPress()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.988f, 0.957f, 0.639f);
        } 
    }
    public void HandleGraphAppBWDPress()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.804f, 0.361f, 0.361f);
        } 
    }

    //
    // Both (forward and backward) Button Press Handlers
    //
    public void HandleDefaultBothButtonPress()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.667f, 0.667f, 0.667f);
        } 
    }

    public void HandleCalenderAppBothPress()
    {
         if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.565f, 0.933f, 0.565f);
        } 
    }

    public void HandleWeatherAppBothPress()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(1f, 0.992f, 0.816f);
        }  
    }
    public void HandleGraphAppBothPress()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.980f, 0.502f, 0.447f);
        } 
    }

    // this thing exist :)
    void Update()
    {

    }
}
