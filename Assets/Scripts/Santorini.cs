using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santorini : MonoBehaviour
{
    [SerializeField]
    Board _board = default;

    [SerializeField]
    PlayerCamera _camera = default;

    [SerializeField]
    InputSystem _input = default;

    List<Player> _players = default;
    Player _activePlayer = null;

    void Start()
    {
        _players = new List<Player>() { new Player(), new Player() };
        _activePlayer = _players[0];

        _players[0].Initialize(_input, _board, Worker.Colour.Blue);
        _players[1].Initialize(_input, _board, Worker.Colour.White);

        _board.OnStart(_players);
    }

    void Update()
    {
        try
        {
            _input.OnUpdate();
            _board.OnUpdate(_activePlayer);
            _camera.OnUpdate(_input.Mouse1Clicked(), _input.GetMouseScrollDeltaY(), _board.transform);
            
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
                player.UpdatePlayer(player == _activePlayer);
            }

            if (_activePlayer.IsDoneTurn())
            {
                _activePlayer.FinalizeTurn();
                _activePlayer = GetNextPlayer();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            Debug.Break();
        }
    }
    
    Player GetNextPlayer()
    {
        //TODO: Any players that haven't placed all their workers should be prioritized

        if (_players[0] == _activePlayer)
        {
            return _players[1];
        }

        return _players[0];
    }
}