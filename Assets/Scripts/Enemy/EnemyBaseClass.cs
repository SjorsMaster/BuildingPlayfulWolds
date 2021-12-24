using UnityEngine;

/// <summary>
/// As the title describes, enemy base class.
/// Wasn't entirely sure as to how to build states with functions, so used enum.
/// </summary>
public abstract class EnemyBaseClass : MonoBehaviour
{
    protected enum EnumState { Patroling, Idling, Distracted, Walking, Attacking }
    [Tooltip("Current State")]
    [SerializeField]
    protected EnumState _state;

    void ExecuteState()
    {
        switch (_state)
        {
            default:
            case EnumState.Patroling: Patroling(); break;
            case EnumState.Idling: Idling(); break;
            case EnumState.Distracted: Distracted(); break;
            case EnumState.Walking: Walking(); break;
            case EnumState.Attacking: Attacking(); break;
        }
    }

    protected abstract void Patroling();
    protected abstract void Idling();
    protected abstract void Distracted();
    protected abstract void Walking();
    protected abstract void Attacking();

    void Update()
    {
        ExecuteState();
    }
}
