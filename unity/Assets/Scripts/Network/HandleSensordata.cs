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
    // set these when AppStatechanges are received
    public enum AppStates
    {
        Default = 0,
        Weather = 1,
        Graph = 2,
        Documents = 3
    }
    public AppStates currentAppState;

    void Start()
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
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.0f, 0.0f, 0.0f);
        }
    }

    public void HandleDocumentsAppConfirmPress()
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

    public void HandleDocumentsAppFWDPress()
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

    public void HandleDocumentsAppBWDPress()
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
    // leave this as is for now or implement timer based detection
    public void HandleDefaultBothButtonPress()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.color = new Color(0.667f, 0.667f, 0.667f);
        } 
    }

    public void HandleDocumentsAppBothPress()
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

    void Update()
    {
    }
}
