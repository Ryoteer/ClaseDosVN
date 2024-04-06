using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Inputs")]
    [Tooltip("Selects the key the player will use to jump.")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

    [Header("Values")]
    [Tooltip("Modifies how high the player will jump.")]
    [SerializeField] private float _jumpForce = 5f;
    [Tooltip("Modifies how fast the player will move.")]
    [SerializeField] private float _movSpeed = 5f;

    private float _xAxis = 0f, _zAxis = 0f;
    private Vector3 _dir = new();

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.angularDrag = 1f;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        _xAxis = Input.GetAxis("Horizontal");
        _zAxis = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(_jumpKey))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if(_xAxis != 0 || _zAxis != 0)
        {
            Movement(_xAxis, _zAxis);
        }
    }

    private void Movement(float xAxis, float zAxis)
    {
        _dir = (transform.right * xAxis + transform.forward * zAxis).normalized;

        _rb.MovePosition(transform.position + _dir * _movSpeed * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }
}
