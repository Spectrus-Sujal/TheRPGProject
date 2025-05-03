using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float _health;
    public float Health { get { return _health; } }

    [SerializeField] private float _movespeed;
    public float MoveSpeed { get { return _movespeed; } }

    [SerializeField] private float _attackpower;
    public float AttackPower { get { return _attackpower; } }

    [SerializeField] private float _attackspeed;
    public float AttackSpeed { get { return _attackspeed; } }

}
