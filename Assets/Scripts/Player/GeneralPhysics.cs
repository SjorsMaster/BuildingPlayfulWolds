using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a small script that handles the gravity physics
/// There's probably a better way such as trigger points with directions, but it works well enough! (And I thought of it too late!)
/// 
/// I also wanted to avoid using box/sphere colliders, these cause issues with the wall transitions
/// </summary>
public class GeneralPhysics : MonoBehaviour
{
    [Tooltip("How quickly the player updates upon walking a wall")]
    [SerializeField]
    float _transitionSpeed = 12f;

    //Physics data
    float _fallingTresh = 0f, _headbumpDampen = 0.1f, _currentVelocity = 0f, _currentGravity = 0.8f, _velocityLimit = 2f, _antiClip = 0.1f;
    float _rcLength;
    bool _isGrounded = false;

    [Tooltip("Ignore gravity")]
    [SerializeField]
    bool _static;

    [Tooltip("Add an offset to the raycast, Often required when working with Capsules")]
    [SerializeField]
    float _rayOffset = 0;

    [Tooltip("Source physics")]
    [SerializeField]
    GeneralSettings _generalVariables;

    //Freeze object from it's physics
    public void _toggleFreeze(bool state)
    {
        _static = state; 
    }

    //Fetch physics
    public void FetchStuff()
    {
        _fallingTresh = _generalVariables.fallingTresh;
        _currentGravity = _generalVariables.currentGravity;
        _velocityLimit = _generalVariables.velocityLimit;
        _antiClip = _generalVariables.antiClip;
        _transitionSpeed = _generalVariables.transitionSpeed;
        _headbumpDampen = _generalVariables.headbumpDampen;
    }

    // Fetching lenght for the raycast
    void Awake()
    {
        _rcLength = _rcLength != 0 ? _rcLength : transform.localScale.y / 2;
        GeneralSettings.pushValuesDelegate += FetchStuff; //Subscribe to delegate
    }

    private void FixedUpdate()
    {
        Gravity();
    }

    /// <summary>
    /// Small function for changing the velocity
    /// </summary>
    /// <param name="inputVelocity">By how much the velocity should be changed</param>
    public void ChangeVelocity(float repeatedVelocity, float initialVelocity)
    {
        ChangeVelocity(repeatedVelocity, initialVelocity, false);
    }

    /// <summary>
    /// Small function for changing the velocity
    /// </summary>
    /// <param name="inputVelocity">By how much the velocity should be changed</param>
    /// <param name="bypass">Bypass actor being grounded, this does not go for the repeated action</param>
    public void ChangeVelocity(float repeatedVelocity, float initialVelocity, bool bypass)
    {
        if (_isGrounded || bypass) _currentVelocity += initialVelocity;
        if (_currentVelocity < _fallingTresh) _currentVelocity += repeatedVelocity * Time.deltaTime;
    }

    public void ChangeDownward(Vector3 target, bool additive)
    {
        transform.eulerAngles = additive ? target + transform.eulerAngles : target;
    }
    public Quaternion ChangeDownward(Transform begin, Transform target, float speed)
    {
        return Quaternion.Lerp(begin.rotation, target.rotation, Time.deltaTime * speed);
    }

    /// <summary>
    /// Fetches the ground state.
    /// </summary>
    /// <returns>true or false value for the ground state.</returns>
    public bool IsGrounded() { return _isGrounded; }

    /// <summary>
    /// This function handles the gravity physics
    /// </summary>
    void Gravity()
    {
        if (_static) return;

        //Seting layers
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit hit;

        //Update gravity to it's ''Downward'' direction
        transform.localPosition += -transform.up * _currentVelocity;

        //Having a variable raycast helps from the object not clipping through on high speeds
        //Tried to look on the web for a long time for a solution but I couldn't find one, Everyone refers to "Rigibody", which is something I'd like to avoid.
        //While it's not a complext gravity system, it sure was a bit of a brain breaker.
        if (Physics.Raycast((transform.position), transform.TransformDirection(Vector3.down), out hit, _rcLength + _currentVelocity + _rayOffset, layerMask))
        {
            _currentVelocity = _currentVelocity > _fallingTresh ? _currentVelocity * _antiClip : _currentVelocity;
            //Maybe keep the lerp in a function/loop so that it will continue til it's done.
            //As for now, depending on the tag below it varies per speed
            transform.localRotation = ChangeDownward(transform, hit.transform, _transitionSpeed);
            transform.localPosition += -transform.up * (hit.distance - _rayOffset - (transform.localScale.y / 2));
            _isGrounded = true;
        }
        //Update velocity when in air
        else
        {
            if (_currentVelocity < _velocityLimit) _currentVelocity += _currentGravity * Time.deltaTime;
            _isGrounded = false;
        }

        //Making sure the character doesn't clip through the roof when going up
        if (_currentVelocity < 0 && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, _rcLength + _rayOffset - _currentVelocity / 2, layerMask))
        {
            _currentVelocity = _headbumpDampen * _currentGravity;
        }
    }

}
