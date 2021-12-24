using UnityEngine;

/// <summary>
/// Small base script used for checking if something is above the object
/// </summary>
public abstract class AboveChecker : MonoBehaviour
{
    [Tooltip("Lenght of the raycast and how far the offeset of the start needs to be on the Y axis")]
    [SerializeField]
    protected float _rcLength = 2f, _offset = 0;
    protected RaycastHit _hit;
    protected int _layerMask = 1 << 8;

    //Done like this so you can still use awake in other classes without ignoring layermasks
    private void Awake()
    {
        AwakeActions();
    }

    //Setting up layermask
    /// <summary>
    /// Actions you can do when in awake, please include base for layermask
    /// </summary>
    public virtual void AwakeActions()
    {
        _layerMask = ~_layerMask;
    }

    //Regular update loop keeping track of hitting something with a raycast
    //Keeping debug in, should remove automatically when unity compiles
    public virtual void Update()
    {
        Debug.DrawRay(transform.position + transform.up * _offset, transform.TransformDirection(Vector3.up) * _rcLength, Color.red);
        //Might consider using box casts
        if (Physics.Raycast((transform.position + transform.up * _offset), transform.TransformDirection(Vector3.up), out _hit, _rcLength, _layerMask))
        {
            Hit();
        }
        else
        {
            NothingHit();
        }
    }

    /// <summary>
    /// What should happen when nothing is hit
    /// </summary>
    public virtual void NothingHit() { }
    /// <summary>
    /// What should happen when something is hit
    /// </summary>
    public abstract void Hit();
}
