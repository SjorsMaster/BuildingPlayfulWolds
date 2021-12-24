using UnityEngine;
/// <summary>
/// Destroy an object from the inspector
/// </summary>
public class Annihilate : MonoBehaviour
{
    /// <summary>
    /// Destroy specific object
    /// </summary>
    /// <param name="toBeDestroyed">gameobject to be destroyed</param>
    public void Annihilation(GameObject toBeDestroyed)
    {
        Destroy(toBeDestroyed);
    }
    /// <summary>
    /// Destroy player
    /// </summary>
    public void AnnihilatePlayer()
    {
        Destroy(GameObject.FindObjectOfType<Player>().gameObject);
    }
}
