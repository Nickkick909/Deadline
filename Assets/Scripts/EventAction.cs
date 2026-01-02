using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "EventAction", menuName = "Scriptable Objects/EventAction")]
public abstract class EventAction : ScriptableObject
{
    public abstract IEnumerator Execute();
}
