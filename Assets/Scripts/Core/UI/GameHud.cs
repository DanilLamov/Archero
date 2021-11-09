using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameHud : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private PrepareGamePanel _prepareGamePanel;
    [SerializeField] private ToggleWithSpriteSwap _pauseToggle;

    private Action<Vector2> _changedInput;
    private Action<bool> _pauseClicked;
    private Action _quitGame;

    private CancellationTokenSource _prepareCancellation;

    private bool IsInput => _joystick.Input.magnitude > 0f;
    private Vector2 Input => _joystick.Input;

    
    public void Initialize(Action<Vector2> changedInput, Action<bool> pauseClicked, Action quitGame)
    {
        _changedInput = changedInput;
        _pauseClicked = pauseClicked;
        _quitGame = quitGame;

        _pauseToggle.ValueChanged += OnPauseClicked;
    }

    public async void StartCountdown(Action action)
    {
        _prepareCancellation?.Dispose();
        _prepareCancellation = new CancellationTokenSource();
        if (await _prepareGamePanel.Prepare(_prepareCancellation.Token))
        {
            action.Invoke();
        }
    }

    public void GameUpdate()
    {
        if (IsInput)
        {
            _changedInput?.Invoke(Input);
        }
    }

    private async void OnPauseClicked()
    {
        _pauseClicked?.Invoke(true);
        var isConfirmed = await PauseMenu.Instance.AwaitForDecision();
        _pauseClicked?.Invoke(false);
        if (isConfirmed) {
            _quitGame?.Invoke();
        }
    }
}