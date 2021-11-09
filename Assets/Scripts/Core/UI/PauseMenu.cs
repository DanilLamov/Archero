using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _continueButton;

    public static PauseMenu Instance { get; private set; }

    private Canvas _canvas;
    private TaskCompletionSource<bool> _taskCompletion;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;
        Instance = this;

        _mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        _continueButton.onClick.AddListener(OnContinueClicked);
    }

    public async Task<bool> AwaitForDecision()
    {
        _canvas.enabled = true;
        _taskCompletion = new TaskCompletionSource<bool>();
        var result = await _taskCompletion.Task;
        _canvas.enabled = false;
        return result;
    }

    private void OnMainMenuClicked()
    {
        _taskCompletion.SetResult(true);
    }

    private void OnContinueClicked()
    {
        _taskCompletion.SetResult(false);
    }   
}