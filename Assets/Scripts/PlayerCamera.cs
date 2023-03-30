using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    float _rotateSpeed = 1.0f;

    [Header("Zoom")]
    [SerializeField]
    float _zoomSpeed = 1.0f;
    [SerializeField]
    float minZoom = 30f;
    [SerializeField]
    float maxZoom = 100f;

    public void OnUpdate(bool mouse1Clicked, float scrollDeltaY, Transform board)
    {
        // Rotate Camera if Right Click
        if (mouse1Clicked)
        {
            RotateAroundTransform(board, Input.GetAxis("Mouse X"));
        }

        // Zoom Camera if Mouse Wheel
        if (scrollDeltaY > Mathf.Epsilon)
        {
            ZoomIn(board.position);
        }
        else if (scrollDeltaY < -Mathf.Epsilon)
        {
            ZoomOut(board.position);
        }
    }

    public void RotateAroundTransform(Transform targetTransform, float xAxis)
    {
        transform.RotateAround(targetTransform.position, targetTransform.up, xAxis * _rotateSpeed);
    }

    public void ZoomIn(Vector3 targetPosition)
    {
        if (transform.position.y > minZoom)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _zoomSpeed);
        }
    }

    public void ZoomOut(Vector3 targetPosition)
    {
        if (transform.position.y < maxZoom)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, -_zoomSpeed);
        }
    }
}
