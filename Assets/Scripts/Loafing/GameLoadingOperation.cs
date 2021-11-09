using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoadingOperation : ILoadingOperation
{
    public string Description => "Game loading...";

    public async Task Load(Action<float> onProgress)
    {
        onProgress?.Invoke(0.5f);
        var loadOp = SceneManager.LoadSceneAsync(Constants.Scenes.GAME, LoadSceneMode.Single);
        while (loadOp.isDone == false)
        {
            await Task.Delay(1);
        }
        var scene = SceneManager.GetSceneByName(Constants.Scenes.GAME);
        var game = scene.GetRoot<Game>();
        onProgress?.Invoke(.9f);

        //int levelNumber;
        //if (PlayerPrefs.HasKey(Constants.LEVEL_KEY))
        //{
        //    levelNumber = PlayerPrefs.GetInt(Constants.LEVEL_KEY);
        //}
        //else
        //{
        //    levelNumber = 0;
        //    PlayerPrefs.SetInt(Constants.LEVEL_KEY, levelNumber);
        //}

        game.Initialize();
        onProgress?.Invoke(1.0f);
    }
}