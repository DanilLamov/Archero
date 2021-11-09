using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 50f;

    private Action<Coin> _coinCollected;

    public void Initialize(Action<Coin> coinCollected)
    {
        _coinCollected = coinCollected;
    }

    public void GameUpdate()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _coinCollected.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void Recycle()
    {
        _coinCollected = null;
        Destroy(gameObject);
    }
}