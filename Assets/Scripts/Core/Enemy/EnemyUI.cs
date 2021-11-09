using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _fillImage;
    [SerializeField] private Color _fullHealthColor;
    [SerializeField] private Color _zeroHealthColor;

    private float _maxHealth;
    private Quaternion _relativeRotation;

    public void Initialize(float maxHealth)
    {
        _slider.maxValue = maxHealth;
        _slider.value = maxHealth;
        _maxHealth = maxHealth;
        _relativeRotation = GetComponent<Canvas>().transform.localRotation;
        _fillImage.color = _fullHealthColor;
    }

    public void GameUpdate()
    {
        transform.rotation = _relativeRotation;
    }

    public void SetHealth(float health)
    {
        Debug.Log(health);

        _slider.value = health;
        _fillImage.color = Color.Lerp(_fullHealthColor, _zeroHealthColor, health / _maxHealth);
    }
}
