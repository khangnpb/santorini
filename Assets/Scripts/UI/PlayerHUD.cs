using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField]
    GameObject _endTurn = default;
    [SerializeField]
    GameObject _undoTurn = default;

    bool _readyToEndTurn = false;
    bool _readyToUndoTurn = false;

    public void Reset()
    {
        ;
    }

    public void EnableEndTurnButton()
    {
        if (!_endTurn.activeInHierarchy)
        {
            _endTurn.SetActive(true);
            _readyToEndTurn = false;
        }
    }

    public void DisableEndTurnButton()
    {
        if (_endTurn.activeInHierarchy)
        {
            _endTurn.SetActive(false);
            _readyToEndTurn = false;
        }
    }

    public void EndTurnPressed()
    {
        _readyToEndTurn = true;
    }

    public bool PressedEndTurn()
    {
        return _readyToEndTurn;
    }

    public void EnableUndoTurnButton()
    {
        if (!_undoTurn.activeInHierarchy)
        {
            _undoTurn.SetActive(true);
            _readyToUndoTurn = false;
        }
    }

    public void DisableUndoTurnButton()
    {
        if (_undoTurn.activeInHierarchy)
        {
            _undoTurn.SetActive(false);
            _readyToUndoTurn = false;
        }
    }

    public void UndoTurnPressed()
    {
        _readyToUndoTurn = true;
    }

    public bool PressedUndoTurn()
    {
        return _readyToUndoTurn;
    }
}
