using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomPlayer;

public class MovingState : State
{
    int getMinimaxValue(int[,] boardData)
    {
        int value = 0;
        //find 1



        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (boardData[i, j] == 1)
                {
                    value += boardData[i, j] / 10 * 10 * 2;
                    if (boardData[i, j] >= 30) value += 10000;
                    //check if it can move
                    if (i - 1 >= 0 && boardData[i - 1, j] > 10 && boardData[i - 1, j] < 40) value += boardData[i - 1, j] / 10 * 10;
                    if (i + 1 < 5 && boardData[i + 1, j] > 10 && boardData[i - 1, j] < 40) value += boardData[i + 1, j] / 10 * 10;
                    if (j - 1 >= 0 && boardData[i, j - 1] > 10 && boardData[i - 1, j] < 40) value += boardData[i, j - 1] / 10 * 10;
                    if (j + 1 < 5 && boardData[i, j + 1] > 10 && boardData[i - 1, j] < 40) value += boardData[i, j + 1] / 10 * 10;
                }
                else if (boardData[i, j] == 2)
                {
                    value -= boardData[i, j] / 10 * 10 * 2;
                    if (boardData[i, j] >= 30) value -= 10000;
                    //check if it can move
                    if (i - 1 >= 0 && boardData[i - 1, j] >= 10 && boardData[i - 1, j] < 13) value -= boardData[i - 1, j] / 10 * 10;
                    if (i + 1 < 5 && boardData[i + 1, j] >= 10 && boardData[i - 1, j] < 13) value -= boardData[i + 1, j] / 10 * 10;
                    if (j - 1 >= 0 && boardData[i, j - 1] >= 10 && boardData[i - 1, j] < 13) value -= boardData[i, j - 1] / 10 * 10;
                    if (j + 1 < 5 && boardData[i, j + 1] >= 10 && boardData[i - 1, j] < 13) value -= boardData[i, j + 1] / 10 * 10;
                }
            }
        }

        return value;
    }

    Vector3 getMoveFromPosValue(int pos1, int pos2)
    {
        Vector3[,] mV = new Vector3[5, 5];
        mV[0, 0] = new Vector3(-20.91f, 16f, -20.46f);
        mV[0, 1] = new Vector3(-22.44f, 16f, -7.87f);
        mV[0, 2] = new Vector3(-20.56f, 16f, 0.9f);
        mV[0, 3] = new Vector3(-21.35f, 16f, 9.54f);
        mV[0, 4] = new Vector3(-21.66f, 16f, 20.81f);

        mV[1, 0] = new Vector3(-10.45f, 16f, -21.6f);
        mV[1, 1] = new Vector3(-11.72f, 16f, -13.11f);//
        mV[1, 2] = new Vector3(-7.87f, 16f, 0.9f);
        mV[1, 3] = new Vector3(14.91f, 16f, -12.39f);//
        mV[1, 4] = new Vector3(-9.54f, 16f, 20.81f);

        mV[2, 0] = new Vector3(0.9f, 16f, -20.46f);
        mV[2, 1] = new Vector3(-14.28f, 16f, -7.87f);//?
        mV[2, 2] = new Vector3(0.9f, 16f, 0.9f);
        mV[2, 3] = new Vector3(0.9f, 16f, 9.54f);
        mV[2, 4] = new Vector3(0.9f, 16f, 20.81f);

        mV[3, 0] = new Vector3(0.9f, 16f, -20.46f);
        mV[3, 1] = new Vector3(14.91f, 16f, -12.39f);//
        mV[3, 2] = new Vector3(0.9f, 16f, 0.9f);
        mV[3, 3] = new Vector3(0.9f, 16f, 9.54f);
        mV[3, 4] = new Vector3(0.9f, 16f, 20.81f);

        mV[4, 0] = new Vector3(0.9f, 16f, -20.46f);
        mV[4, 1] = new Vector3(0.9f, 16f, -7.87f);
        mV[4, 2] = new Vector3(0.9f, 16f, 0.9f);
        mV[4, 3] = new Vector3(0.9f, 16f, 9.54f);
        mV[4, 4] = new Vector3(0.9f, 16f, 20.81f);
        return mV[pos1, pos2];
    }
    Vector3 Move(int[,] boardData)
    {
        // Tìm nước đi tối ưu bằng thuật toán Minimax
        int bestValue = int.MinValue;
        Vector3 bestMove = Vector3.zero;

        //find 2
        int p11 = -1, p12 = -1, p21 = -1, p22 = -1;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (boardData[i, j] == 2)
                {
                    if (p11 == -1)
                    {
                        p11 = i;
                        p12 = j;
                    }
                    else
                    {
                        p21 = i;
                        p22 = j;
                    }
                }

            }
        }

        //create 2d array temp = [[p11 - 1, p12], [p11, p12 - 1], [p11, p12 + 1], [p11 + 1, p12]
        int[,] temp = new int[4, 2];
        temp[0, 0] = p11 - 1;
        temp[0, 1] = p12;
        temp[1, 0] = p11;
        temp[1, 1] = p12 - 1;
        temp[2, 0] = p11;
        temp[2, 1] = p12 + 1;
        temp[3, 0] = p11 + 1;
        temp[3, 1] = p12;

        for (int i = 0; i < 4; i++)
        {
            int t1 = temp[i, 0], t2 = temp[i, 1];
            if (t1 >= 0 && boardData[t1, t2] == 0)
            {
                boardData[t1, t2] = 2;
                boardData[p11, p12] = 0;
                int value = Minimax(boardData, false, 0);
                boardData[t1, t2] = 0;
                boardData[p11, p12] = 2;

                if (value > bestValue)
                {
                    bestValue = value;
                    bestMove = getMoveFromPosValue(t1, t2);
                }
            }
        }

        return bestMove;
    }

    int Minimax(int[,] boardData, bool isMaximizingPlayer, int depth)
    {
        // Nếu đạt độ sâu tối đa hoặc là trạng thái lá, trả về giá trị đánh giá
        if (depth == 0)
        {
            return getMinimaxValue(boardData);
        }

        int bestValue = 0;
        if (isMaximizingPlayer)
        {
            bestValue = int.MinValue;
            for (int i = 0; i < boardData.GetLength(0); i++)
            {
                for (int j = 0; j < boardData.GetLength(1); j++)
                {
                    if (boardData[i, j] == 0)
                    {
                        boardData[i, j] = 1; // Giả sử đánh cờ X
                        int value = Minimax(boardData, false, depth + 1);
                        boardData[i, j] = 0; // Khôi phục lại trạng thái ban đầu

                        bestValue = Mathf.Max(bestValue, value);
                    }
                }
            }
        }
        else
        {
            bestValue = int.MaxValue;
            for (int i = 0; i < boardData.GetLength(0); i++)
            {
                for (int j = 0; j < boardData.GetLength(1); j++)
                {
                    if (boardData[i, j] == 0)
                    {
                        boardData[i, j] = 2; // Giả sử đánh cờ O
                        int value = Minimax(boardData, true, depth + 1);
                        boardData[i, j] = 0; // Khôi phục lại trạng thái ban đầu

                        bestValue = Mathf.Min(bestValue, value);
                    }
                }
            }
        }
        return bestValue;
    }

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
            for (float x = -20f ; x <= 20f; x += 10f)
            {
                for (float y = -20f; y <= 20f; y += 10f)
                {
                    if(activePlayer.GetGod().AllowsMove(board.GetNearestTileToPosition(new Vector3(x, 16f, y)), selectedWorker) && board.AllowsMove(selectedWorker, board.GetNearestTileToPosition(new Vector3(x, 16f, y))))
                    {
                        Tiles.Add(board.GetNearestTileToPosition(new Vector3(x, 16f, y)));
                        Levels.Add(board.GetNearestTileToPosition(new Vector3(x, 16f, y)).GetLevel());
                    }
                }
            }
            List<int> intList = Levels.ConvertAll(c => (int)c);
            string listAsString = string.Join(", ", intList);
            Debug.Log(listAsString);
            int maxValue=0;
            for (int x = 0; x < intList.Count; x++)
            {
                if (intList[x] > maxValue && intList[x]!=4)
                {
                    maxValue = intList[x];
                }
            }

            Tile.Level maxlevel = (Tile.Level)maxValue;

            int i = 0;
            for (i = 0; i<Tiles.Count; i++)
            {
                if (Levels[i] == maxlevel)
                {
                    break;
                }

            }
            Tile nearestTileToClick = Tiles[i];

            Worker workerOnTile = nearestTileToClick.GetWorkerOnTile();
            if (!activePlayer._isCom && workerOnTile != null && activePlayer.GetWorkers().Contains(workerOnTile))
            {
                // Player has decided to reselect which worker they're using
                selectedWorker.DisableHighlight();
                return (int)Player.StateId.Selecting;
            }

            if (activePlayer.GetGod().AllowsMove(nearestTileToClick, selectedWorker) &&
                board.AllowsMove(selectedWorker, nearestTileToClick) &&
                board.OpponentsAllowMove(selectedWorker, nearestTileToClick))
            {
                // God, Board, and opponents all agree that the move is legal
                selectedWorker.GetTile().RemoveWorker();
                nearestTileToClick.AddWorker(selectedWorker);
                activePlayer.GetGod().RegisterMove();
                selectedWorker.SetTile(nearestTileToClick);

                if (activePlayer.GetGod().DoneMoving())
                {
                    return (int)Player.StateId.Building;
                }


            }
            return -1;
        }
        else
        {
             
           Vector3 clickedPosition =   input.GetMouse0ClickedPositionBoard();
           Tile nearestTileToClick = board.GetNearestTileToPosition(clickedPosition);


            Worker workerOnTile = nearestTileToClick.GetWorkerOnTile();
            if (!activePlayer._isCom && workerOnTile != null && activePlayer.GetWorkers().Contains(workerOnTile))
            {
                // Player has decided to reselect which worker they're using
                selectedWorker.DisableHighlight();
                return (int)Player.StateId.Selecting;
            }

            if (activePlayer.GetGod().AllowsMove(nearestTileToClick, selectedWorker) &&
                board.AllowsMove(selectedWorker, nearestTileToClick) &&
                board.OpponentsAllowMove(selectedWorker, nearestTileToClick))
            {
                // God, Board, and opponents all agree that the move is legal
                selectedWorker.GetTile().RemoveWorker();
                nearestTileToClick.AddWorker(selectedWorker);
                activePlayer.GetGod().RegisterMove();
                selectedWorker.SetTile(nearestTileToClick);

                if (activePlayer.GetGod().DoneMoving())
                {
                    return (int)Player.StateId.Building;
                }


            }

            return -1;

        }
        //activePlayer._isCom ? new Vector3(Random.Range(-20f, 20f), 16f, Random.Range(-20f, 20f)) : input.GetMouse0ClickedPositionBoard();


        


        return -1;
    }

    public override int GetStateId()
    {
        return (int)Player.StateId.Moving;
    }
}
