using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField, Range(0f, 50f)] protected float _moveSpeed;
    [SerializeField, Range(0f, 100f)] protected float _movementRange;
    [SerializeField, Range(0f, 30f)] protected float _immobilityTime;
    [SerializeField, Range(0f, 1000f)] protected float _startHealth;
    [SerializeField, Range(.5f, 2f)] protected float _shootsPerSeconds;
    [SerializeField, Range(1f, 100f)] protected float _damage;

    [SerializeField] protected Transform _targetPoint;
    [SerializeField] protected EnemyUI _enemyUI;

    public EnemyFactory OriginFactory { get; set; }
    public Vector3 TargetPoint => _targetPoint.position;
    public EnemyType Type { get; protected set; }

    protected Rigidbody _rigidbidy;

    protected float _health;
    protected EnemyState _state;
    protected Player _target;
    protected Action<Enemy> _removeTarget;
    protected Action<Enemy> _afterDie;
    protected bool _isAttack;
    protected bool _isDead;

    public virtual void Initialize(Player player, Action<Enemy> removeTarget, Action<Enemy> aferDie)
    {
        _target = player;
        _removeTarget = removeTarget;
        _afterDie = aferDie;
        _health = _startHealth;
        _state = EnemyState.Immobility;
        

        _rigidbidy = GetComponent<Rigidbody>();
        _isAttack = true;
        _enemyUI.Initialize(_startHealth);
    }

    public abstract void GameUpdate();

    public abstract void TakeDamage(float damage);

    public void Recycle()
    {
        OriginFactory.Reclaim(this);
    }
}
