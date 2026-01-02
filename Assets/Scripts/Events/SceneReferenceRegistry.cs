using System.Collections.Generic;
using System;
using UnityEngine;

public class SceneReferenceRegistry : MonoBehaviour
{
    public static SceneReferenceRegistry Instance { get; private set; }

    [Serializable]
    public class Entry
    {
        public string key;
        public UnityEngine.Object reference;
    }

    public List<Entry> entries = new();

    void Awake()
    {
        Instance = this;
    }

    public T Get<T>(string key) where T : UnityEngine.Object
    {
        foreach (var entry in entries)
        {
            if (entry.key == key)
                return entry.reference as T;
        }

        Debug.LogError($"Registry: Key '{key}' not found.");
        return null;
    }
}
