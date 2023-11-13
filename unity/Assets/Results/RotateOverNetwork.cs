using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMLD.MixedReality.Network;
using System;

public class RotateOverNetwork : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkServer.Instance.RegisterMessageHandler(MessageContainer.MessageType.BINARY_UINT, HandleIntData);

        NetworkServer.Instance.RegisterMessageHandler(MessageContainer.MessageType.JSON_DICTIONARY, HandleJsonData);
    }

    private void HandleJsonData(MessageContainer container)
    {
        Debug.Log(MessageJsonDictionary.Unpack(container).Data["mySensor"]);
    }

    private void HandleIntData(MessageContainer container)
    {
        var message = MessageBinaryUInt.Unpack(container);
        float angle = (message.Data / 1024f) * 360;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
