using System;
using UnityEngine;

[Serializable]
public class SceneReference
{
    public string key;  // Matches a key in the SceneReferenceRegistry

    public T Get<T>() where T : UnityEngine.Object
    {
        return SceneReferenceRegistry.Instance.Get<T>(key);
    }
}