using UnityEngine;

public class StartButtonUnlock : MonoBehaviour
{
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
