using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Player
{
    public bool PreventsWin(Computer opponent)
    {
        return _god.PreventsWin(opponent);
    }
}
