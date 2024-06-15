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

    public void Attack()
    {
        _parent.Attack();
    }

    public void RangeAttack()
    {
        _parent.RangeAttack();
    }

    public void AreaAttack()
    {
        _parent.AreaAttack();
    }

    public void Jump()
    {
        _parent.Jump();
    }

    public void PlayStepClip()
    {
        _parent.PlayStepClip();
    }

    public void PlayAttackClip()
    {
        _parent.PlayAttackClip();
    }
}
