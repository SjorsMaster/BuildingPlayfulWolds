using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Small script for the button based off the trigger system for easy inspector execution
/// 
/// https://docs.unity3d.com/ScriptReference/Material-ctor.html
/// Used as reference for material
/// </summary>

public class ActivatorButton : AboveChecker
{
    bool _active = false;
    Material _baseMat;

    [Tooltip("Colors for the states")]
    [SerializeField]
    Color _positiveColor = Color.green, _negativeColor = Color.red;

    [Tooltip("What should happen when active/inactive")]
    [SerializeField]
    UnityEvent _onActive, _onInactive;

    [Tooltip("Color for the light")]
    [SerializeField]
    Light _light;

    //Fetch material
    public override void AwakeActions()
    {
        base.AwakeActions();
        _baseMat = GetComponent<Renderer>().material;
    }

    public override void Hit()
    {
        if (_light) _light.color = _positiveColor;
        _active = true;
        _baseMat.color = _positiveColor;
        _onActive.Invoke();
    }

    public override void NothingHit()
    {
        if (_active)
        {
            _onInactive.Invoke();
            _active = false;
        }
        if (_light) _light.color = _negativeColor;
        _baseMat.color = _negativeColor;
    }
}