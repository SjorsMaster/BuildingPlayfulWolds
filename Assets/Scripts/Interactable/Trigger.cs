using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Script used for triggers for player
/// </summary>
public class Trigger : AboveChecker
{
    [Tooltip("What should execute upon hit player")]
    [SerializeField]
    UnityEvent _action;

    public override void Hit()
    {
        if (_hit.transform.GetComponent<Player>())
        {
            _action.Invoke();
            Destroy(this.gameObject);
        }
    }

}
