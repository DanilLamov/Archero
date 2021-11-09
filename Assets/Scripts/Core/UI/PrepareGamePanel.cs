using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PrepareGamePanel : MonoBehaviour
{
    [SerializeField, Range(1f, 15f)] private float _prepareTime;

    [SerializeField] private GameObject[] _colors;
    [SerializeField] private GameObject _go;
    [SerializeField] private Vector3 _defaultScale;
    [SerializeField] private Vector3 _bigScale;

    public async Task<bool> Prepare(CancellationToken cancellationToken)
    {
        Reset();
        gameObject.SetActive(true);

        var elementsCount = _colors.Length + 1;
        var unitTime = _prepareTime / elementsCount;

        for (var i = 0; i < _colors.Length; i++)
        {
            if (i > 0)
            {
                _colors[i - 1].transform.localScale = _defaultScale;
            }
            _colors[i].transform.localScale = _bigScale;
            await Task.Delay(TimeSpan.FromSeconds(unitTime), cancellationToken);

            if (cancellationToken.IsCancellationRequested)
            {
                gameObject.SetActive(false);
                return false;
            }
        }

        foreach (var c in _colors)
        {
            c.gameObject.SetActive(false);
        }
        _go.SetActive(true);

        await Task.Delay(TimeSpan.FromSeconds(unitTime), cancellationToken);
        if (cancellationToken.IsCancellationRequested)
        {
            gameObject.SetActive(false);
            return false;
        }

        gameObject.SetActive(false);
        return true;
    }

    private void Reset()
    {
        foreach (var c in _colors)
        {
            c.transform.localScale = _defaultScale;
            c.gameObject.SetActive(true);
        }
        _go.SetActive(false);
    }
}