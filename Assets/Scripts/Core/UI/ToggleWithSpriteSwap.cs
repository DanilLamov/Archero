using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleWithSpriteSwap : MonoBehaviour
{
    [SerializeField] private Image _changableImage;
    [SerializeField] private Sprite _offSprite;
    [SerializeField] private Sprite _onSprite;

    private Button _button;

    private bool _isOn;

    public event Action ValueChanged;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _isOn = false;
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        _isOn = !_isOn;
        _changableImage.sprite = _isOn ? _onSprite : _offSprite;
        ValueChanged?.Invoke();
    }
}