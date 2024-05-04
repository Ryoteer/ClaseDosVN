using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour
{
    [Header("Components")]
    [Tooltip("Assign a Game Object for the Camera to follow.")]
    [SerializeField] private Transform _target;

    private Vector3 _offset = new(), _finalCamPos = new();

    private void Start()
    {
        _offset = transform.position - _target.position;
    }

    private void LateUpdate()
    {
        _finalCamPos = _target.position + _offset;

        transform.position = _finalCamPos;
    }
}
