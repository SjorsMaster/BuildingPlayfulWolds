using UnityEngine;
/// <summary>
/// Script that makes sure there's only one player
/// </summary>
public class DestroyPlayerInstance : MonoBehaviour
{
    private void Awake()
    {
        if (GameObject.FindObjectsOfType<ActiveInstance>().Length != 0)
            Destroy(GameObject.FindObjectsOfType<ActiveInstance>()[0].gameObject);
    }
}
