using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : Player
{
    // Computer impliment
    public void UpdatePlayer(bool activeComputer)
    {
        if (!activeComputer)
        {
            return;
        }

        _stateMachine.UpdateCurrentState();
    }

    
     
    public bool PreventsWin(Computer opponent)
    {
        return _god.PreventsWin(opponent);
    }

    public bool HasWon()
    {
        return _god.HasWon(_board, _workers);
    }

    public bool HasLost()
    {
        return _hasLost;
    }

    public void SetHasLost(bool lost)
    {
        _hasLost = lost;
    }

    public bool IsDoneTurn()
    {
        return _stateMachine.GetCurrentStateId() == (int)StateId.DoneTurn;
    }

    public bool IsWaiting()
    {
        return _stateMachine.GetCurrentStateId() == (int)StateId.Waiting;
    }

    public bool IsBuilding()
    {
        return _stateMachine.GetCurrentStateId() == (int)StateId.Building;
    }

    public bool IsWaitingOnConfirmation()
    {
        return _stateMachine.GetCurrentStateId() == (int)StateId.WaitingOnConfirmation;
    }

    public bool HasAvailableMove()
    {
        foreach (Worker worker in _workers)
        {
            List<Tile> possibleMoves = _board.GetAvailableMoves(worker);

            foreach (Tile tile in possibleMoves)
            {
                if (_god.AllowsMove(tile, worker))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void FinalizeTurn()
    {
        if (_god.DonePlacing())
        {
            _god.EnableRealTurns();
        }

        _stateMachine.SetState((int)StateId.Waiting);
    }

    public bool TrySelectWorker(Worker worker)
    {
        if (_workers.Contains(worker))
        {
            _selectedWorker = worker;
            return true;
        }

        return false;
    }

    public Worker GetSelectedWorker()
    {
        return _selectedWorker;
    }

    public God GetGod()
    {
        return _god;
    }

    public List<Worker> GetWorkers()
    {
        return _workers;
    }

    public void AddWorker(Worker worker)
    {
        _workers.Add(worker);
    }

    public Worker.Colour GetColour()
    {
        return _colour;
    }

    public Player.StateId GetCurrentState()
    {
        return (Player.StateId)_stateMachine.GetCurrentStateId();
    }
}
