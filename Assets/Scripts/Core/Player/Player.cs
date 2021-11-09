using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Player : MonoBehaviour
{
    [SerializeField, Range(0f, 50f)] private float _moveSpped;
    [SerializeField, Range(0f, 1000f)] private float _maxHealth;
    [SerializeField, Range(.5f, 2f)] private float _shootsPerSeconds;
    [SerializeField, Range(1f, 100f)] private float _damage;
    [SerializeField, Range(0f, 50f)] private float _shotDistance;

    [SerializeField] Transform _attackDirection;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private Arrow _arrowPrefab;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerUI _playerUI;


    private Rigidbody _rigidbody;
    private Vector2 _input;
    private float _health;
    private bool _changePosition;
    private bool _isShooting;
    private bool _isDead;
    private Action _onDie;

    public List<Enemy> _enemies;
    private Enemy _target;

    public Vector3 TargetPoint => _targetPoint.position;

    public void Initialize(Action onDie)
    {
        _onDie = onDie;
        _rigidbody = GetComponent<Rigidbody>();
        _health = _maxHealth;
        _isShooting = true;
        _playerUI.Initialize(_maxHealth);
    }

    public void Revival()
    {
        _health = _maxHealth;
        _isDead = false;
    }

    public void SetTargets(List<Enemy> enemies)
    {
        _enemies = enemies;
    }

    public void GameUpdate()
    {
        if (_changePosition)
        {
            _animator.SetBool("isRunning", true);
            _animator.SetBool("isAttack", false);

            _changePosition = false;
            _rigidbody.velocity = new Vector3(_input.x * _moveSpped, _rigidbody.velocity.y, _input.y * _moveSpped);
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);

            _playerUI.GameUpdate();
        }
        else
        {
            _animator.SetBool("isRunning", false);
            Shoot();
        }
    }

    public void ChangedInput(Vector2 input)
    {
        _changePosition = true;
        _input = input;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0f && !_isDead)
        {
            _health = 0f;
            Die();
        }
        
        _playerUI.SetHealth(_health);
    }

    private void Die()
    {
        _onDie.Invoke();
        _isDead = true;
    }

    private void Shoot()
    {
        if (_enemies.Count == 0)
        {
            _animator.SetBool("isAttack", false);
            return;
        }

        _target = _enemies[0];
        for (int i = 1; i < _enemies.Count; ++i)
        {

            if (Vector3.Distance(_enemies[i].transform.position, transform.position) < Vector3.Distance(_target.transform.position, transform.position))
            {
                _target = _enemies[i];
            }
        }

        if (_isShooting && _target != null)
        {
            _isShooting = false;
            _animator.SetBool("isAttack", true);
            StartCoroutine(ShootCoroutine());

            Vector3 point = _target.TargetPoint;
            transform.LookAt(point);
            transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
            _attackDirection.LookAt(point);

            Arrow arrowInstance = Instantiate(_arrowPrefab, _attackDirection.position, _attackDirection.rotation);
            arrowInstance.Initialize((point - _attackDirection.position).normalized);
        }
    }

    private IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(1f / _shootsPerSeconds);
        _isShooting = true;
    }

    public void Recycle()
    {
        _enemies = null;
        Destroy(gameObject);
    }
}