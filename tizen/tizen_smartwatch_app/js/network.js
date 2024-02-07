// WebSocket Kommunikation
//var ws = new WebSocket('ws://192.168.2.142:12345/MyService');
//var ws = new WebSocket('ws://141.76.67.227:12345/MyService');//Dieters Laptop Uni Netz
//var ws = new WebSocket('ws://141.76.67.228:12345/MyService');// uni netzwerk
var ws = new WebSocket('ws://141.76.67.187:12345/MyService');// uni standrechner


ws.onopen = function() {
	//ws.send(0); //initiale "0" Nachricht
    console.log("Verbindung hergestellt");
    //log = "verbindung hergestellt"; 
    //aktualisiereAnzeige();
};

ws.onerror = function(error) {
    console.error('WebSocket-Fehler', error);
    //log = "WebSocket-Fehler"; 
    //aktualisiereAnzeige();
};

ws.onmessage = function(e) {
    console.log('Nachricht vom Server: ' + e.data);
    //log = "Nachricht vom Server"; 
    //aktualisiereAnzeige();
};

function sendMessage(message) {
    ws.send(message);
    //log = "Nachricht an Server"; 
    //aktualisiereAnzeige();
}



function network_reload() {
    // Schließe die bestehende WebSocket-Verbindung, falls vorhanden
    if (ws && ws.readyState === WebSocket.OPEN) {
        ws.close();
    }
    //log = "Reload";
    //aktualisiereAnzeige();
   
    // Erstelle eine neue WebSocket-Verbindung
    //var ws = new WebSocket('ws://192.168.2.142:12345/MyService');
    //ws = new WebSocket('ws://141.76.67.228:12345/MyService');// Uni netzwerk
    //ws = new WebSocket('ws://141.76.67.227:12345/MyService');// Dieters LaptopUni netzwerk
    ws = new WebSocket('ws://141.76.67.187:12345/MyService');// uni standrechner
    
    //neue seite laden (auch um initiale "0" Nachricht zu verschicken)
    window.location.href = 'index.html';
}
