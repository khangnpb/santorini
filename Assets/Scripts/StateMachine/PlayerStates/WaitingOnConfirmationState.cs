using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingOnConfirmationState : State
{
    public override void EnterState(InputSystem input, Board board)
    {
        input.ResetMouse0Click();
    }

    public override void ExitState() { return; }

    public override int UpdateState(InputSystem input, Board board)
    {
        if(board.PressedEndTurn())
        {
            return (int)Player.StateId.DoneTurn;
        }

        return -1;
    }

    public override int GetStateId()
    {
        return (int)Player.StateId.WaitingOnConfirmation;
    }
}
