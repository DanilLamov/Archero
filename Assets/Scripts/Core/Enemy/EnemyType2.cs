using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyType2 : Enemy
{
    [SerializeField] private Shell _shellPrefab;
    [SerializeField] private Transform _attackDirection;
    [SerializeField] private float _shotDistance;
    [SerializeField] private Animator _animator;

    private NavMeshAgent _agent;
    private Coroutine _standCoroutine;
    private bool _isShooting;
    private float _minDistance = 3f;

    public override void Initialize(Player player, Action<Enemy> removeTarget, Action<Enemy> afterDie)
    {
        Type = EnemyType.Type2;
        _target = player;
        _removeTarget = removeTarget;
        _afterDie = afterDie;
        _health = _startHealth;
        _state = EnemyState.Immobility;
        _isShooting = true;

        _rigidbidy = GetComponent<Rigidbody>();
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
            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    private IEnumerator StandCoroutine()
    {
        yield return new WaitForSeconds(_immobilityTime);
        _state = EnemyState.Move;
        _animator.SetBool("isRunning", true);
    }

    private void Move()
    {
        if (Vector3.Distance(_target.transform.position, transform.position) <= _shotDistance)
        {
            if (Vector3.Distance(_target.transform.position, transform.position) <= _minDistance)
            {
                _agent.SetDestination(transform.position);
                _state = EnemyState.Attack;
                Shoot();
            }

            RaycastHit hit;
            if (Physics.Raycast(_attackDirection.position, _target.TargetPoint - _attackDirection.position, out hit, _shotDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    _agent.SetDestination(transform.position);
                    _state = EnemyState.Attack;
                    Shoot();
                }
                else
                {
                    _animator.SetBool("isRunning", true);
                    //_animator.SetBool("isAttack", false);
                    _agent.SetDestination(_target.transform.position);
                }
            }
            else
            {
                _animator.SetBool("isRunning", true);
                //_animator.SetBool("isAttack", false);
                _agent.SetDestination(_target.transform.position);
            }
        }
        else
        {
            _animator.SetBool("isRunning", true);
            //_animator.SetBool("isAttack", false);
            _agent.SetDestination(_target.transform.position);
        }
    }

    private void Shoot()
    {
        if (_isShooting)
        {
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isAttack", true);
            _isShooting = false;
            StartCoroutine(ShootCoroutine());

            Vector3 point = _target.TargetPoint;
            transform.LookAt(point);
            transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
            _attackDirection.LookAt(point);

            Shell shell = Instantiate(_shellPrefab, _attackDirection.position, _attackDirection.rotation);
            shell.Initialize((point - _attackDirection.position).normalized, _damage);
        }
    }

    private void Attack()
    {
        RaycastHit hit;
        if (Physics.Raycast(_attackDirection.position, _target.TargetPoint - _attackDirection.position, out hit, _shotDistance))
        {
            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Arrow") || hit.collider.CompareTag("Shell"))
            {
                Shoot();
            }
            else
            {
                _state = EnemyState.Move;
            }
        }
        else
        {
            _state = EnemyState.Move;
            _agent.SetDestination(_target.transform.position);
        }
    }

    private IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(1f / _shootsPerSeconds);
        _isShooting = true;
        _animator.SetBool("isAttack", false);
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