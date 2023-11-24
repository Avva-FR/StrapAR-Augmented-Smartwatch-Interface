//Global section
// Global log
var log = "empty";

function aktualisiereAnzeige() {
    document.getElementById("logs").innerText = log;
}

// WebSocket Kommunikation
var ws = new WebSocket('ws://192.168.2.142:12345/MyService');

ws.onopen = function() {
    console.log("Verbindung hergestellt");
    log = "verbindung hergestellt"; 
    aktualisiereAnzeige();
};

ws.onerror = function(error) {
    console.error('WebSocket-Fehler', error);
    log = "WebSocket-Fehler"; 
    aktualisiereAnzeige();
};

ws.onmessage = function(e) {
    console.log('Nachricht vom Server: ' + e.data);
    log = "Nachricht vom Server"; 
    aktualisiereAnzeige();
};

function sendMessage(message) {
    ws.send(message);
    log = "Nachricht an Server"; 
    aktualisiereAnzeige();
}







/*
 // HTTP Nachrichten
//Global Send Method
//var serverURL = 'http://192.168.2.142:12345/message';//privat
//var serverURL = 'http://192.168.2.142:12345/message';//Uni_laptop
var serverURL = 'http://141.76.67.228:12345/message';//Uni_standrechner deadl1ne13



function sendMessage(message) {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", serverURL, true);
    xhr.setRequestHeader('Content-Type', 'text/plain');

    xhr.onreadystatechange = function() {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status === 200) {
                console.log('Antwort vom Server: ' + this.responseText);
            } else {
                console.error('Fehler beim Senden der Nachricht: ' + this.status);
            }
        }
    }

    xhr.onerror = function(e) {
        console.error('Fehler bei der Netzwerkkommunikation', e);
    };

    xhr.send(message);
}
*/

//Hallo Nachricht und logs
/*
(function() {   
    log = 'network aufgerufen';
    aktualisiereAnzeige();

    sendMessage('Hallo Server');

	// Logs Aktuallisieren

}());
*/