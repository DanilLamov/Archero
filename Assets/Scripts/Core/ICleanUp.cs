using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICleanUp
{
    IEnumerable<GameObjectFactory> Factories { get; }
    string SceneName { get; }
    void Cleanup();
}