using UnityEngine;
/// <summary>
/// Unlocking mouse on main menu after play
/// </summary>
public class UnlockMouse : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
