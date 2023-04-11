using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomPlayer;

public class BuildingState : State
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
        if (activePlayer.GetGod().AllowsBuild(nearestTileToClick, selectedWorker) && 
            board.AllowsBuild(selectedWorker, nearestTileToClick) &&
            board.OpponentsAllowBuild(selectedWorker, nearestTileToClick))
        {
            // God, Board, and opponents all agree that the build is legal
            nearestTileToClick.AddTowerPiece();
            activePlayer.GetGod().RegisterBuild();
            if(activePlayer.GetGod().DoneBuilding())
            {
                selectedWorker.DisableHighlight();
                if(activePlayer._isCom)
                {
                    return (int)Player.StateId.DoneTurn;
                }
                return (int)Player.StateId.WaitingOnConfirmation;
            }
        }

        return -1;
    }

    public override int GetStateId()
    {
        return (int)Player.StateId.Building;
    }
}
