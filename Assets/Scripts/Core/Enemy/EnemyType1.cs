using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyType1 : Enemy
{
    [SerializeField] private float _attackDistance;
    [SerializeField] private Animator _animator;
    
    
    private NavMeshAgent _agent;
    private Coroutine _standCoroutine;

    public override void Initialize(Player player, Action<Enemy> removeTarget, Action<Enemy> afterDie)
    {
        Type = EnemyType.Type1;
        _target = player;
        _removeTarget = removeTarget;
        _afterDie = afterDie;
        _health = _startHealth;
        _state = EnemyState.Immobility;

        _agent = GetComponent<NavMeshAgent>();
        _isAttack = true;
        _enemyUI.Initialize(_startHealth);
    }


    public override void GameUpdate()
    {
        switch (_state)
        {
            case EnemyState.Immobility:
                if (_standCoroutine == null)
                {
                    _standCoroutine = StartCoroutine(StandCoroutine());
                }
                break;
            case EnemyState.Move:
                Move();
                break;
        }
    }

    private IEnumerator StandCoroutine()
    {
        yield return new WaitForSeconds(_immobilityTime);
        _state = EnemyState.Move;
        _animator.SetBool("isRunning", true);
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(1f / _shootsPerSeconds);
        _isAttack = true;
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, _target.transform.position) <= _attackDistance)
        {
            _agent.SetDestination(transform.position);
            Attack();
        }
        else
        {
            _animator.SetBool("isRunning", true);
            _animator.SetBool("isAttack", false);
            _agent.SetDestination(_target.transform.position);
        }
    }

    private void Attack()
    {
        if (_isAttack)
        {
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isAttack", true);
            _isAttack = false;
            Vector3 point = _target.TargetPoint;
            transform.LookAt(point);
            transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
            _target.TakeDamage(_damage);
            StartCoroutine(AttackCoroutine());
        }
    }

    public override void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0f && !_isDead)
        {
            _health = 0f;
            StartCoroutine(DieCoroutine());
        }

        _enemyUI.SetHealth(_health);
    }

    private IEnumerator DieCoroutine()
    {
        _isDead = true;
        _animator.SetBool("isDead", true);
        _agent.SetDestination(transform.position);
        _removeTarget.Invoke(this);
        yield return new WaitForSeconds(1f);
        _afterDie.Invoke(this);
        Recycle();
    }
}