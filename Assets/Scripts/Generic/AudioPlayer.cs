using UnityEngine;
/// <summary>
/// Small audio manager that can be used from the inspector
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    AudioSource _src;
    private void Awake()
    {
        _src = GetComponent<AudioSource>();
    }
    /// <summary>
    /// Play a specific audioclip
    /// </summary>
    /// <param name="clip">audioclip</param>
    public void PlayClip(AudioClip clip) {
        _src.clip = clip;
        _src.Play();
    }
}
