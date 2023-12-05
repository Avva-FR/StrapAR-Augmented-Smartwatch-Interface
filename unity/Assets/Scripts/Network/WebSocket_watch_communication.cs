using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class WebSocket_watch_communication : MonoBehaviour
{
    WebSocketServer wssv;

    void Start()
    {
        // Erstellen und Starten des WebSocket-Servers
        //wssv = new WebSocketServer("ws://192.168.2.142:12345");
        //wssv = new WebSocketServer("ws://141.76.67.226:12345");
        wssv = new WebSocketServer("ws://141.76.67.187:12345");
        wssv.AddWebSocketService<MyWebSocketService>("/MyService");
        wssv.Start();

        Debug.Log("WebSocket-Server gestartet auf ws://192.168.2.142:12345");
    }

    void OnApplicationQuit()
    {
        // Stoppen des WebSocket-Servers beim Beenden der Anwendung
        if (wssv != null)
        {
            wssv.Stop();
            Debug.Log("WebSocket-Server gestoppt");
        }
    }

    // Definition des WebSocket-Service
    class MyWebSocketService : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Debug.Log("Nachricht erhalten: " + e.Data);
            // Hier können Sie die empfangenen Daten verarbeiten
            StateChanges.SetAppState(int.Parse(e.Data));
        }
    }
}

