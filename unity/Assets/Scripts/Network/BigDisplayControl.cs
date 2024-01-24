using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMLD.MixedReality.Network;
using System;

public class BigDisplayControl : HandleSensorData
{
    public bool bigDisplayActive = false;
    public bool reset = false;
    private int s1MsgCount = 0;

    // hide Object on Start
    public override void Start()
    {
        base.Start();
        gameObject.transform.Find("BigPlane").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("BigPlane").gameObject.transform.Find("PageIndicator_big").gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.Find("LoopExtension").gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public override void HandleButtonPress()
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
}
