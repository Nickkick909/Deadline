using UnityEngine;

public class StartStoryEvent : MonoBehaviour
{
    public StoryEvent storyEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            storyEvent.Play();
        }
    }
}
