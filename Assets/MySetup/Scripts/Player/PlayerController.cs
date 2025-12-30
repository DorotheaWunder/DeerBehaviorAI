using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    private Rigidbody _rb;
    private Vector3 _moveInput;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(h, 0f, v).normalized;
        
        if (cameraTransform != null)
        {
            Vector3 camForward = cameraTransform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = cameraTransform.right;
            camRight.y = 0f;
            camRight.Normalize();

            _moveInput = (camForward * input.z + camRight * input.x).normalized;
        }
        else
        {
            _moveInput = input;
        }
    }

    private void FixedUpdate()
    {
        Vector3 velocity = _moveInput * moveSpeed;
        _rb.velocity = new Vector3(velocity.x, _rb.velocity.y, velocity.z);
    }
}
