using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomPlayer;
using Mirror;

public struct InputMessage : NetworkMessage {
    public bool mouse0ClickedThisFrame;
    public bool mouse0ClickedBoard;
    public Vector3 mouse0ClickedPositionScreen;
    public Vector3 mouse0ClickedPositionBoard;

    public bool clickEndTurn;
}
public class Santorini : MonoBehaviour
{
    [SerializeField]
    Board _board = default;

    [SerializeField]
    PlayerCamera _camera = default;

    [SerializeField]
    InputSystem _input = default;

    [System.SerializableAttribute]
    struct SetPlayer
    {
#pragma warning disable 0649
        public Worker.Colour colour;       
        public bool non_player;
#pragma warning restore 0649
    }

    [Header("Set Player")]
    [SerializeField]
    List<SetPlayer> _PLAYERS = default;


    List<Player> _players = default;
    Player _activePlayer = null;

    public int turn = 0;//test
    public bool flag = false;//Dùng để check xem nút end turn được bấm bởi người chơi hay bởi message từ server or client
    
    void Start()
    {
        _players = new List<Player>();

        for (int i =0; i < _PLAYERS.Count; i++)
        {
            _players.Add(new Player());
            if (_PLAYERS[i].non_player)
            {
                _players[i].Initialize(_input, _board, _PLAYERS[i].colour,true);
            }
            else
            {
                _players[i].Initialize(_input, _board, _PLAYERS[i].colour,false);
            };
        }
        _activePlayer = _players[0];
        _board.OnStart(_players);

        NetworkServer.ReplaceHandler<InputMessage>(OnMessageOfServer);
        NetworkClient.ReplaceHandler<InputMessage>(OnMessage);
    }

    void Update()
    {
        try
        {
            _input.OnUpdate();
            _board.OnUpdate(_activePlayer);
            _camera.OnUpdate(_input.Mouse1Clicked(), _input.GetMouseScrollDeltaY(), _board.transform);
            
            //check win or lost
            foreach (Player player in _players)
            {
                if(player.HasWon())
                {
                    bool canWin = true;
                    foreach (Player otherPlayer in _players)
                    {
                        if(otherPlayer.PreventsWin(player))
                        {
                            canWin = false;
                            break;
                        }
                    }

                    if (canWin)
                    {
                        Debug.Log("You Win!!!");
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.ExitPlaymode();
#endif
                    }
                    else
                    {
                        Debug.Log("An opponent prevented your win! D:");
                    }
                }

                if(player.HasLost())
                {
                    Debug.Log("You lose!!!");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.ExitPlaymode();
#endif
                }
            }


            foreach (Player player in _players)
            {   
                if(_input.GetMouse0ClickedThisFrame() && _input.Mouse0ClickedOnBoard())
                {
                    Debug.Log("Mouse0ClickedThisFrame");
                    if (NetworkServer.active)
                    {
                        NetworkServer.SendToAll(new InputMessage 
                        { 
                            mouse0ClickedThisFrame = _input.GetMouse0ClickedThisFrame(), 
                            mouse0ClickedBoard = _input.Mouse0ClickedOnBoard(), 
                            mouse0ClickedPositionScreen = _input.GetMouse0ClickedPositionScreen(), 
                            mouse0ClickedPositionBoard = _input.GetMouse0ClickedPositionBoard(),
                            clickEndTurn = false
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
                            mouse0ClickedPositionBoard = _input.GetMouse0ClickedPositionBoard(),
                            clickEndTurn = false
                        });
                        Debug.Log("Send message from Client");
                    }
                    else Debug.Log("No connection");
                }
                player.UpdatePlayer(player == _activePlayer);
            }

            if (_activePlayer.IsDoneTurn())
            {
                if(!flag)
                if (NetworkServer.active)
                {
                    NetworkServer.SendToAll(new InputMessage { clickEndTurn = true });
                }
                else if (NetworkClient.active)
                {
                    NetworkClient.Send(new InputMessage { clickEndTurn = true });
                }
                else Debug.Log("No connection");

                _activePlayer.FinalizeTurn();
                _activePlayer = GetNextPlayer();
                flag = false;
                Debug.Log("End turn!");
                turn++;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Break();
        }
    }

    void OnMessage(InputMessage inputMsg)
    {
        if(NetworkServer.active) return;
        if(inputMsg.clickEndTurn) 
        {
            GameObject buttonObject = GameObject.Find("End Turn Button");
            if (buttonObject != null)
            {
                flag = true;
                buttonObject.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
            }
            else Debug.Log("Button not found");
            return;
        }
        Debug.Log("Received message from Server");
        _input.setValue(inputMsg.mouse0ClickedThisFrame, 
            inputMsg.mouse0ClickedBoard, 
            inputMsg.mouse0ClickedPositionScreen, 
            inputMsg.mouse0ClickedPositionBoard);
        foreach (Player player in _players)
        {
            if(player.HasWon())
            {
                bool canWin = true;
                foreach (Player otherPlayer in _players)
                {
                    if(otherPlayer.PreventsWin(player))
                    {
                        canWin = false;
                        break;
                    }
                }

                if (canWin)
                {
                    Debug.Log("You Win!!!");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.ExitPlaymode();
#endif
                }
                else
                {
                    Debug.Log("An opponent prevented your win! D:");
                }
            }

            if(player.HasLost())
            {
                Debug.Log("You lose!!!");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
#endif
            }
        }
        foreach (Player player in _players)
            player.UpdatePlayer(player == _activePlayer);
        if (_activePlayer.IsDoneTurn())
        {
            _activePlayer.FinalizeTurn();
            _activePlayer = GetNextPlayer();
            Debug.Log("Turn: " + turn.ToString());
            turn++;
        }
        _input.reset();
    }

    void OnMessageOfServer(NetworkConnection conn, InputMessage inputMsg)
    {
        if(inputMsg.clickEndTurn) 
        {
            GameObject buttonObject = GameObject.Find("End Turn Button");
            if (buttonObject != null)
            {
                flag = true;
                buttonObject.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
            }
            else Debug.Log("Button not found");
            return;
        }
        Debug.Log("Received message from Server");
        _input.setValue(inputMsg.mouse0ClickedThisFrame, 
            inputMsg.mouse0ClickedBoard, 
            inputMsg.mouse0ClickedPositionScreen, 
            inputMsg.mouse0ClickedPositionBoard);
        foreach (Player player in _players)
        {
            if(player.HasWon())
            {
                bool canWin = true;
                foreach (Player otherPlayer in _players)
                {
                    if(otherPlayer.PreventsWin(player))
                    {
                        canWin = false;
                        break;
                    }
                }

                if (canWin)
                {
                    Debug.Log("You Win!!!");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.ExitPlaymode();
#endif
                }
                else
                {
                    Debug.Log("An opponent prevented your win! D:");
                }
            }

            if(player.HasLost())
            {
                Debug.Log("You lose!!!");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
#endif
            }
        }
        foreach (Player player in _players)
            player.UpdatePlayer(player == _activePlayer);
        if (_activePlayer.IsDoneTurn())
        {
            _activePlayer.FinalizeTurn();
            _activePlayer = GetNextPlayer();
            Debug.Log("Turn: " + turn.ToString());
            turn++;
        }
        _input.reset();
    }
    
    Player GetNextPlayer()
    {
        //TODO: Any players that haven't placed all their workers should be prioritized

        for(int i =0 ; i < _players.Count; i++)
        {
            if (_players[i] == _activePlayer)
            {
                return _players[(i+1)% _players.Count];
            }
        }
        return _players[0];
    }
}
