using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private TextMeshProUGUI _coinText;

    private void Start()
    {
        _playButton.onClick.AddListener(OnGameButtonClicked);

        if (PlayerPrefs.HasKey("Coins"))
        {
            _coinText.text = $"Coins: {PlayerPrefs.GetInt("Coins")}";
        }
        else
        {
            PlayerPrefs.SetInt("Coins", 0);
            _coinText.text = $"Coins: 0";
        }
    }

    private void OnGameButtonClicked()
    {
        var loadingOperations = new Queue<ILoadingOperation>();
        loadingOperations.Enqueue(new GameLoadingOperation());
        LoadingScreen.Instance.Load(loadingOperations);
    }
}