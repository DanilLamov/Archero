using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Arrow : MonoBehaviour
{
    [SerializeField] private float _launchForce;
    [SerializeField] private float _damage;

    private Rigidbody _rigidbody;

    private Vector3 _direction;

    public void Initialize(Vector3 direction)
    {
        _direction = direction;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = direction * _launchForce;
    }

    //private void Update()
    //{
    //    transform.position += _direction * Time.deltaTime * 5f;
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Shell"))
    //    {
    //        return;
    //    }

    //    if (collision.collider.CompareTag("Enemy"))
    //    {
    //        collision.collider.GetComponent<Enemy>().TakeDamage(_damage);
    //    }
    //    Destroy(gameObject);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Shell"))
        {
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(_damage);
        }
        Destroy(gameObject);
    }
}