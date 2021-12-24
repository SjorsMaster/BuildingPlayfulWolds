using UnityEngine;
/// <summary>
/// Small script that allows for grabbing and holding objects
/// </summary>
public class InteractWith : MonoBehaviour
{
    //Raycast stuff
    float _rcLength = 5f;
    RaycastHit _hit;
    int _layerMask = 1 << 8;
    //

    [Tooltip("The body of the player")]
    [SerializeField]
    GameObject _body;
    GameObject _other;

    //Setup layermask
    private void Awake()
    {
        _layerMask = ~_layerMask;
    }

    //Interaction function for others to reach
    public void Interacting()
    {
        Interact();
    }

    //Make sure holding object doesn't go through walls
    private void Update()
    {
        if (_other)
        {
            if (CastCheck(.45f))
            {
                _other.transform.position = _hit.point - (transform.forward * .45f);
            }
            else
            {
                _other.transform.position = transform.position + (transform.forward * 2);
                _other.transform.eulerAngles = _body.transform.rotation.eulerAngles;
            }
        }
    }

    //Check if holding or seeking
    void Interact()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * _rcLength, Color.red);
        if (_other)
        {
            QuickToggle(false);
            _other = null;
        }
        else if (CastCheck())
        {
            if (_hit.transform.GetComponent<PickUp>())
            {
                _other = _hit.collider.gameObject;
                QuickToggle(true);
            }
        }
    }

    //Disable collider to prevent flying and other issues
    void QuickToggle(bool state)
    {
        if (_other.GetComponent<GeneralPhysics>()) _other.GetComponent<GeneralPhysics>()._toggleFreeze(state);
        _other.transform.GetComponent<Collider>().enabled = !state;
    }

    //Check if hitting somethign without needing to put in a modifier
    private bool CastCheck()
    {
        return CastCheck(1);
    }

    /// <summary>
    /// Check if hitting something
    /// </summary>
    /// <param name="modifier">Extra lenght or not</param>
    /// <returns></returns>
    private bool CastCheck(float modifier)
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * _rcLength * modifier, Color.blue);
        return (Physics.Raycast((transform.position), transform.TransformDirection(Vector3.forward), out _hit, _rcLength * modifier, _layerMask));
    }
}