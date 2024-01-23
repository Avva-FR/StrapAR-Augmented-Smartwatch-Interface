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
    public static string appOpened = "";
    public static string rotationDirection = "";
    public string activeDocument = "";
    public bool insideMenu = true;
    public bool insideAppMenu = false;
    public bool smallDisplayActive = true;

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
        GetComponent<MeshRenderer>().enabled = true;
        gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/start"));
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
        Debug.Log("check1");
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
                gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
                insideMenu = true;
                insideAppMenu = false;
                activeDocument = "";
                currentPage = 0;
                break;
            case 1:
                currentAppState = AppStates.Weather;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather"));
                gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
                insideMenu = true;
                insideAppMenu = false;
                activeDocument = "";
                currentPage = 0;
                break;
            case 2:
                currentAppState = AppStates.Graph;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph"));
                gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
                insideMenu = true;
                insideAppMenu = false;
                activeDocument = "";
                currentPage = 0;
                break;
            case 3:
                currentAppState = AppStates.Documents;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents"));
                gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
                insideMenu = true;
                insideAppMenu = false;
                activeDocument = "";
                currentPage = 0;
                break;
            default:
                Debug.Log("keine Standart Seite");
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
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page1"));
                currentPage = 1;
                insideMenu = false;
                insideAppMenu = true;
                break;
            case "a2":
                currentAppState = AppStates.Graph;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_0"));
                insideMenu = false;
                insideAppMenu = true;

                break;
            case "a3":
                currentAppState = AppStates.Documents;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0"));
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
        Debug.Log("Check2");
        Debug.Log(currentAppState);
        if (confirmPressed)
        {
            Debug.Log("Check3");
            Debug.Log(currentAppState);
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
            //Reset potential pageZoom status
            pageZoomActive = false;

            if(!reset)
            {
                if(smallDisplayActive)
                {
                    GetComponent<MeshRenderer>().enabled = false;
                    gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
                    smallDisplayActive = false;  
                }
                else
                {
                    GetComponent<MeshRenderer>().enabled = true;
                    if(!insideAppMenu && !insideMenu)
                    {
                        gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = true;
                    }
                    smallDisplayActive = true;  
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
            //Reset potential pageZoom status
            pageZoomActive = false;

            if(!reset)
            {
                GetComponent<MeshRenderer>().enabled = false;
                gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
                smallDisplayActive = false; 
                reset = true;
            }
            else
            {
                GetComponent<MeshRenderer>().enabled = true;
                if(!insideAppMenu && !insideMenu)
                {
                    gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
                smallDisplayActive = true;  
                reset = false;
            }

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
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/start_0"));
    }

    public void HandleDocumentsAppConfirmPress()
    {
        if (insideAppMenu)
        {
            switch (currentPage)
            {
                case 0:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Goethe_0"));
                    if(smallDisplayActive)
                    {
                        gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = true;   
                    }
                    gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_14")); 
                    insideAppMenu = false;
                    activeDocument = "Goethe";
                    break;
                case 1:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Liste_0"));
                    if(smallDisplayActive)
                    {
                        gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = true;   
                    }
                    gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_14")); 
                    insideAppMenu = false;
                    activeDocument = "Liste";
                    currentPage = 0;
                    break;
                case 2:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Frosch"));
                    if(smallDisplayActive)
                    {
                        gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = true;   
                    }
                    gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_11")); 
                    insideAppMenu = false;
                    activeDocument = "Frosch";
                    currentPage = 0;
                    break;
                case 3:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Lorem_0"));
                    if(smallDisplayActive)
                    {
                        gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = true;   
                    }
                    gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_118")); 
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
            GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0"));
            gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
            insideAppMenu = true;
            activeDocument = "";
            currentPage = 0;
        }
        Debug.Log("insideAppMenu:" + insideAppMenu);
        Debug.Log("insideMenu:" + insideMenu);
    }
    public void HandleGraphAppConfirmPress()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_0"));
    }
    public void HandleWeatherAppConfirmPress()
    {
       switch (currentPage)
        {
            case 1:
                if(pageZoomActive)
                {
                   GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page1")); 
                   pageZoomActive = false;
                }
                else
                {
                   GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page1_zoom"));
                   pageZoomActive = true; 
                }
               break;
            case 2:
               if(pageZoomActive)
                {
                   GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page2")); 
                   pageZoomActive = false;
                }
                else
                {
                   GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page2_zoom"));
                   pageZoomActive = true; 
                }
               break;
            case 3:
               if(pageZoomActive)
                {
                   GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page3")); 
                   pageZoomActive = false;
                }
                else
                {
                   GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page3_zoom"));
                   pageZoomActive = true; 
                }
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
        if(insideAppMenu)
        {
            switch (currentPage)
            {
                case 0:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_1")); 
                    currentPage = 1;
                    break;
                case 1:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_2")); 
                    currentPage = 2;
                    break;
                case 2:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_3")); 
                    currentPage = 3;
                    break;
                case 3:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0")); 
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
                case "Goethe":
                    if(currentPage + 1 <= 2)
                    {
                        currentPage = currentPage + 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Goethe_" + currentPage));
                    }
                    break;
                case "Liste":
                    if(currentPage + 1 <= 2)
                    {
                        currentPage = currentPage + 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Liste_" + currentPage));
                    }
                    break;
                 case "Lorem":
                    if(currentPage + 1 <= 17)
                    {
                        currentPage = currentPage + 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Lorem_" + currentPage));
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
        switch (currentPage)
        {
            case 1:
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page2")); 
               currentPage = 2;
               break;
            case 2:
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page3")); 
               currentPage = 3;
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
        if(insideAppMenu)
        {
            switch (currentPage)
            {
                case 0:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_3")); 
                    currentPage = 3;
                    break;
                case 1:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0")); 
                    currentPage = 0;
                    break;
                case 2:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_1")); 
                    currentPage = 1;
                    break;
                case 3:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_2")); 
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
                    if(currentPage - 1 >= 0)
                    {
                        currentPage = currentPage - 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Goethe_" + currentPage));
                    }
                    else
                    {
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0"));
                        currentPage = 0;
                        insideAppMenu = true;
                    }
                    break;
                case "Liste":
                    if(currentPage - 1 >= 0)
                    {
                        currentPage = currentPage - 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Liste_" + currentPage));
                    }
                    else
                    {
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_1"));
                        currentPage = 1;
                        insideAppMenu = true;
                    }
                    break;
                case "Frosch":
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_2"));
                    currentPage = 2;
                    insideAppMenu = true;
                    break;
                case "Lorem":
                    if(currentPage - 1 >= 0)
                    {
                        currentPage = currentPage - 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Lorem_" + currentPage));
                    }
                    else
                    {
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_3"));
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
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page1")); 
               currentPage = 1;
               break;
            case 3:
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page2")); 
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
        if(!insideMenu)
        {
            GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents"));
            gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
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
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather"));
                insideMenu = true;
                insideAppMenu = false;
                currentPage = 0;
                break;
        }
    }
    public void HandleGraphAppBothPress()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_3"));
    }

    //
    // CW Rotation Handlers
    //
    public void HandleDefaultCWRotation()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/start_1"));
    }

    public void HandleDocumentsAppCWRotation()
    {
        if(insideAppMenu)
        {
            switch (currentPage)
            {
                case 0:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_1")); 
                    currentPage = 1;
                    break;
                case 1:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_2")); 
                    currentPage = 2;
                    break;
                case 2:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_3")); 
                    currentPage = 3;
                    break;
                case 3:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0")); 
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
                case "Goethe":
                    if(currentPage + 1 <= 2)
                    {
                        currentPage = currentPage + 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Goethe_" + currentPage));
                        gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_" + (currentPage + 1) + "3"));
                    }
                    break;
                case "Liste":
                    if(currentPage + 1 <= 2)
                    {
                        currentPage = currentPage + 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Liste_" + currentPage));
                        gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_" + (currentPage + 1) + "3"));
                    }
                    break;
                 case "Lorem":
                    if(currentPage + 1 <= 17)
                    {
                        currentPage = currentPage + 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Lorem_" + currentPage));
                        gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_" + (currentPage + 1) + "18"));
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
        switch (currentPage)
        {
            case 1:
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page2")); 
               currentPage = 2;
                Debug.Log("case 1");
               break;
            case 2:
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page3")); 
               currentPage = 3;
                Debug.Log("case 2");
               break;
            default:
                // do nothing 
                Debug.Log("currentPage nicht richtig gesetzt!");
                Debug.Log(currentPage);
               break;
        }
    }

    public void HandleGraphAppCWRotation()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_1"));
    }

    //
    // CCW Rotation Handlers
    //
    public void HandleDefaultCCWRotation()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/start_2"));
    }

    public void HandleDocumentsAppCCWRotation()
    {
        if(insideAppMenu)
        {
            switch (currentPage)
            {
                case 0:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_3")); 
                    currentPage = 3;
                    break;
                case 1:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0")); 
                    currentPage = 0;
                    break;
                case 2:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_1")); 
                    currentPage = 1;
                    break;
                case 3:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_2")); 
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
                    if(currentPage - 1 >= 0)
                    {
                        currentPage = currentPage - 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Goethe_" + currentPage));
                        gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_" + (currentPage + 1) + "4"));
                    }
                    break;
                case "Liste":
                    if(currentPage - 1 >= 0)
                    {
                        currentPage = currentPage - 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Liste_" + currentPage));
                        gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_" + (currentPage + 1) + "4"));
                    }
                    break;
                case "Lorem":
                    if(currentPage - 1 >= 0)
                    {
                        currentPage = currentPage - 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Lorem_" + currentPage));
                        gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_" + (currentPage + 1) + "18"));
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
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page1")); 
               currentPage = 1;
               break;
            case 3:
               GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page2"));
               currentPage = 2;
               break;
            default:
               // do nothing 
               break;
        }
    }

    public void HandleGraphAppCCWRotation()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_2"));
    }

    // this thing exist :)
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
