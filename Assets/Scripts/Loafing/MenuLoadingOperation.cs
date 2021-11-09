using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoadingOperation : ILoadingOperation
{
    public string Description => "Main menu loading...";

    public async Task Load(Action<float> onProgress)
    {
        onProgress?.Invoke(0.5f);
        var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.MAIN_MENU, LoadSceneMode.Additive);
        while (loadOp.isDone == false)
        {
            await Task.Delay(1);
        }
        onProgress?.Invoke(1f);
    }
}