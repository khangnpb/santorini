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

        List<Tile> Tiles = new List<Tile>();
        List<Tile.Level> Levels = new List<Tile.Level>();
        Worker selectedWorker = activePlayer.GetSelectedWorker();

        if (activePlayer._isCom)
        {
            for (float x = -20f; x <= 20f; x += 10f)
            {
                for (float y = -20f; y <= 20f; y += 10f)
                {
                    if (activePlayer.GetGod().AllowsMove(board.GetNearestTileToPosition(new Vector3(x, 16f, y)), selectedWorker) && board.AllowsMove(selectedWorker, board.GetNearestTileToPosition(new Vector3(x, 16f, y))))
                    {
                        Tiles.Add(board.GetNearestTileToPosition(new Vector3(x, 16f, y)));
                        Levels.Add(board.GetNearestTileToPosition(new Vector3(x, 16f, y)).GetLevel());
                    }
                }
            }
            List<int> intList = Levels.ConvertAll(c => (int)c);
            string listAsString = string.Join(", ", intList);
            Debug.Log(listAsString);
            int maxValue = 0;
            for (int x = 0; x < intList.Count; x++)
            {
                if (intList[x] > maxValue && intList[x] != 4)
                {
                    maxValue = intList[x];
                }
            }

            Tile.Level maxlevel = (Tile.Level)maxValue;

            int i = 0;
            for (i = 0; i < Tiles.Count; i++)
            {
                if (Levels[i] == maxlevel)
                {
                    break;
                }

            }
            Tile nearestTileToClick = Tiles[i];

            Worker workerOnTile = nearestTileToClick.GetWorkerOnTile();
            if (activePlayer.GetGod().AllowsBuild(nearestTileToClick, selectedWorker) &&
            board.AllowsBuild(selectedWorker, nearestTileToClick) &&
            board.OpponentsAllowBuild(selectedWorker, nearestTileToClick))
            {
                // God, Board, and opponents all agree that the build is legal
                nearestTileToClick.AddTowerPiece();
                activePlayer.GetGod().RegisterBuild();
                if (activePlayer.GetGod().DoneBuilding())
                {
                    selectedWorker.DisableHighlight();
                    if (activePlayer._isCom)
                    {
                        return (int)Player.StateId.DoneTurn;
                    }
                    return (int)Player.StateId.WaitingOnConfirmation;
                }
            }
            return -1;
        }
        else
        {

            Vector3 clickedPosition = input.GetMouse0ClickedPositionBoard();
            Tile nearestTileToClick = board.GetNearestTileToPosition(clickedPosition);


            Worker workerOnTile = nearestTileToClick.GetWorkerOnTile();
            if (activePlayer.GetGod().AllowsBuild(nearestTileToClick, selectedWorker) &&
            board.AllowsBuild(selectedWorker, nearestTileToClick) &&
            board.OpponentsAllowBuild(selectedWorker, nearestTileToClick))
            {
                // God, Board, and opponents all agree that the build is legal
                nearestTileToClick.AddTowerPiece();
                activePlayer.GetGod().RegisterBuild();
                if (activePlayer.GetGod().DoneBuilding())
                {
                    selectedWorker.DisableHighlight();
                    if (activePlayer._isCom)
                    {
                        return (int)Player.StateId.DoneTurn;
                    }
                    return (int)Player.StateId.WaitingOnConfirmation;
                }
            }

            return -1;

        }
        //activePlayer._isCom ? new Vector3(Random.Range(-20f, 20f), 16f, Random.Range(-20f, 20f)) : input.GetMouse0ClickedPositionBoard();

        return -1;
    }

    public override int GetStateId()
    {
        return (int)Player.StateId.Building;
    }
}
