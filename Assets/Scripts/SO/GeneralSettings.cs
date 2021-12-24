using UnityEngine;

/// <summary>
/// Small scriptable object to keep all physics the same
/// 
/// Sources:
/// http://www.unitygeek.com/delegates-events-unity/
/// </summary>
[CreateAssetMenu(fileName = "Physics Settings", menuName = "Global Game Settings/Level Physics", order = 1)]
public class GeneralSettings : ScriptableObject
{
    //Delegate for updating physics over everything
    public delegate void PushValuesDelegate();
    public static PushValuesDelegate pushValuesDelegate;

    [Tooltip("Gravity strenght overtime\nDefault: 0.8f")]
    public float currentGravity = 0.8f;
    [Tooltip("Maximum falling speed\nDefault: 2f")]
    public float velocityLimit = 2f;
    [Tooltip("Value to avoid clipping on the Y-axis on high speeds\nDefault: 0.1f")]
    public float antiClip = 0.1f;
    [Tooltip("How quickly the character adjusts to the new rotation\nDefault: 12f")]
    public float transitionSpeed = 12f;
    [Tooltip("Under which the object is considered falling\nDefault: 0f")]
    public float fallingTresh = 0f;// FETCH THIS FROM SCRIPTABLE OBJECT
    [Tooltip("Multiplier to which the current velocity slows down with when going up\nDefault: 2f")]
    public float headbumpDampen = 0.1f;

    private void Awake()
    {
        Push();
    }

    //Fetch when pushed
    public void Push()
    {
        if (pushValuesDelegate != null)
            pushValuesDelegate.Invoke();
    }
}
