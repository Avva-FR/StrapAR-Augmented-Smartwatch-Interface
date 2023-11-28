using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMLD.MixedReality.Network;
using System;


public class ChangePlaneTexture : MonoBehaviour
{
    public bool confirmPressed = false;
    public bool fwdButtonPressed = false;
    public bool bwdButtonPressed = false;
    public bool pageZoomActive = false;

    public int currentPage = 0;
    public int currentAppInt = 0;

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

    private int s1MsgCount = 0;

    void Start()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/start"));
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
        
        // // debug
        // SetAppState(0);
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
    public void SetAppState(int state)
    {
        switch (state)
        {
            case 0:
                currentAppState = AppStates.Default;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/start"));
                currentPage = 0;
                break;
            case 1:
                currentAppState = AppStates.Weather;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather"));
                currentPage = 0;
                break;
            case 2:
                currentAppState = AppStates.Graph;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph"));
                currentPage = 0;
                break;
            case 3:
                currentAppState = AppStates.Documents;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents"));
                currentPage = 0;
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
            //Reset potential pageZoom status
            pageZoomActive = false;
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
            //Reset potential pageZoom status
            pageZoomActive = false;
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
            //Reset potential pageZoom status
            pageZoomActive = false;
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
    // currently changes the page of the "weather-app" until it reaches page 3 and then stops
    // in all other states causes color change of icon to "WHITE"
    public void HandleDefaultConfirmPress()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/start_0"));
    }

    public void HandleDocumentsAppConfirmPress()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_0"));
    }
    public void HandleGraphAppConfirmPress()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_0"));
    }
    public void HandleWeatherAppConfirmPress()
    {
       switch (currentPage)
        {
            case 0:
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/page1")); 
               currentPage = 1;
               break;
            case 1:
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/page2")); 
               currentPage = 2;
               break;
            case 2:
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/page3")); 
               currentPage = 3;
               break;
            default:
               // do nothing 
               break;
        }
    }

    //
    // Forward Button Pressed Handlers
    // currently zooms into the graphs and images of the "weather-app" and goes back top normal if pressed again
    // in all other states causes color change of icon to "GREEN"
    //
    public void HandleDefaultFWDButtonPress()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/start_1"));
    }

    public void HandleDocumentsAppFWDPress()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_1"));
    }

    public void HandleWeatherAppFWDPress()
    {
        switch (currentPage)
        {
            case 1:
                if(pageZoomActive)
                {
                   GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/page1")); 
                   pageZoomActive = false;
                }
                else
                {
                   GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/page1_zoom"));
                   pageZoomActive = true; 
                }
               break;
            case 2:
               if(pageZoomActive)
                {
                   GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/page2")); 
                   pageZoomActive = false;
                }
                else
                {
                   GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/page2_zoom"));
                   pageZoomActive = true; 
                }
               break;
            case 3:
               if(pageZoomActive)
                {
                   GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/page3")); 
                   pageZoomActive = false;
                }
                else
                {
                   GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/page3_zoom"));
                   pageZoomActive = true; 
                }
               break;
            default:
               // do nothing 
               break;
        }
    }
    public void HandleGraphAppFWDPress()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_1"));
    }

    //
    // Backward Button Press Handlers
    // currently goes back to the previously opened page of the "weather-app" but will not go to the title image ("first page")
    // in all other states causes color change of icon to "BLUE"
    //
    public void HandleDefaultBWDButtonPress()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/start_2"));
    }

    public void HandleDocumentsAppBWDPress()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_2"));
    }

    public void HandleWeatherAppBWDPress()
    {
        switch (currentPage)
        {
            case 2:
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/page1")); 
               currentPage = 1;
               break;
            case 3:
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/page2")); 
               currentPage = 2;
               break;
            default:
               // do nothing 
               break;
        }
    }
    public void HandleGraphAppBWDPress()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_2"));
    }

    //
    // Both (forward and backward) Button Press Handlers
    // currently transports the viewer back to the title image ("first page") of the weather app
    // in all other states causes color change of icon to "RED"
    //
    public void HandleDefaultBothButtonPress()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/start_3"));
    }

    public void HandleDocumentsAppBothPress()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_3"));
    }

    public void HandleWeatherAppBothPress()
    {
        switch (currentPage)
        {
            case 0:
               // do nothing
               break;
            default:
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather"));
                currentPage = 0;
               break;
        }
    }
    public void HandleGraphAppBothPress()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_3"));
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
