using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomPlayer;

public class MovingState : State
{
    public override void EnterState(InputSystem input, Board board)
    {
        input.ResetMouse0Click();
    }

    public override void ExitState() { return; }

    public override int UpdateState(InputSystem input, Board board)
    {
        Player activePlayer = board.GetActivePlayer();
        if (!activePlayer._isCom && !input.Mouse0ClickedOnBoard()) { return -1; }
        Vector3 clickedPosition = activePlayer._isCom ? new Vector3(Random.Range(-25f, 25f), 16f, Random.Range(-25f, 25f)) : input.GetMouse0ClickedPositionBoard();
        Tile nearestTileToClick = board.GetNearestTileToPosition(clickedPosition);
        Worker selectedWorker = activePlayer.GetSelectedWorker();

        Worker workerOnTile = nearestTileToClick.GetWorkerOnTile();
        if(workerOnTile != null && activePlayer.GetWorkers().Contains(workerOnTile))
        {
            // Player has decided to reselect which worker they're using
            selectedWorker.DisableHighlight();
            return (int)Player.StateId.Selecting;
        }

        if(activePlayer.GetGod().AllowsMove(nearestTileToClick, selectedWorker) && 
            board.AllowsMove(selectedWorker, nearestTileToClick) &&
            board.OpponentsAllowMove(selectedWorker, nearestTileToClick))
        {
            // God, Board, and opponents all agree that the move is legal
            selectedWorker.GetTile().RemoveWorker();
            nearestTileToClick.AddWorker(selectedWorker);
            activePlayer.GetGod().RegisterMove();
            selectedWorker.SetTile(nearestTileToClick);

            if(activePlayer.GetGod().DoneMoving())
            {
                return (int)Player.StateId.Building;
            }
        }

        return -1;
    }

    public override int GetStateId()
    {
        return (int)Player.StateId.Moving;
    }
}
