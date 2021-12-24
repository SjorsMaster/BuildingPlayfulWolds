using UnityEngine;
/// <summary>
/// Simple quit application, can be called as event in inspector
/// </summary>
public class Quit : MonoBehaviour
{
    /// <summary>
    /// Quit the application
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }
}
