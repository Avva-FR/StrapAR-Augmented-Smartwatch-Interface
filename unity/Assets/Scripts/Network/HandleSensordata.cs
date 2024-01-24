using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMLD.MixedReality.Network;
using System;

public class HandleSensorData : MonoBehaviour
{
    public bool fwdButtonPressed = false;
    //public bool fwdDoublePress = false;
    //public int fwdTapCount = 0;

    public bool confirmPressed = false;
    //public bool confirmDoublePress = false;
    //public int confirmTapCount = 0;

    public bool bwdButtonPressed = false;
    //public bool bwdDoublePress = false;
    //public int bwdTapCount = 0;

    //public float doubleTapTimer = 0.5f;
    //public bool isCoroutineRunning = false;
    

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

    /*private IEnumerator InputCoroutine(int context)
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
    }*/

    // Sensor 1 recv Handler
    public void HandleSensor0Data(MessageContainer container)
    {
        var messageS0 = MsgBinUintS0.Unpack(container);
        uint data0 = messageS0.Data;
        Debug.Log("recv Sensor0 data: " + data0);
        /*fwdTapCount++;
        if (!isCoroutineRunning)
        {
            StartCoroutine(InputCoroutine(0));
        }*/
        fwdButtonPressed = true;
        HandleButtonPress();
    }

    // Messagehandler for middle Sensor equivalent to confirm
    public void HandleSensor2Data(MessageContainer container)
    {
        // if you want to doe something with the data received
        var messageS2 = MsgBinUintS2.Unpack(container);
        uint data2 = messageS2.Data;
        Debug.Log("recv Sensor2 data: " + data2);
        /*confirmTapCount++;
        if (!isCoroutineRunning)
        {
            StartCoroutine(InputCoroutine(2));
        }*/
        confirmPressed = true;
        HandleButtonPress();
    }

    public void HandleSensor4Data(MessageContainer container)
    {
        var messageS4 = MsgBinUintS4.Unpack(container);
        uint data4 = messageS4.Data;
        Debug.Log("recv Sensor4 data: " + data4);
        /*bwdTapCount++;
        if (!isCoroutineRunning)
        {
            StartCoroutine(InputCoroutine(4));
        }*/
        bwdButtonPressed = true;
        HandleButtonPress();
    }

    /*  If you send an int from the watch representing the state like we do in the Arduinopart
     *  Call This function to set the State so, HandleConfirmButtonPress() selects the correct behaviour
     */
    public virtual void SetAppState(int state)
    {
        //@TODO
    }

    public virtual void SetRotation(string rotationDirection)
    {
        //@TODO
    }

    // opens the first page of the corresponding app
    // "enters the app-menu"
    public virtual void SetOpenedApp(string appOpened)
    {
        //@TODO
    }

    // confirm button behaviour selector
    public virtual void HandleButtonPress()
    {
        Debug.Log("Check2_Handler");
        if (confirmPressed)
        {
            GameObject.Find("SmallDisplay").gameObject.GetComponent<ChangePlaneTexture>().ExecuteConfirmPress();
            GameObject.Find("BigDisplay").gameObject.transform.Find("BigPlane").gameObject.GetComponent<ChangeBigPlaneTexture>().ExecuteConfirmPress();
            confirmPressed = false;
        }
        else if (fwdButtonPressed & !bwdButtonPressed)
        {
            GameObject.Find("SmallDisplay").gameObject.GetComponent<ChangePlaneTexture>().ExecuteFWDPress();
            GameObject.Find("BigDisplay").gameObject.GetComponent<BigDisplayControl>().ExecuteFWDPress();
            GameObject.Find("BigDisplay").gameObject.transform.Find("BigPlane").gameObject.GetComponent<ChangeBigPlaneTexture>().ExecuteFWDPress();
            fwdButtonPressed = false;
        }
        else if (bwdButtonPressed & !fwdButtonPressed)
        {
            GameObject.Find("SmallDisplay").gameObject.GetComponent<ChangePlaneTexture>().ExecuteBWDPress();
            GameObject.Find("BigDisplay").gameObject.GetComponent<BigDisplayControl>().ExecuteBWDPress();
            GameObject.Find("BigDisplay").gameObject.transform.Find("BigPlane").gameObject.GetComponent<ChangeBigPlaneTexture>().ExecuteBWDPress();
            bwdButtonPressed = false;
        }
        else if (fwdButtonPressed & bwdButtonPressed)
        {
            GameObject.Find("SmallDisplay").gameObject.GetComponent<ChangePlaneTexture>().ExecuteBothPress();
            bwdButtonPressed = false;
            fwdButtonPressed = false;
        }
    }

