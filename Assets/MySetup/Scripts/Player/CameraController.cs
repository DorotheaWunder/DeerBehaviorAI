using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target & Offset")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -8f);

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float pitchMin = -20f;
    [SerializeField] private float pitchMax = 60f;

    private float yaw;
    private float pitch;

    private void LateUpdate()
    {
        if (!target) return;
        
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
        
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        
        transform.position = target.position + rotation * offset;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
