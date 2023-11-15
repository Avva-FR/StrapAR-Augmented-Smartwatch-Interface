## Msg types
MessageTypes in unity\Assets\Scripts\Network\MessageContainer.cs

        public enum MessageType
        {
            Sensor0 = 0;
            Sensor1 = 1;
            Sensor2 = 2;
            Sensor3 = 3;
            Sensor4 = 4;
        }

\unity\Assets\Scripts\Network\Messages\MessageBinaryUInt.cs

- public static MessageContainer.MessageType Type = MessageContainer.MessageType.BINARY_UINT;

needs to be changed to, then either copy paste for the rest of the types or handle it smart

- public static MessageContainer.MessageType Type = MessageContainer.MessageType.Sensor0;

## Setting State
Whats required for this to work:

1. Register statechange on watch (e.g.: swiped to different App)
2. encode state e.g. represent as Int
3. send over tcp
4. decode to int
5. call public void SetAppState(int receivedAppState)

Buttons presses infer what function to call based on the current State, hence the need to set the state mentioned in 5. The state is represented as follows:

    // set these when AppStatechanges are received
    public enum AppStates
    {
        Default = 0,
        Calendar = 1,
        Graph = 2,
        Weather = 3
    }
    public AppStates currentAppState;


## Using button presses

see HandleButtonPress() function.

The function infers the corresponding functionto call based on current AppState and which Button was pressed.

