using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody _rb;
    private Vector3 _moveInput;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        _moveInput = new Vector3(h, 0f, v).normalized;
    }

    private void FixedUpdate()
    {
        Vector3 velocity = _moveInput * moveSpeed;
        _rb.velocity = new Vector3(velocity.x, _rb.velocity.y, velocity.z); 
    }
}
