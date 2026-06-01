using UnityEngine;
using UnityEngine.InputSystem;

public class PauseInvoker : MonoBehaviour
{
    public GameObject pause_menu;
    private bool active = false;
    private void Update()
    {
        if (!active && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            active= true;
            pause_menu.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (active && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            active = false;
            pause_menu.SetActive(false);
            Time.timeScale = 1f;
        }

    }
}
