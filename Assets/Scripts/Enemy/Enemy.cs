using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("<color=red>AI</color>")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform[] _patrolNodes;
    [SerializeField] private float _chaseDist = 7.5f;
    [SerializeField] private float _attackDist = 2.5f;
    [SerializeField] private float _nodeChangeDist = .75f;
    [SerializeField] private float _minIdleTime = 5f;
    [SerializeField] private float _maxIdleTime = 15f;

    [Header("<color=red>Values</color>")]
    [SerializeField] private int _maxHP = 100;

    private bool _isIdle = false;
    private int _actualHP;

    private Transform _actualNode;

    private void Awake()
    {
        _actualHP = _maxHP;
    }

    private void Start()
    {
        if (!_agent)
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        _actualNode = GetNewNode();

        _agent.SetDestination(_actualNode.position);
    }

    private void FixedUpdate()
    {
        if((_target.position - transform.position).sqrMagnitude <= Mathf.Pow(_chaseDist, 2))
        {
            if((_target.position - transform.position).sqrMagnitude <= Mathf.Pow(_attackDist, 2))
            {
                _agent.isStopped = true;
                transform.LookAt(_target);
            }
            else if (_agent.isStopped)
            {
                _agent.isStopped = false;
            }
            else
            {
                _agent.SetDestination(_target.position);
            }
        }
        else if((_actualNode.position - transform.position).sqrMagnitude <= Mathf.Pow(_nodeChangeDist, 2) && !_isIdle)
        {
            StartCoroutine(Idle(Random.Range(_minIdleTime, _maxIdleTime)));
        }
        else
        {
            _agent.SetDestination(_actualNode.position);
        }
    }

    public void TakeDamage(int dmg)
    {
        _actualHP -= dmg;

        if(_actualHP <= 0)
        {
            print($"Oh my God, they killed {name}!");

            Destroy(gameObject);
        }
        else
        {
            print($"<color=red>{name}</color> recibió <color=black>{dmg}</color> puntos de daño y tiene <color=green>{_actualHP}</color> puntos de vida restantes.");
        }
    }

    private Transform GetNewNode(Transform actualNode = null)
    {
        Transform newNode = null;

        do
        {
            newNode = _patrolNodes[Random.Range(0, _patrolNodes.Length)];
        }
        while (newNode == actualNode);

        return newNode;
    }

    private IEnumerator Idle(float t)
    {
        _isIdle = !_isIdle;

        _agent.isStopped = true;

        yield return new WaitForSeconds(t);

        _actualNode = GetNewNode(_actualNode);

        _agent.SetDestination(_actualNode.position);

        _agent.isStopped = false;

        _isIdle = !_isIdle;
    }
}
