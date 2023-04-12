using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomPlayer;

public class InputSystem : MonoBehaviour
{
    bool _mouse0ClickedThisFrame = false;
    bool _mouse0ClickedBoard = false;
    Vector3 _mouse0ClickedPositionScreen = default;
    Vector3 _mouse0ClickedPositionBoard = default;
   
    public void OnUpdate()
    {
        ResetMouse0Click();

        _mouse0ClickedThisFrame = Input.GetMouseButtonDown(0);
        if (_mouse0ClickedThisFrame)
        {
            _mouse0ClickedPositionScreen = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(_mouse0ClickedPositionScreen);
            RaycastHit hit;
            float raycastDistance = 150f;
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                _mouse0ClickedBoard = true;
                _mouse0ClickedPositionBoard = hit.point;
            }
        }
    }

    public bool GetMouse0ClickedThisFrame()
    {
        return _mouse0ClickedThisFrame;
    }

    public bool Mouse0ClickedOnBoard()
    {
        return _mouse0ClickedBoard;
    }

    public Vector3 GetMouse0ClickedPositionBoard()
    {
        return _mouse0ClickedPositionBoard;
    }

    public Vector3 GetMouse0ClickedPositionScreen()
    {
        return _mouse0ClickedPositionScreen;
    }

    public bool Mouse1Clicked()
    {
        return Input.GetMouseButton(1);
    }

    public float GetMouseScrollDeltaY()
    {
        return Input.mouseScrollDelta.y;
    }

    public void ResetMouse0Click()
    {
        _mouse0ClickedThisFrame = false;
        _mouse0ClickedBoard = false;
        _mouse0ClickedPositionBoard = new Vector3();
        _mouse0ClickedPositionScreen = new Vector3();
    }
}
