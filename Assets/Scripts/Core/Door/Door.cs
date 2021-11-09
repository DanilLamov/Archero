using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Door : MonoBehaviour
{
    [SerializeField] private Barrier _barier;

    private Action _levelCompleted;
    public void Initialize(Action levelCompleted)
    {
        _levelCompleted = levelCompleted;
    }

    public void Open()
    {
        StartCoroutine(GetDownBarrierCoroutine());
    }

    private IEnumerator GetDownBarrierCoroutine()
    {
        Vector3 startPosition = _barier.transform.position;
        Vector3 targetPosition = _barier.transform.position + 2.5f * Vector3.down;
        for (float i = 0f; i < 1f; i += Time.deltaTime)
        {
            _barier.transform.position = Vector3.Lerp(startPosition, targetPosition, i);

            yield return null;
        }

        _barier.transform.position = targetPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _levelCompleted?.Invoke();
        }
    }
}
