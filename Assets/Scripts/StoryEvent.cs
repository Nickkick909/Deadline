using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEvent : MonoBehaviour
{
    public List<EventAction> actions;

    private int index = 0;

    public void Play()
    {
        Debug.Log("Starting Story Event");
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        while (index < actions.Count)
        {
            Debug.Log("Story index: " + index);
            yield return actions[index].Execute();
            index++;
        }
    }
}
