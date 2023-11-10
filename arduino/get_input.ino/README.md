# Documentation of the Arduino Part

## TCP_msg body

| msg size          | type                   | payload                  |
| ----------------- | ---------------------- | ------------------------ |
| byte 1-4          | byte 5                 | byte 6-n                 |
| should be 7 bytes | uint8_t in {0,1,2,3,4} | uint16_t in {0,...,1023} |


## Datapipeline

- check void loop() functionbody for details

1. Obtain Sensorinput
2. Cut out Noise 
3. remap Overreadings
4. Filter out "unintentional" Inputdata
5. SendTCP