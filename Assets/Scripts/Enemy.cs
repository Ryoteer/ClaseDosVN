using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("<color=red>Values</color>")]
    [SerializeField] private int _maxHP = 100;

    private int _actualHP;

    private void Awake()
    {
        _actualHP = _maxHP;
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
}
