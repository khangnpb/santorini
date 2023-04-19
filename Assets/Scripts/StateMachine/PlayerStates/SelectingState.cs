using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomPlayer;

public class SelectingState : State
{
    public override void EnterState(InputSystem input, Board board)
    {
        if (!board.GetActivePlayer().HasAvailableMove())
        {
            Debug.LogErrorFormat("Player {0} cannot move!", board.GetActivePlayer().GetColour().ToString());
            board.GetActivePlayer().SetHasLost(true);
        }
    }

    public override void ExitState() { return; }

    public override int UpdateState(InputSystem input, Board board)
    {
        Player activePlayer = board.GetActivePlayer();
        if (!activePlayer._isCom && !input.Mouse0ClickedOnBoard()) { return -1; }

        if (activePlayer._isCom)
        {
            List < Worker > workers = new List<Worker>();
            for (float x = -20f; x <= 20f; x += 10f)
            {
                for (float y = -20f; y <= 20f; y += 10f)
                {
                    Worker selectedWorker = board.GetNearestTileToPosition(new Vector3(x, 16f, y)).GetWorkerOnTile();
                    if (activePlayer.TrySelectWorker(selectedWorker))
                    {
                        workers.Add(selectedWorker);
                    }
                }
            }
            if ((int)workers[0].GetTile().GetLevel() >= (int)workers[1].GetTile().GetLevel())
            {
                    workers[0].EnableHighlight();
                    return (int)Player.StateId.Moving;
             }
            else
            {
                workers[1].EnableHighlight();
                return (int)Player.StateId.Moving;
            }
        }
        else
        {
            Vector3 clickedPosition =  input.GetMouse0ClickedPositionBoard();       
            Tile nearestTileToClick = board.GetNearestTileToPosition(clickedPosition);
            Worker selectedWorker = board.GetNearestTileToPosition(clickedPosition).GetWorkerOnTile();
            if (activePlayer.TrySelectWorker(selectedWorker))
            {
                selectedWorker.EnableHighlight();
                return (int)Player.StateId.Moving;
            }
        }

        return -1;
    }

    public override int GetStateId()
    {
        return (int)Player.StateId.Selecting;
    }
}
