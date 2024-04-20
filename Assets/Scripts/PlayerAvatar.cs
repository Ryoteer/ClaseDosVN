using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    private Player _parent;

    private void Start()
    {
        _parent = GetComponentInParent<Player>();
    }

    public void SetAttackState(int state)
    {
        _parent.SetAttackState(state);
    }

    public void Attack(int dmg)
    {
        _parent.Attack(Random.Range(dmg / 2, dmg * 2));
    }

    public void SetJumpState(int state)
    {
        _parent.SetJumpState(state);
    }

    public void Jump()
    {
        _parent.Jump();
    }
}
