using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Small script for switching scenes that can be called from the inspector
/// </summary>
public class LevelLoader : MonoBehaviour
{
    /// <summary>
    /// Go to the next index level
    /// </summary>
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }
    /// <summary>
    /// Do to the previous index level
    /// </summary>
    public void PreviousLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);
    }
    /// <summary>
    /// Go to specific scene index
    /// </summary>
    /// <param name="index">index of the scene</param>
    public void SpecificLevel(int index)
    {
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }
}
