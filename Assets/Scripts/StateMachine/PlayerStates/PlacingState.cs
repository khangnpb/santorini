using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomPlayer;

public class PlacingState : State
{
    public override void EnterState(InputSystem input, Board board)
    {
        input.ResetMouse0Click();
    }

    public override void ExitState() { return; }

    public override int UpdateState(InputSystem input, Board board)
    {
        Player activePlayer = board.GetActivePlayer();

        if(activePlayer.GetGod().DonePlacing())
        {
            return (int)Player.StateId.Selecting;
        }


        if (!activePlayer._isCom && !input.Mouse0ClickedOnBoard()) { return -1; }
        
        Vector3 clickedPosition = activePlayer._isCom ? new Vector3(Random.Range(-20f, 20f), 16f, Random.Range(-20f, 20f)) : input.GetMouse0ClickedPositionBoard();
        Tile nearestTileToClick = board.GetNearestTileToPosition(clickedPosition);
        Debug.Log(nearestTileToClick);

        if (activePlayer.GetGod().AllowsMove(nearestTileToClick) && board.OpponentsAllowMove(nearestTileToClick))
        {
            GameObject newWorkerGO = board.GetNextWorkerPrefab(out Worker.Gender gender, out Worker.Colour colour);
            Worker newWorker = nearestTileToClick.PlaceWorker(newWorkerGO, gender, colour);
            newWorker.SetPlayer(activePlayer);
            activePlayer.AddWorker(newWorker);
            activePlayer.GetGod().RegisterPlacedWorker();

            if (activePlayer.GetGod().DonePlacingThisTurn())
            {
                if (activePlayer._isCom)
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
        return (int)Player.StateId.Placing;
    }
}
