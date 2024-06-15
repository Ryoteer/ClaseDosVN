using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class Player : MonoBehaviour
{
    [Header("<color=yellow>Animator</color>")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _xAxisName = "xAxis";
    [SerializeField] private string _zAxisName = "zAxis";
    [SerializeField] private string _onJumpName = "onJump";
    [SerializeField] private string _onAttackName = "onAttack";
    [SerializeField] private string _onAreaAttackName = "onAreaAttack";
    [SerializeField] private string _onRangeAttackName = "onRangeAttack";
    [SerializeField] private string _isMovingName = "isMoving";
    [SerializeField] private string _isGroundedName = "isGrounded";

    [Header("<color=purple>Audio</color>")]
    [SerializeField] private AudioClip[] _stepClips;
    [SerializeField] private AudioClip[] _attackClips;

    [Header("<color=red>Combat</color>")]
    [Tooltip("Modifies how much damage our Player deals.")]
    [SerializeField] private int _attackDmg = 20;
    [SerializeField] private Transform _leftAttackOrigin;
    [SerializeField] private Transform _rightAttackOrigin;
    [SerializeField] private float _attackDist = .75f;
    [SerializeField] private float _rangeAttackRadius = .25f;
    [SerializeField] private float _rangeAttackDist = 15f;
    [SerializeField] private float _areaAttackRadius = 2.5f;
    [SerializeField] private LayerMask _attackMask;

    [Header("<color=green>Inputs</color>")]
    [Tooltip("Selects the key the player will use to jump.")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [Tooltip("Selects the key the player will use to use a melee attack.")]
    [SerializeField] private KeyCode _attackKey = KeyCode.Mouse0;
    [Tooltip("Selects the key the player will use an area attack.")]
    [SerializeField] private KeyCode _areaAttackKey = KeyCode.Mouse2;
    [Tooltip("Selects the key the player will use to ues a range attack.")]
    [SerializeField] private KeyCode _rangeAttackKey = KeyCode.Mouse1;

    [Header("<color=blue>Physics</color>")]    
    [SerializeField] private float _groundCheckDist = .5f;
    [SerializeField] private LayerMask _groundCheckMask;
    [SerializeField] private float _moveCheckDist = .75f;
    [SerializeField] private LayerMask _moveCheckMask;

    [Header("<color=orange>Values</color>")]    
    [Tooltip("Modifies how high the Player will jump.")]
    [SerializeField] private float _jumpForce = 5f;
    [Tooltip("Modifies how fast the Player will move.")]
    [SerializeField] private float _movSpeed = 5f;

    private bool _isAttacking = false;
    private float _xAxis = 0f, _zAxis = 0f;
    private Vector3 _dir = new(), _transformOffset = new(), _moveOrigin = new(), _moveCheckDir = new();

    private AudioSource _source;
    private Rigidbody _rb;

    private Ray _groundCheckRay, _moveCheckRay, _attackRay;
    private RaycastHit _attackHit;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.angularDrag = 1f;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;

        _source = GetComponent<AudioSource>();
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

        if (Input.GetKeyDown(_jumpKey) && IsGrounded())
        {
            _animator.SetTrigger(_onJumpName);
        }

        if (Input.GetKeyDown(_attackKey) && !_isAttacking)
        {
            _animator.SetTrigger(_onAttackName);
        }
        else if (Input.GetKeyDown(_rangeAttackKey) && !_isAttacking)
        {
            _animator.SetTrigger(_onRangeAttackName);
        }
        else if (Input.GetKeyDown(_areaAttackKey) && !_isAttacking)
        {
            _animator.SetTrigger(_onAreaAttackName);
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

    public void Attack()
    {
        _attackRay = new Ray(_rightAttackOrigin.position, transform.forward);

        if(Physics.Raycast(_attackRay, out _attackHit, _attackDist, _attackMask))
        {
            if(_attackHit.collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(_attackDmg);
            }
        }
    }

    public void RangeAttack()
    {
        RaycastHit[] hitColliders = Physics.SphereCastAll(_leftAttackOrigin.position, _rangeAttackRadius, transform.forward, _rangeAttackDist, _attackMask);

        foreach(RaycastHit hitObj in hitColliders)
        {
            if(hitObj.collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(_attackDmg / 2);
            }
        }
    }

    public void AreaAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _areaAttackRadius, _attackMask);

        foreach(Collider hitObj in hitColliders)
        {
            if(hitObj.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(_attackDmg * 2);
            }
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
        _moveOrigin = new Vector3(transform.position.x,
                                       transform.position.y + .1f,
                                       transform.position.z);

        _moveCheckDir = (transform.right * xAxis + transform.forward * zAxis);

        _moveCheckRay = new Ray(_moveOrigin, _moveCheckDir);

        return Physics.Raycast(_moveCheckRay, _moveCheckDist, _moveCheckMask);
    }

    public void PlayStepClip()
    {
        if (_source.isPlaying)
        {
            _source.Stop();
        }

        _source.clip = _stepClips[Random.Range(0, _stepClips.Length)];

        _source.Play();
    }

    public void PlayAttackClip()
    {
        if (_source.isPlaying)
        {
            _source.Stop();
        }

        _source.clip = _attackClips[Random.Range(0, _attackClips.Length)];

        _source.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_moveCheckRay);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_groundCheckRay);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(_attackRay);
        Gizmos.DrawWireSphere(transform.position, _areaAttackRadius);
    }
}
