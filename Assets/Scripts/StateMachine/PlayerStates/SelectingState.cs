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
        Vector3 clickedPosition = activePlayer._isCom ? new Vector3(Random.Range(-20f, 20f), 16f, Random.Range(-20f, 20f)) : input.GetMouse0ClickedPositionBoard();
        
        Tile nearestTileToClick = board.GetNearestTileToPosition(clickedPosition);

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