    // Confirm Double Taps
    // use this to exit input modalities
    public virtual void HandleWeatherAppConfirmDoublePress()
    {
        //@TODO
    }
    public virtual void HandleGraphAppConfirmDoublePress()
    {
        //@TODO
    }
    public virtual void HandleDefaultConfirmDoublePress()
    {
        //@TODO
    }
    public virtual void HandleDocumentsAppConfirmDoublePress()
    {
        //@TODO
    }
    // Forward Double Taps
    public virtual void HandleWeatherAppFWDDoublePress()
    {
        //@TODO
    }
    public virtual void HandleGraphAppFWDDoublePress()
    {
        //@TODO
    }
    public virtual void HandleDocumentsAppFWDDoublePress()
    {
        //@TODO
    }
    public virtual void HandleDefaultFWDDoubleButtonPress()
    {
        //@TODO
    }
    public virtual void HandleWeatherAppBWDDoublePress()
    {
        //@TODO
    }
    public virtual void HandleGraphAppBWDDoublePress()
    {
        //@TODO
    }
    public virtual void HandleDocumentsAppBWDDoublePress()
    {
        //@TODO
    }
    public virtual void HandleDefaultBWDButtonDoublePress()
    {
        //@TODO
    }

    // Confirm Button Press Handler
    public virtual void HandleDefaultConfirmPress()
    {
        //@TODO
    }

    public virtual void HandleDocumentsAppConfirmPress()
    {
        //@TODO
    }
    public virtual void HandleGraphAppConfirmPress()
    {
        //@TODO
    }
    public virtual void HandleWeatherAppConfirmPress()
    {
       //@TODO
    }

    //
    // Forward Button Pressed Handlers
    //
    public virtual void HandleDefaultFWDButtonPress()
    {
        //@TODO
    }

    public virtual void HandleDocumentsAppFWDPress()
    {
        //@TODO
    }

    public virtual void HandleWeatherAppFWDPress()
    {
        //@TODO
    }
    public virtual void HandleGraphAppFWDPress()
    {
        //@TODO
    }

    //
    // Backward Button Press Handlers
    //
    public virtual void HandleDefaultBWDButtonPress()
    {
        //@TODO
    }

    public virtual void HandleDocumentsAppBWDPress()
    {
        //@TODO
    }

    public virtual void HandleWeatherAppBWDPress()
    {
        //@TODO
    }
    public virtual void HandleGraphAppBWDPress()
    {
        //@TODO
    }

    //
    // Both (forward and backward) Button Press Handlers
    // leave this as is for now or implement timer based detection
    public virtual void HandleDefaultBothButtonPress()
    {
        //@TODO
    }

    public virtual void HandleDocumentsAppBothPress()
    {
         //@TODO
    }

    public virtual void HandleWeatherAppBothPress()
    {
        //@TODO
    }
    public virtual void HandleGraphAppBothPress()
    {
        //@TODO
    }

    //
    // CW Rotation Handlers
    //
    public virtual void HandleDefaultCWRotation()
    {
        //@TODO
    }

    public virtual void HandleDocumentsAppCWRotation()
    {
        //@TODO
    }

    public virtual void HandleWeatherAppCWRotation()
    {
        //@TODO
    }

    public virtual void HandleGraphAppCWRotation()
    {
        //@TODO
    }

    //
    // CCW Rotation Handlers
    //
    public virtual void HandleDefaultCCWRotation()
    {
        //@TODO
    }

    public virtual void HandleDocumentsAppCCWRotation()
    {
        //@TODO
    }

    public virtual void HandleWeatherAppCCWRotation()
    {
        //@TODO
    }
    public virtual void HandleGraphAppCCWRotation()
    {
        //@TODO
    }

    public virtual void Update()
    {
        //@TODO
    }
}
