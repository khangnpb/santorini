using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StateMachine
{
    List<State> _states = default;
    State _currentState = default;

    InputSystem _input = default;
    Board _board = default;

    public bool flag = false;

    public int counter = 0;//test

    public void Initialize(InputSystem input, Board board)
    {
        _states = new List<State>();

        _input = input;
        _board = board;
        NetworkServer.ReplaceHandler<InputMessage>(OnMessageOfServer);
        NetworkClient.ReplaceHandler<InputMessage>(OnMessage);
    }

    public void RegisterState(State state)
    {
        _states.Add(state);
    }

    public void UpdateCurrentState()
    {
        if (flag) return;
        int newStateIndex = _currentState.UpdateState(_input, _board);

        int stateTransitionCounter = 0;
        if (_input.GetMouse0ClickedThisFrame() && _input.Mouse0ClickedOnBoard())
        {
            Debug.Log("UpdateCurrentState");
            Debug.Log(_input.GetMouse0ClickedPositionScreen());
            Debug.Log(_input.GetMouse0ClickedPositionBoard());
        }
        while (newStateIndex != -1)
        {
            ++stateTransitionCounter;
            if(stateTransitionCounter > 15)
            {
                Debug.LogError("Detected possible infinite loop in state machine");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
#endif
                break;
            }

            _currentState.ExitState();
            _currentState = _states[newStateIndex];

            if (_input.GetMouse0ClickedThisFrame() && _input.Mouse0ClickedOnBoard())
            {
                if (NetworkServer.active)
                {
                    NetworkServer.SendToAll(new InputMessage 
                    { 
                        mouse0ClickedThisFrame = _input.GetMouse0ClickedThisFrame(), 
                        mouse0ClickedBoard = _input.Mouse0ClickedOnBoard(), 
                        mouse0ClickedPositionScreen = _input.GetMouse0ClickedPositionScreen(), 
                        mouse0ClickedPositionBoard = _input.GetMouse0ClickedPositionBoard() 
                    });
                    Debug.Log("Send message from Server");
                }
                else if (NetworkClient.active)
                {
                    NetworkClient.Send(new InputMessage 
                    { 
                        mouse0ClickedThisFrame = _input.GetMouse0ClickedThisFrame(), 
                        mouse0ClickedBoard = _input.Mouse0ClickedOnBoard(), 
                        mouse0ClickedPositionScreen = _input.GetMouse0ClickedPositionScreen(), 
                        mouse0ClickedPositionBoard = _input.GetMouse0ClickedPositionBoard() 
                    });
                    Debug.Log("Send message from Client");
                }
                else Debug.Log("No connection");
            }
            

            _currentState.EnterState(_input, _board);
            newStateIndex = _currentState.UpdateState(_input, _board);
        }
    }


    public void UpdateStateFromMessage(InputSystem inputFromMessage)
    {
        int newStateIndex = _currentState.UpdateState(inputFromMessage, _board);
        _currentState.ExitState();
        _currentState = _states[newStateIndex];
        _currentState.EnterState(inputFromMessage, _board);
        newStateIndex = -1;
        flag = false;
    }

    public int GetCurrentStateId()
    {
        return _currentState.GetStateId();
    }

    public void SetState(int stateId)
    {
        _currentState = _states[stateId];
    }

    void OnMessage(InputMessage inputMsg)
    {
        Debug.Log("Received message from Server");
        if (inputMsg.mouse0ClickedThisFrame && inputMsg.mouse0ClickedBoard)
        {
            Debug.Log(inputMsg.mouse0ClickedPositionScreen);
            Debug.Log(inputMsg.mouse0ClickedPositionBoard);
        }
        flag = true;
        _input.setValue(inputMsg.mouse0ClickedThisFrame, 
            inputMsg.mouse0ClickedBoard, 
            inputMsg.mouse0ClickedPositionScreen, 
            inputMsg.mouse0ClickedPositionBoard);
        //UpdateStateFromMessage(_input);
        flag = false;
    }

    void OnMessageOfServer(NetworkConnection conn, InputMessage inputMsg)
    {
        if (inputMsg.mouse0ClickedThisFrame && inputMsg.mouse0ClickedBoard)
        {
            Debug.Log(inputMsg.mouse0ClickedPositionScreen);
            Debug.Log(inputMsg.mouse0ClickedPositionBoard);
        }
        flag = true;
        _input.setValue(inputMsg.mouse0ClickedThisFrame, 
            inputMsg.mouse0ClickedBoard, 
            inputMsg.mouse0ClickedPositionScreen, 
            inputMsg.mouse0ClickedPositionBoard);
        //UpdateStateFromMessage(_input);
        flag = false;
    }
}

public struct InputMessage : NetworkMessage {
    public bool mouse0ClickedThisFrame;
    public bool mouse0ClickedBoard;
    public Vector3 mouse0ClickedPositionScreen;
    public Vector3 mouse0ClickedPositionBoard;
}