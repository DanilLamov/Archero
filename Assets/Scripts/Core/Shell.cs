using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Shell : MonoBehaviour
{
    [SerializeField] private float _launchForce;
    
    private float _damage;
    private Rigidbody _rigidbody;

    public void Initialize(Vector3 direction, float damage)
    {
        _damage = damage;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = direction * _launchForce;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Arrow"))
    //    {
    //        return;
    //    }

    //    if (collision.collider.CompareTag("Player"))
    //    {
    //        collision.collider.GetComponent<Player>().TakeDamage(_damage);
    //    }
    //    Destroy(gameObject);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Arrow"))
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(_damage);
        }
        Destroy(gameObject);
    }
}