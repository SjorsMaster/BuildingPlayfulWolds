using UnityEngine;
/// <summary>
/// Script for adding HP to the player callable from inspector
/// </summary>
public class AddHealth : MonoBehaviour
{
    [Tooltip("How much health should be added")]
    [SerializeField]
    float _amount;

    public void AddHP()
    {
        GameObject tmp = GameObject.FindObjectsOfType<PlayerHealth>()[0].gameObject;
        if (tmp)
        {
            tmp.GetComponent<PlayerHealth>().UpdateHP(_amount);
        }
    }
}
