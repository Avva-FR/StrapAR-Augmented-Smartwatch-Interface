using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMLD.MixedReality.Network;
using System;


public class ChangeBigPlaneTexture : HandleSensorData
{

    public int currentPage = 0;
    public string activeDocument = "";
    public bool insideMenu = true;
    public bool insideAppMenu = false;
    public bool bigDisplayActive = false;

    public int currentAppInt = 0;
    public static string appOpened = "";
    public static string rotationDirection = "";

    public bool reset = false;

    //debug
    public Color newColor = Color.red;

    public override void Start()
    {
        base.Start();
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/start_big"));
        gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
    }



    /*  If you send an int from the watch representing the state like we do in the Arduinopart
     *  Call This function to set the State so, HandleConfirmButtonPress() selects the correct behaviour
     */
    public override void SetAppState(int state)
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
    public override void SetRotation(string rotationDirection)
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
    public override void SetOpenedApp(string appOpened)
    {
        switch (appOpened)
        {
            case "a1":
                Debug.Log("ChangeBigPlaneTexture");
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
    public override void HandleButtonPress()
    {
        Debug.Log("Check2_big");
        Debug.Log(currentAppState);
  
        if (rotationDirection.Equals("cw"))
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

    public void ExecuteConfirmPress()
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
    }

    public void ExecuteFWDPress()
    {
        if (!reset)
        {
            if (bigDisplayActive)
            {
                bigDisplayActive = false;
            }
            else
            {
                if (!insideAppMenu && !insideMenu)
                {
                    gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
                bigDisplayActive = true;
            }
        }
    }

    public void ExecuteBWDPress()
    {
        if (!reset)
        {
            bigDisplayActive = false;
            reset = true;
        }
        else
        {
            reset = false;
        }
    }


    // @TODO implement specific behaviour
    // currently changes the page of the "weather-app" until it reaches page 3 and then stops
    // in all other states causes color change of icon to "WHITE"
    public override void HandleDocumentsAppConfirmPress()
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

    //
    // Forward Button Pressed Handlers
    // currently zooms into the graphs and images of the "weather-app" and goes back top normal if pressed again
    // in all other states causes color change of icon to "GREEN"
    //
    public override void HandleDocumentsAppFWDPress()
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

    public override void HandleWeatherAppFWDPress()
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


    //
    // Backward Button Press Handlers
    // currently goes back to the previously opened page of the "weather-app" but will not go to the title image ("first page")
    // in all other states causes color change of icon to "BLUE"
    //
    public override void HandleDocumentsAppBWDPress()
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

    public override void HandleWeatherAppBWDPress()
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

    //
    // Both (forward and backward) Button Press Handlers
    // currently transports the viewer back to the title image ("first page") of the weather app
    // in all other states causes color change of icon to "RED"
    //
    public override void HandleDocumentsAppBothPress()
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

    public override void HandleWeatherAppBothPress()
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

    //
    // CW Rotation Handlers
    //
    public override void HandleDocumentsAppCWRotation()
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

    public override void HandleWeatherAppCWRotation()
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

    //
    // CCW Rotation Handlers
    //
    public override void HandleDocumentsAppCCWRotation()
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

    public override void HandleWeatherAppCCWRotation()
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

    public override void Update()
    {
        if (currentAppInt != StateChanges.getState())
        {
            currentAppInt = StateChanges.getState();
            SetAppState(currentAppInt);
        }
        if (!appOpened.Equals(StateChanges.getOpenedApp()))
        {
            appOpened = StateChanges.getOpenedApp();
            SetOpenedApp(appOpened);
        }
        if (!rotationDirection.Equals(StateChanges.getRotation2()))
        {
            Debug.Log(rotationDirection);
            rotationDirection = StateChanges.getRotation2();
            StateChanges.resetRotation2();
            SetRotation(rotationDirection);
        }
    }
}