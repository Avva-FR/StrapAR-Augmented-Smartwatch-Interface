using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMLD.MixedReality.Network;
using System;


public class ChangeBigPlaneTexture : MonoBehaviour
{
    public bool confirmPressed = false;
    public bool fwdButtonPressed = false;
    public bool bwdButtonPressed = false;
    public bool pageZoomActive = false;

    public int currentPage = 0;
    public int currentAppInt = 0;
    public static string appOpened = "";
    public static string rotationDirection = "";
    public string activeDocument = "";
    public bool insideMenu = true;
    public bool insideAppMenu = false;

    public bool bigDisplayActive = false;

    public bool reset = false;

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
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/start_big"));
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
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/start_big"));
                gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = false; 
                insideMenu = true;
                insideAppMenu = false;
                activeDocument = "";
                currentPage = 0;
                break;
            case 1:
                currentAppState = AppStates.Weather;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_big"));
                gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = false; 
                insideMenu = true;
                insideAppMenu = false;
                activeDocument = "";
                currentPage = 0;
                break;
            case 2:
                currentAppState = AppStates.Graph;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_big"));
                gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = false; 
                insideMenu = true;
                insideAppMenu = false;
                activeDocument = "";
                currentPage = 0;
                break;
            case 3:
                currentAppState = AppStates.Documents;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_big"));
                gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = false; 
                insideMenu = true;
                insideAppMenu = false;
                activeDocument = "";
                currentPage = 0;
                break;
            default:
                // unknown state
                break;
        }
    }

    // cw-rotation currently has the same functionality as a fwd-button-press and
    // ccw-rotation corresponds to a bwd-button-press
    //
    // might change as soon as I get a different idea for the functionality
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
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page1_big"));
                currentPage = 1;
                insideMenu = false;
                insideAppMenu = true;
                break;
            case "a3":
                currentAppState = AppStates.Documents;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0_big"));
                insideMenu = false;
                insideAppMenu = true;
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
            if(!reset)
            {
                if(bigDisplayActive)
                {
                    bigDisplayActive = false;  
                }
                else
                {
                    if(!insideAppMenu && !insideMenu)
                    {
                        gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = true;
                    }
                    bigDisplayActive = true;  
                }
            }

            // switch (currentAppState)
            // {
            //     case AppStates.Weather:
            //         HandleWeatherAppFWDPress();
            //         break;
            //     case AppStates.Graph:
            //         HandleGraphAppFWDPress();
            //         break;
            //     case AppStates.Documents:
            //         HandleDocumentsAppFWDPress();
            //         break;
            //     default:
            //         HandleDefaultFWDButtonPress();
            //         break;
            // }
            fwdButtonPressed = false;
        }
        else if (bwdButtonPressed & !fwdButtonPressed)
        {
            if(!reset)
            {
                bigDisplayActive = false; 
                reset = true;
            }
            else
            {
                reset = false;
            }
            // //Reset potential pageZoom status
            // pageZoomActive = false;
            // switch (currentAppState)
            // {
            //     case AppStates.Weather:
            //         HandleWeatherAppBWDPress();
            //         break;
            //     case AppStates.Graph:
            //         HandleGraphAppBWDPress();
            //         break;
            //     case AppStates.Documents:
            //         HandleDocumentsAppBWDPress();
            //         break;
            //     default:
            //         HandleDefaultBWDButtonPress();
            //         break;
            // }
            bwdButtonPressed = false;
        }
        else if (fwdButtonPressed & bwdButtonPressed)
        {
            // //Reset potential pageZoom status
            // pageZoomActive = false;
            // switch (currentAppState)
            // {
            //     case AppStates.Weather:
            //         HandleWeatherAppBothPress();
            //         break;
            //     case AppStates.Graph:
            //         HandleGraphAppBothPress();
            //         break;
            //     case AppStates.Documents:
            //         HandleDocumentsAppBothPress();
            //         break;
            //     default:
            //         HandleDefaultBothButtonPress();
            //         break;
            // }
            bwdButtonPressed = false;
            fwdButtonPressed = false;
        }
        else if (rotationDirection.Equals("cw"))
        {
            //Reset potential pageZoom status
            pageZoomActive = false;

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
            //Reset potential pageZoom status
            pageZoomActive = false;
            
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


    // @TODO implement specific behaviour
    // currently changes the page of the "weather-app" until it reaches page 3 and then stops
    // in all other states causes color change of icon to "WHITE"
    public void HandleDefaultConfirmPress()
    {
        // do nothing
    }

    public void HandleDocumentsAppConfirmPress()
    {
        if(insideMenu)
        {
            insideMenu = false;
            insideAppMenu = true;
            GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0_big")); 
        } 
        else if (insideAppMenu)
        {
            switch (currentPage)
            {
                case 0:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Goethe_big")); 
                    if(bigDisplayActive)
                    {
                        gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = true;   
                    }
                    gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_11"));
                    insideAppMenu = false;
                    activeDocument = "Goethe";
                    break;
                case 1:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Liste_big")); 
                    if(bigDisplayActive)
                    {
                        gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = true;   
                    }
                    gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_11"));
                    insideAppMenu = false;
                    activeDocument = "Liste";
                    currentPage = 0;
                    break;
                case 2:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Frosch_big")); 
                    if(bigDisplayActive)
                    {
                        gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = true;   
                    }
                    gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_11"));
                    insideAppMenu = false;
                    activeDocument = "Frosch";
                    currentPage = 0;
                    break;
                case 3:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Lorem_0_big")); 
                    if(bigDisplayActive)
                    {
                        gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = true;   
                    }
                    gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_13"));
                    insideAppMenu = false;
                    activeDocument = "Lorem";
                    currentPage = 0;
                    break;
                default:
                    // do nothing 
                    break;
            } 
        }
        else if (!insideAppMenu && !insideMenu)
        {
            GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0_big"));
            gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = false; 
            insideAppMenu = true;
            activeDocument = "";
            currentPage = 0;
        }
    }

    public void HandleGraphAppConfirmPress()
    {
        // do nothing
    }

    public void HandleWeatherAppConfirmPress()
    {
        // do nothing
    }

    //
    // Forward Button Pressed Handlers
    // currently zooms into the graphs and images of the "weather-app" and goes back top normal if pressed again
    // in all other states causes color change of icon to "GREEN"
    //
    public void HandleDefaultFWDButtonPress()
    {
        // do nothing
    }

    public void HandleDocumentsAppFWDPress()
    {
        if(insideAppMenu)
        {
            switch (currentPage)
            {
                case 0:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_1_big")); 
                    currentPage = 1;
                    break;
                case 1:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_2_big")); 
                    currentPage = 2;
                    break;
                case 2:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_3_big")); 
                    currentPage = 3;
                    break;
                case 3:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0_big")); 
                    currentPage = 0;
                    break;
                default:
                    // do nothing 
                    break;
            } 
        } 
        else if(!insideMenu && !insideAppMenu)
        {
           switch (activeDocument)
            {
                 case "Lorem":
                    if(currentPage + 1 <= 3)
                    {
                        currentPage = currentPage + 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Lorem_" + currentPage + "_big"));
                    }
                    break;
                default:
                    // do nothing 
                    break;
            }  
        }
    }

    public void HandleWeatherAppFWDPress()
    {
       if(insideAppMenu && !insideMenu)
        {
            switch (currentPage)
            {
                case 1:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page2_big")); 
                    currentPage = 2;
                    break;
                case 2:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page3_big")); 
                    currentPage = 3;
                    break;
                default:
                    // do nothing 
                    break;
            }
        }
    }
    public void HandleGraphAppFWDPress()
    {
        // do nothing
    }

    //
    // Backward Button Press Handlers
    // currently goes back to the previously opened page of the "weather-app" but will not go to the title image ("first page")
    // in all other states causes color change of icon to "BLUE"
    //
    public void HandleDefaultBWDButtonPress()
    {
        // do nothing
    }

    public void HandleDocumentsAppBWDPress()
    {
        if(insideAppMenu)
        {
            switch (currentPage)
            {
                case 0:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_3_big")); 
                    currentPage = 3;
                    break;
                case 1:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0_big")); 
                    currentPage = 0;
                    break;
                case 2:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_1_big")); 
                    currentPage = 1;
                    break;
                case 3:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_2_big")); 
                    currentPage = 2;
                    break;
                default:
                    // do nothing 
                    break;
            } 
        } 
        else if(!insideMenu && !insideAppMenu)
        {
           switch (activeDocument)
            {
                case "Goethe":
                case "Liste":
                case "Frosch":
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0_big"));
                    currentPage = 0;
                    insideAppMenu = true;
                    break;
                case "Lorem":
                    if(currentPage - 1 >= 0)
                    {
                        currentPage = currentPage - 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Lorem_" + currentPage + "_big"));
                    }
                    else
                    {
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0_big"));
                        currentPage = 3;
                        insideAppMenu = true;
                    }
                    break;
                default:
                    // do nothing 
                    break;
            }  
        }
    }

    public void HandleWeatherAppBWDPress()
    {
        switch (currentPage)
        {
            case 2:
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page1_big")); 
               currentPage = 1;
               break;
            case 3:
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page2_big")); 
               currentPage = 2;
               break;
            default:
               // do nothing 
               break;
        }
    }
    public void HandleGraphAppBWDPress()
    {
        // do nothing
    }

    //
    // Both (forward and backward) Button Press Handlers
    // currently transports the viewer back to the title image ("first page") of the weather app
    // in all other states causes color change of icon to "RED"
    //
    public void HandleDefaultBothButtonPress()
    {
        // do nothing
    }

    public void HandleDocumentsAppBothPress()
    {
        if(!insideMenu)
        {
            GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_big"));
            gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = false; 
            currentPage = 0; 
            activeDocument = "";
            insideMenu = true;
            insideAppMenu = false;
        }
    }

    public void HandleWeatherAppBothPress()
    {
        switch (currentPage)
        {
            case 0:
               // do nothing
               break;
            default:
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_big"));
                currentPage = 0;
                break;
        }
    }
    public void HandleGraphAppBothPress()
    {
        // do nothing
    }

    //
    // CW Rotation Handlers
    //
    public void HandleDefaultCWRotation()
    {
        // do nothing
    }

    public void HandleDocumentsAppCWRotation()
    {
        if(insideAppMenu)
        {
            switch (currentPage)
            {
                case 0:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_1_big")); 
                    currentPage = 1;
                    break;
                case 1:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_2_big")); 
                    currentPage = 2;
                    break;
                case 2:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_3_big")); 
                    currentPage = 3;
                    break;
                case 3:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0_big")); 
                    currentPage = 0;
                    break;
                default:
                    // do nothing 
                    break;
            } 
        } 
        else if(!insideMenu && !insideAppMenu)
        {
           switch (activeDocument)
            {
                 case "Lorem":
                    if(currentPage + 1 <= 3)
                    {
                        currentPage = currentPage + 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Lorem_" + currentPage + "_big"));
                        gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_" + (currentPage + 1) + "4"));
                    }
                    break;
                default:
                    // do nothing 
                    break;
            }  
        }
    }

    public void HandleWeatherAppCWRotation()
    {
        if(insideAppMenu && !insideMenu)
        {
            switch (currentPage)
            {
                case 1:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page2_big")); 
                    currentPage = 2;
                    break;
                case 2:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page3_big")); 
                    currentPage = 3;
                    break;
                default:
                    // do nothing 
                    break;
            }
        }
    }

    public void HandleGraphAppCWRotation()
    {
        // do nothing
    }

    //
    // CCW Rotation Handlers
    //
    public void HandleDefaultCCWRotation()
    {
        // do nothing
    }

    public void HandleDocumentsAppCCWRotation()
    {
       if(insideAppMenu)
        {
            switch (currentPage)
            {
                case 0:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_3_big")); 
                    currentPage = 3;
                    break;
                case 1:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0_big")); 
                    currentPage = 0;
                    break;
                case 2:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_1_big")); 
                    currentPage = 1;
                    break;
                case 3:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_2_big")); 
                    currentPage = 2;
                    break;
                default:
                    // do nothing 
                    break;
            } 
        } 
        else if(!insideMenu && !insideAppMenu)
        {
           switch (activeDocument)
            {
                case "Lorem":
                    if(currentPage - 1 >= 0)
                    {
                        currentPage = currentPage - 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Lorem_" + currentPage + "_big"));
                        gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_" + (currentPage + 1) + "3"));
                    }
                    break;
                default:
                    // do nothing 
                    break;
            }  
        }
    }

    public void HandleWeatherAppCCWRotation()
    {
         switch (currentPage)
        {
            case 2:
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page1_big")); 
               currentPage = 1;
               break;
            case 3:
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page2_big")); 
               currentPage = 2;
               break;
            default:
               // do nothing 
               break;
        }
    }

    public void HandleGraphAppCCWRotation()
    {
        // do nothing
    }

    // this thing exist :)
    void Update()
    {
        if(currentAppInt != StateChanges.getState())
        {
           currentAppInt = StateChanges.getState(); 
           SetAppState(currentAppInt);
        }
        if(appOpened != StateChanges.getOpenedApp())
        {
           appOpened = StateChanges.getOpenedApp(); 
           SetOpenedApp(appOpened);
        }
        if(!rotationDirection.Equals(StateChanges.getRotation()))
        {
           rotationDirection = StateChanges.getRotation();
            Debug.Log("rotation received: " + rotationDirection);
            SetRotation(rotationDirection);
        }
    }
}