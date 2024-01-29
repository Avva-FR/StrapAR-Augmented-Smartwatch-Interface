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
        gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().deactivateAll(); 
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
                gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().deactivateAll(); 
                insideMenu = true;
                insideAppMenu = false;
                activeDocument = "";
                currentPage = 0;
                break;
            case 1:
                currentAppState = AppStates.Weather;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_big"));
                gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = false; 
                gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().deactivateAll(); 
                insideMenu = true;
                insideAppMenu = false;
                activeDocument = "";
                currentPage = 0;
                break;
            case 2:
                currentAppState = AppStates.Graph;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_big"));
                gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = false; 
                gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().deactivateAll(); 
                insideMenu = true;
                insideAppMenu = false;
                activeDocument = "";
                currentPage = 0;
                break;
            case 3:
                currentAppState = AppStates.Documents;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_big"));
                gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().deactivateAll();  
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
                currentAppState = AppStates.Weather;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page1_big"));
                currentPage = 1;
                insideMenu = false;
                insideAppMenu = true;
                break;
            case "a2":
                currentAppState = AppStates.Graph;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_big"));
                insideMenu = false;
                insideAppMenu = true;
                break;
            case "a3":
                currentAppState = AppStates.Documents;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0_big"));
                if(bigDisplayActive)
                {
                    gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().activateOne();
                }
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
        if (rotationDirection.Equals("cw"))
        {
            switch (currentAppState)
            {
                case AppStates.Weather:
                    HandleWeatherAppCWRotation();
                    break;
                case AppStates.Documents:
                    HandleDocumentsAppCWRotation();
                    break;
                case AppStates.Graph:
                    HandleGraphAppCWRotation();
                    break;
                default:
                    // do nothing
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
                case AppStates.Documents:
                    HandleDocumentsAppCCWRotation();
                    break;
                case AppStates.Graph:
                    HandleGraphAppCCWRotation();
                    break;
                default:
                    // do nothing
                    break;
            }
            rotationDirection = "";
        }
    }

    public void ExecuteConfirmPress()
    {
        switch (currentAppState)
        {
            case AppStates.Documents:
                HandleDocumentsAppConfirmPress();
                break;
            case AppStates.Graph:
                HandleGraphAppConfirmPress();
                break;
            default:
                // do nothing
                break;
        }
    }

    public void ExecuteBothPress()
    {
        switch (currentAppState)
        {
            case AppStates.Weather:
                HandleWeatherAppBothPress();
                break;
            case AppStates.Documents:
                HandleDocumentsAppBothPress();
                break;
            case AppStates.Graph:
                HandleGraphAppBothPress();
                break;
            default:
                // do nothing
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
                else if(insideAppMenu && currentAppState.Equals(AppStates.Documents))
                {
                    switch (currentPage)
                    {
                        case 0:
                           gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().activateOne();
                           break;
                        case 1:
                           gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().activateTwo();
                           break;
                        case 2:
                           gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().activateThree();
                           break;
                        case 3:
                           gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().activateFour();
                           break;
                        default:
                            // do nothing
                            break;
                    }
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
        if (insideAppMenu)
        {
            switch (currentPage)
            {
                case 0:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Goethe_big")); 
                    if(bigDisplayActive)
                    {
                        gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = true;   
                    }
                    gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().deactivateAll(); 
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
                    gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().deactivateAll(); 
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
                    gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().deactivateAll(); 
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
                    gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().deactivateAll(); 
                    gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_14"));
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
            if(bigDisplayActive)
            {
                gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().activateOne();
            } 
            insideAppMenu = true;
            activeDocument = "";
            currentPage = 0;
        }
    }

    public override void HandleGraphAppConfirmPress()
    {
       GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_1_big")); 
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
            gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().deactivateAll(); 
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

    public override void HandleGraphAppBothPress()
    {
       GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_big")); 
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
                    if(bigDisplayActive)
                    {
                        gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().activateTwo();
                    }
                    currentPage = 1;
                    break;
                case 1:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_2_big"));
                    if(bigDisplayActive)
                    {
                        gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().activateThree();
                    } 
                    currentPage = 2;
                    break;
                case 2:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_3_big"));
                    if(bigDisplayActive)
                    {
                        gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().activateFour();
                    } 
                    currentPage = 3;
                    break;
                case 3:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0_big")); 
                    if(bigDisplayActive)
                    {
                        gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().activateOne();
                    }
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
                case 3:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page1_big")); 
                    currentPage = 1;
                    break; 
                default:
                    // do nothing 
                    break;
            }
        }
    }

    public override void HandleGraphAppCWRotation()
    {
       GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_2_big")); 
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
                    if(bigDisplayActive)
                    {
                        gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().activateFour();
                    }
                    currentPage = 3;
                    break;
                case 1:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0_big")); 
                    if(bigDisplayActive)
                    {
                        gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().activateOne();
                    }
                    currentPage = 0;
                    break;
                case 2:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_1_big"));
                    if(bigDisplayActive)
                    {
                        gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().activateTwo();
                    } 
                    currentPage = 1;
                    break;
                case 3:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_2_big"));
                    if(bigDisplayActive)
                    {
                        gameObject.transform.Find("Selectors_big").gameObject.GetComponent<BigSelectorControl>().activateThree();
                    }
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
                        gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_" + (currentPage + 1) + "4"));
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
            case 1:
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page3_big")); 
                currentPage = 3;
                break; 
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

    public override void HandleGraphAppCCWRotation()
    {
       GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_3_big")); 
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
            rotationDirection = StateChanges.getRotation2();
            StateChanges.resetRotation2();
            SetRotation(rotationDirection);
        }
    }
}