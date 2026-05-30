using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 cameraOffset;
    public float smoothTime = 0.3f;
    public Transform player;

    private Vector3 _currentVelocity = Vector3.zero;
    
    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(player.position.x, cameraOffset.y, player.position.z + cameraOffset.z);
        
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            targetPosition, 
            ref _currentVelocity, 
            smoothTime
        );
    }
}
