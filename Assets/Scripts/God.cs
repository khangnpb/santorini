using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class God
{
    int _moves = 0;
    int _builds = 0;
    int _placedWorkersThisTurn = 0;
    int _placedWorkers = 0;

    protected int _maxMoves = 0;
    protected int _maxBuilds = 0;
    protected int _placedWorkersPerTurn = 1;
    protected int _maxPlacedWorkers = 2;

    public virtual void Initialize() { return; }

    public virtual void ResetCounters()
    {
        InitializeMoves();
        InitializeBuilds();
        InitializePlacedWorkers();
    }

    public virtual void EnableRealTurns()
    {
        _maxMoves = 1;
        _maxBuilds = 1;
    }

    public virtual bool FinishedTurn()
    {
        return DonePlacingThisTurn() && DoneMoving() && DoneBuilding();
    }

    public virtual bool HasAvailableMove(Worker worker)
    {
        foreach(Tile.TileNeighbour neighbour in worker.GetTile().GetTileNeighbours())
        {
            if(AllowsMove(neighbour.GetTile(), worker))
            {
                return true;
            }
        }

        return false;
    }

    public virtual bool AllowsMove(Tile tile, Worker worker)
    {
        return AllowsMove(tile) && worker.GetTile().IsTileDirectlyNeighbouring(tile);
    }

    public virtual bool AllowsMove(Tile tile)
    {
        return !tile.HasWorkerOnTile();
    }

    public virtual bool AllowsOpponentMove(Tile tile) { return true; }
    public virtual bool AllowsOpponentMove(Worker worker, Tile tile) { return true; }
    
    public virtual bool AllowsBuild(Tile tile, Worker worker)
    {
        if(tile.HasWorkerOnTile())
        {
            return false;
        }

        if(!worker.GetTile().IsTileDirectlyNeighbouring(tile))
        {
            return false;
        }

        return true;
    }

    public virtual bool AllowsOpponentBuild(Worker worker, Tile tile) { return true; }

    public virtual bool HasWon(Board board, List<Worker> workers)
    {
        foreach(Worker worker in workers)
        {
            if(worker.GetTile().GetLevel() == Tile.Level.Level3)
            {
                return true;
            }
        }

        return false;
    }

    public virtual bool PreventsWin(Player opponent) { return false; }

    public virtual void InitializeMoves() { _moves = 0; }
    public virtual void RegisterMove() { ++_moves; }
    public virtual bool DoneMoving() { return _moves >= _maxMoves; }


    public virtual void InitializeBuilds() { _builds = 0; }
    public virtual void RegisterBuild() { ++_builds; }
    public virtual bool DoneBuilding() { return _builds >= _maxBuilds; }

    public virtual void InitializePlacedWorkers() { _placedWorkersThisTurn = 0; }
    public virtual void RegisterPlacedWorker() { ++_placedWorkers; ++_placedWorkersThisTurn; }
    public virtual bool DonePlacing() { return _placedWorkers >= _maxPlacedWorkers; }
    public virtual bool DonePlacingThisTurn() { return _placedWorkersThisTurn >= _placedWorkersPerTurn; }
}