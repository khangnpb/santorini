using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(!input.Mouse0ClickedOnBoard()) { return -1; }

        Vector3 clickedPosition = input.GetMouse0ClickedPositionBoard();
        Tile nearestTileToClick = board.GetNearestTileToPosition(clickedPosition);
        Player activePlayer = board.GetActivePlayer();

        Worker selectedWorker = board.GetNearestTileToPosition(clickedPosition).GetWorkerOnTile();
        if(activePlayer.TrySelectWorker(selectedWorker))
        {
            selectedWorker.EnableHighlight();
            return (int)Player.StateId.Moving;
        }

        return -1;
    }

    public override int GetStateId()
    {
        return (int)Player.StateId.Selecting;
    }
}
