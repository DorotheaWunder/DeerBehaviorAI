using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target & Offset")]
    [SerializeField] private Transform _currentTarget;
    public Transform TargetPlayer;
    public Transform TargetDeer;

    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -8f);

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float pitchMin = -20f;
    [SerializeField] private float pitchMax = 60f;

    private float yaw;
    private float pitch;

    public bool IsPlayer { get; private set; }

    private void Start()
    {
        SwitchToPlayer();
    }

    private void LateUpdate()
    {
        if (!_currentTarget) return;

        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        transform.position = _currentTarget.position + rotation * offset;
        transform.LookAt(_currentTarget.position + Vector3.up * 1.5f);
    }

    public void SwitchTarget()
    {
        if (IsPlayer)
            SwitchToDeer();
        else
            SwitchToPlayer();
    }

    private void SwitchToPlayer()
    {
        _currentTarget = TargetPlayer;
        IsPlayer = true;
    }

    private void SwitchToDeer()
    {
        _currentTarget = TargetDeer;
        IsPlayer = false;
    }
}
