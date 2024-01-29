using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMLD.MixedReality.Network;
using System;


public class ChangePlaneTexture : HandleSensorData
{
    public bool pageZoomActive = false;

    public int currentPage = 0;
    public string activeDocument = "";
    public bool insideMenu = true;
    public bool insideAppMenu = false;
    public bool smallDisplayActive = true;

    public int currentAppInt = 0;
    public static string appOpened = "";
    public static string rotationDirection = "";

    public bool reset = false;

    //debug
    public Color newColor = Color.red;

    public override void Start()
    {
        base.Start();
        GetComponent<MeshRenderer>().enabled = true;
        gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().deactivateAll();
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/start"));
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
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/start"));
                gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().deactivateAll();
                insideMenu = true;
                insideAppMenu = false;
                activeDocument = "";
                currentPage = 0;
                break;
            case 1:
                currentAppState = AppStates.Weather;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather"));
                gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().deactivateAll();
                insideMenu = true;
                insideAppMenu = false;
                activeDocument = "";
                currentPage = 0;
                break;
            case 2:
                currentAppState = AppStates.Graph;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph"));
                gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().deactivateAll();
                insideMenu = true;
                insideAppMenu = false;
                activeDocument = "";
                currentPage = 0;
                break;
            case 3:
                currentAppState = AppStates.Documents;
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents"));
                gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().deactivateAll();
                insideMenu = true;
                insideAppMenu = false;
                activeDocument = "";
                currentPage = 0;
                break;
            default:
                // do nothing
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
                if(smallDisplayActive)
                {
                    gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateOne();
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
                    // do nothing
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
                // do nothing
                break;
        }
    }

    public void ExecuteFWDPress()
    {
        //Reset potential pageZoom status
        pageZoomActive = false;

        if (!reset)
        {
            if (smallDisplayActive)
            {
                GetComponent<MeshRenderer>().enabled = false;
                gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().deactivateAll();
                smallDisplayActive = false;
            }
            else
            {
                GetComponent<MeshRenderer>().enabled = true;
                if (!insideAppMenu && !insideMenu)
                {
                    gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
                else if(insideAppMenu && currentAppState.Equals(AppStates.Documents))
                {
                    switch (currentPage)
                    {
                        case 0:
                           gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateOne();
                           break;
                        case 1:
                           gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateTwo();
                           break;
                        case 2:
                           gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateThree();
                           break;
                        case 3:
                           gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateFour();
                           break;
                        default:
                            // do nothing
                            break;
                    }
                }
                smallDisplayActive = true;
            }
        }
    }

    public void ExecuteBWDPress()
    {
        //Reset potential pageZoom status
        pageZoomActive = false;

        if (!reset)
        {
            GetComponent<MeshRenderer>().enabled = false;
            gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().deactivateAll();
            smallDisplayActive = false;
            reset = true;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = true;
            if (!insideAppMenu && !insideMenu)
            {
                gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
            else if(insideAppMenu && currentAppState.Equals(AppStates.Documents))
                {
                    switch (currentPage)
                    {
                        case 0:
                           gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateOne();
                           break;
                        case 1:
                           gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateTwo();
                           break;
                        case 2:
                           gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateThree();
                           break;
                        case 3:
                           gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateFour();
                           break;
                        default:
                            // do nothing
                            break;
                    }
                }
            smallDisplayActive = true;
            reset = false;
        }
    }

    public void ExecuteBothPress()
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
                // do nothing
                break;
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
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Goethe_0"));
                    if(smallDisplayActive)
                    {
                        gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = true;   
                    }
                    gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().deactivateAll();
                    gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_13")); 
                    insideAppMenu = false;
                    activeDocument = "Goethe";
                    break;
                case 1:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Liste_0"));
                    if(smallDisplayActive)
                    {
                        gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = true;  
                    }
                    gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().deactivateAll();
                    gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_13")); 
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
                    gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().deactivateAll();
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
                    gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().deactivateAll();
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
            if(smallDisplayActive)
            {
                gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateOne(); 
            }
            gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
            insideAppMenu = true;
            activeDocument = "";
            currentPage = 0;
        }
    }

    public override void HandleGraphAppConfirmPress()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_1"));
    }

    public override void HandleWeatherAppConfirmPress()
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
    // Both (forward and backward) Button Press Handlers
    // currently transports the viewer back to the title image ("first page") of the weather app
    // in all other states causes color change of icon to "RED"
    //
    public override void HandleDocumentsAppBothPress()
    {
        if(!insideMenu)
        {
            GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents"));
            gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().deactivateAll();
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
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather"));
                insideMenu = true;
                insideAppMenu = false;
                currentPage = 0;
                break;
        }
    }
    public override void HandleGraphAppBothPress()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph"));
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
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_1")); 
                    gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateTwo();
                    if(smallDisplayActive)
                    {
                        gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateTwo();
                    }
                    currentPage = 1;
                    break;
                case 1:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_2"));
                    if(smallDisplayActive)
                    {
                        gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateThree();
                    } 
                    currentPage = 2;
                    break;
                case 2:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_3")); 
                    if(smallDisplayActive)
                    {
                        gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateFour();
                    }
                    currentPage = 3;
                    break;
                case 3:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0"));
                    if(smallDisplayActive)
                    {
                        gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateOne();
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

    public override void HandleWeatherAppCWRotation()
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
            case 3:
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page1")); 
                currentPage = 1;
                break; 
            default:
                // do nothing 
               break;
        }
    }

    public override void HandleGraphAppCWRotation()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_2"));
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
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_3"));
                    if(smallDisplayActive)
                    {
                        gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateFour();
                    }
                    currentPage = 3;
                    break;
                case 1:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_0"));
                    if(smallDisplayActive)
                    {
                        gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateOne();
                    }
                    currentPage = 0;
                    break;
                case 2:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_1"));
                    if(smallDisplayActive)
                    {
                        gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateTwo();
                    }
                    currentPage = 1;
                    break;
                case 3:
                    GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/documents_interface_2"));
                    if(smallDisplayActive)
                    {
                        gameObject.transform.Find("Selectors").gameObject.GetComponent<SelectorControl>().activateThree();
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
                case "Goethe":
                    if(currentPage - 1 >= 0)
                    {
                        currentPage = currentPage - 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Goethe_" + currentPage));
                        gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_" + (currentPage + 1) + "3"));
                    }
                    break;
                case "Liste":
                    if(currentPage - 1 >= 0)
                    {
                        currentPage = currentPage - 1;
                        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/documents_app/Liste_" + currentPage));
                        gameObject.transform.Find("PageIndicator").gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/pageIndicator/pi_" + (currentPage + 1) + "3"));
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

    public override void HandleWeatherAppCCWRotation()
    {
         switch (currentPage)
        {
            case 1:
                GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/weather_app/page3")); 
                currentPage = 3;
                break; 
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

    public override void HandleGraphAppCCWRotation()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", Resources.Load<Texture2D>( "Textures/graph_3"));
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
        if (!rotationDirection.Equals(StateChanges.getRotation1()))
        {
            rotationDirection = StateChanges.getRotation1();
            StateChanges.resetRotation1();
            SetRotation(rotationDirection);
        }
    }
}
