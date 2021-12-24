using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Small script that executes something after an audiosource is done playing
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class DoSomethingAfterPlay : MonoBehaviour
{
    AudioSource _src;
    [Tooltip("What should be executed")]
    [SerializeField]
    UnityEvent _func;
    private void Awake()
    {
        _src = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!_src.isPlaying)
        {
            _func.Invoke();
        }
    }
}
