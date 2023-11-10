# Documentation of the Arduino Part

## TCP_msg body

| msg size          | type                   | payload                  |
| ----------------- | ---------------------- | ------------------------ |
| byte 1-4          | byte 5                 | byte 6-n                 |
| should be 7 bytes | uint8_t in {0,1,2,3,4} | uint16_t in {0,...,1023} |

## How to process:

In Unity recv_handler decodes byte array.

You can differentiate the received messages by type. 0 and 4 are sensors at the side of the wrist (for and back could be functionality).

Type 1 and 3 are just there to recognize swipe gestures.

Type 2 corresponds to the sensor at the "bottom" of the bracelet.

For example: a downward swipe gesture with the right thumb could send the following messages with type 0->1->2. 



## Datapipeline

- check void loop() functionbody for details

1. Obtain Sensorinput
2. Cut out Noise 
3. remap Overreadings
4. Filter out "unintentional" Inputdata
5. SendTCP