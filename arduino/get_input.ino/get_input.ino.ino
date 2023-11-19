#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>
#include "wifi.h"
#include <Adafruit_ADS1X15.h>

// func signature
void sendTCP_MSG_uint16(uint8_t type, uint16_t valueToSend, boolean debug=false);
uint16_t clipRange(uint16_t adc_reading);

// 12 bit ADS
Adafruit_ADS1015 ads;

// WiFi
WiFiClient client;
ESP8266WiFiMulti WiFiMulti;
const char* ssid = STASSID;
const char* password = STAPSK;
// home
//const char* host = "192.168.178.30";
const char* host = "192.168.0.105";
const int16_t port = 11337;

uint16_t val_array[5];
bool sensor_pressed[5] = {false};
// double tap window currently at 0.5 s
unsigned long double_tap_timer = 500;
unsigned long last_release_time[5] = {0};
uint16_t pressure_treshhold = 700;


void setup() {
  // Setup baudRate
  Serial.begin(9600);
  delay(200);

  // check for ADS init
  if (!ads.begin()) {
    Serial.println("Failed to initialize ADS.");
    while (1);
  }
  
  /* 
  //Setup connection
  WiFi.mode(WIFI_STA);
  WiFiMulti.addAP(ssid, password);
  
  // debug infos
  Serial.println();
  Serial.print("Try to connect to ");
  Serial.print(ssid);
  Serial.print(" WiFi...");

  while (WiFiMulti.run() != WL_CONNECTED) {
    Serial.print(".");
    delay(500);
  }

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("------");

  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());

  delay(500);
  Serial.println("------");

  while (!client.connect(host, port)) {
    Serial.println("TCP connection failed");
    Serial.println("wait 5 sec...");
    delay(5000);
  }

  Serial.println("TCP connection successful");
}
*/
  
}

void loop() {
  // put your main code here, to run repeatedly:
  uint16_t val_piezo0 = analogRead(A0);
  uint16_t val_piezo1 = ads.readADC_SingleEnded(0);
  uint16_t val_piezo2 = ads.readADC_SingleEnded(1);
  uint16_t val_piezo3 = ads.readADC_SingleEnded(2);
  uint16_t val_piezo4 = ads.readADC_SingleEnded(3);
  
  /*  
   * cut out false readings, on ADSnarrow down range
   * note that val_piezo0 is measured at the ESP A0 Pin and does not display behaviour that would require clipping or remapping
   */
  val_array[0] = val_piezo0;
  val_array[1] = map(clipRange(val_piezo1), 0, 1100, 0, 1023);
  val_array[2] = map(clipRange(val_piezo2), 0, 1100, 0, 1023);
  val_array[3] = map(clipRange(val_piezo3), 0, 1100, 0, 1023);
  val_array[4] = map(clipRange(val_piezo4), 0, 1100, 0, 1023);
  
  /*
   * only send touchevents
   * type {0,...,4} corresponds to sensor values 0 to 4
   */
  for (int i = 0; i < 5; i++) {
    if (val_array[i] > pressure_treshhold && !sensor_pressed[i]) {
      sensor_pressed[i] = true;
    } 
    // send msg on release of the touch event
    else if (val_array[i] <= pressure_treshhold && sensor_pressed[i])
    {
      unsigned long current_time = millis();
      // send static value of 100 for a double tap event
      if (current_time - last_release_time[i] <= double_tap_timer) 
      {
        sendTCP_MSG_uint16(i, 100, true);    
        
      } else {
      
      sendTCP_MSG_uint16(i, val_array[i], true);
      sensor_pressed[i] = false;
    }
  }
}


/* values read from the ads have a tendency to be 0 or 2**16
 * this function is a preprocessing step to cutout random values of 2**16
 * call this function before map
*/
uint16_t clipRange(uint16_t adc_reading) {
  return (adc_reading > 200 && adc_reading < 1023) ? adc_reading : 0;
}

void sendTCP_MSG_uint16(uint8_t type, uint16_t valueToSend, boolean debug){

  // Define the single sizes in bytes of the TCP message
  uint8_t msgLengthInBytes_MsgSize = 4;                    // byte #1-#4: length definition in bytes of the TCP size (32bit integer per specificaion)
  uint8_t msgLengthInBytes_Type = 1;                       // byte #5: length definition in bytes of the TCP type (8 bit integer per specificaion)
  uint32_t msgLengthInBytes_Payload = sizeof(valueToSend); // byte #6-#n: length definition in bytes of the TCP payload (e.g., 4 bytes for a 32bit data type,)  

  // Calculate the overall length of the TCP message (4 byte message size + 1 byte type + n bytes payload)
  uint32_t msgLengthInBytes_Complete=msgLengthInBytes_MsgSize+msgLengthInBytes_Type+msgLengthInBytes_Payload;

  // Create byte array
  byte msgData[msgLengthInBytes_Complete];

  // Message length in bytes of the payload
  msgData[0] =  msgLengthInBytes_Payload;
  msgData[1] =  msgLengthInBytes_Payload >> 8;
  msgData[2] =  msgLengthInBytes_Payload >> 16;
  msgData[3] =  msgLengthInBytes_Payload >> 24;

  // Type
  msgData[4] = (uint8_t) type;

  // Payload
  msgData[5] = valueToSend;
  msgData[6] = valueToSend >> 8;

  // Send 
  client.write(msgData, msgLengthInBytes_Complete);

  // Debug
  if(debug){
    Serial.println("-------------------------");
    Serial.println("sendTCP_MSG_uint16");
    for(int i = 0; i < msgLengthInBytes_Complete; i++){
      char byteID[5];
      sprintf(byteID, "%03d", i+1);
      Serial.print("Byte: #");
      Serial.print(byteID);
      Serial.print(" ");
      Serial.print("Value: ");
      Serial.println(msgData[i]);
    }
    Serial.println("-------------------------");    
  }
}
