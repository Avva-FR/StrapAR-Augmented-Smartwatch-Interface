using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMLD.MixedReality.Network;
using System;

public class BigDisplayControl : MonoBehaviour
{
    public bool fwdButtonPressed = false;
    public bool bwdButtonPressed = false;
    public bool bigDisplayActive = false;
    public bool reset = false;
    private int s1MsgCount = 0;

    // hide Object on Start
    void Start()
    {
        gameObject.transform.Find("BigPlane").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("BigPlane").gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("LoopExtension").gameObject.GetComponent<MeshRenderer>().enabled = false;
        NetworkServer.Instance.RegisterMessageHandler(MessageContainer.MessageType.Sensor0, HandleSensor0Data);
        NetworkServer.Instance.RegisterMessageHandler(MessageContainer.MessageType.Sensor4, HandleSensor4Data);
    }

    //
    // this sensor corresponds to fwd
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

    //
    // this sensor corresponds to bwd
    public void HandleSensor4Data(MessageContainer container)
    {
        var messageS4 = MsgBinUintS4.Unpack(container);
        uint data4 = messageS4.Data;
        Debug.Log("recv Sensor4 data: " + data4);

        bwdButtonPressed = true;
        HandleButtonPress();

    }

    public void HandleButtonPress()
    {
        if (fwdButtonPressed & !bwdButtonPressed)
        {
            if(!reset)
            {
                if (!bigDisplayActive)
                {
                    gameObject.transform.Find("BigPlane").gameObject.GetComponent<MeshRenderer>().enabled = true;
                    gameObject.transform.Find("LoopExtension").gameObject.GetComponent<MeshRenderer>().enabled = true;

                    bigDisplayActive = true;  
                }
                else
                {
                    gameObject.transform.Find("BigPlane").gameObject.GetComponent<MeshRenderer>().enabled = false;
                    gameObject.transform.Find("BigPlane").gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
                    gameObject.transform.Find("LoopExtension").gameObject.GetComponent<MeshRenderer>().enabled = false;

                    bigDisplayActive = false;
                }
            }

            fwdButtonPressed = false;
        }
        else if (bwdButtonPressed & !fwdButtonPressed)
        {
            if(!reset)
            {
                gameObject.transform.Find("BigPlane").gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.transform.Find("BigPlane").gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.transform.Find("LoopExtension").gameObject.GetComponent<MeshRenderer>().enabled = false;
                reset = true;
            }
            else
            {
                reset = false;
            }

            bwdButtonPressed = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
