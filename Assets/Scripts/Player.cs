using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("<color=yellow>Animator</color>")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _xAxisName = "xAxis";
    [SerializeField] private string _zAxisName = "zAxis";
    [SerializeField] private string _onJumpName = "onJump";
    [SerializeField] private string _onAttackName = "onAttack";
    [SerializeField] private string _isMovingName = "isMoving";
    [SerializeField] private string _isGroundedName = "isGrounded";

    [Header("<color=green>Inputs</color>")]
    [Tooltip("Selects the key the player will use to jump.")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _attackKey = KeyCode.Mouse0;

    [Header("<color=blue>Physics</color>")]
    [SerializeField] private float _groundCheckDist = .5f;
    [SerializeField] private LayerMask _groundCheckMask;
    [SerializeField] private float _moveCheckDist = .75f;
    [SerializeField] private LayerMask _moveCheckMask;

    [Header("<color=orange>Values</color>")]
    [Tooltip("Modifies how high the player will jump.")]
    [SerializeField] private float _jumpForce = 5f;
    [Tooltip("Modifies how fast the player will move.")]
    [SerializeField] private float _movSpeed = 5f;

    private bool _isAttacking = false, _isJumping = false;
    private float _xAxis = 0f, _zAxis = 0f;
    private Vector3 _dir = new(), _transformOffset = new(), _moveCheckDir = new();

    private Rigidbody _rb;

    private Ray _groundCheckRay, _moveCheckRay;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.angularDrag = 1f;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Start()
    {
        if (!_animator)
        {
            _animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        _xAxis = Input.GetAxis("Horizontal");
        _zAxis = Input.GetAxis("Vertical");

        _animator.SetBool(_isMovingName, (_xAxis != 0 || _zAxis != 0));

        _animator.SetFloat(_xAxisName, _xAxis);
        _animator.SetFloat(_zAxisName, _zAxis);

        if (Input.GetKeyDown(_jumpKey) && !_isJumping)
        {
            _animator.SetTrigger(_onJumpName);
        }

        if (Input.GetKeyDown(_attackKey) && !_isAttacking)
        {
            _animator.SetTrigger(_onAttackName);
        }

        _animator.SetBool(_isGroundedName, IsGrounded());
    }

    private void FixedUpdate()
    {
        if((_xAxis != 0 || _zAxis != 0) && !IsBlocked(_xAxis, _zAxis))
        {
            Movement(_xAxis, _zAxis);
        }
    }

    private void Movement(float xAxis, float zAxis)
    {
        _dir = (transform.right * xAxis + transform.forward * zAxis).normalized;

        _rb.MovePosition(transform.position + _dir * _movSpeed * Time.fixedDeltaTime);
    }

    public void SetAttackState(int state)
    {
        switch (state)
        {
            case 0:
                _isAttacking = false;
                break;
            case 1:
                _isAttacking = true;
                break;
        }
    }

    public void Attack(int dmg)
    {
        print($"Comé, recibiste {dmg} puntos de daño.");
    }

    public void SetJumpState(int state)
    {
        switch (state)
        {
            case 0:
                _isJumping = false;
                break;
            case 1:
                _isJumping = true;
                break;
        }
    }

    public void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        _transformOffset = new Vector3(transform.position.x,
                                       transform.position.y + _groundCheckDist / 4,
                                       transform.position.z);

        _groundCheckRay = new Ray(_transformOffset, -transform.up);

        return Physics.Raycast(_groundCheckRay, _groundCheckDist, _groundCheckMask);
    }

    private bool IsBlocked(float xAxis, float zAxis)
    {
        _moveCheckDir = (transform.right * xAxis + transform.forward * zAxis);

        _moveCheckRay = new Ray(transform.position, _moveCheckDir);

        return Physics.Raycast(_moveCheckRay, _moveCheckDist, _moveCheckMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_moveCheckRay);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_groundCheckRay);
    }
}
