using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class GameResultWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameResultText;
    [SerializeField] private TextMeshProUGUI _coins;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _quitButton;

    private Canvas _canvas;
    private Action _onRestart;
    private Action _onQuit;

    public void Initialize(Action onRestart, Action onQuit)
    {
        _onRestart = onRestart;
        _onQuit = onQuit;

        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;
        _restartButton.onClick.AddListener(OnRestartClicked);
        _quitButton.onClick.AddListener(OnQuitClicked);
    }
    public async void Show(GameResultType result, int coins)
    {
        _coins.text = $"+{coins} coins";
        switch (result)
        {
            case GameResultType.Win:
                _gameResultText.text = "Victory!";
                break;
            case GameResultType.Lost:
                _gameResultText.text = "You have lost";
                break;
        }

        _canvas.enabled = true;
    }

    private void OnRestartClicked()
    {
        _onRestart.Invoke();
        _canvas.enabled = false;
    }

    private void OnQuitClicked()
    {
        _onQuit.Invoke();
        _canvas.enabled = false;
    }
}