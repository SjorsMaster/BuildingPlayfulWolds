using UnityEngine;
/// <summary>
/// Player inputmanager that does actions based on input to inform all the other relevant scripts.
/// Should consider using delegates for these, but I'm short on time...
/// </summary>
public class PlayerInput : MonoBehaviour
{
    [Tooltip("Mouse sensivity")]
    [SerializeField]
    float _sensivity = 10.5f;
    float _jumpStrenght = -0.2f, _jumpHang = -0.45f, _currentBoost, _fallingTresh;
    bool _jumped;

    [Tooltip("What interacts on click")]
    [SerializeField]
    InteractWith _interactionManager;

    [Tooltip("What controls the player")]
    [SerializeField]
    PlayerMovement _movementManager;

    [Tooltip("Player itself")]
    [SerializeField]
    GameObject _player;

    [Tooltip("Physics")]
    [SerializeField]
    GeneralSettings _generalVariables;

    //Set up stuff
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        FetchVariables();
    }

    //Fetch physics
    void FetchVariables()
    {
        //use delegate to fetch
        _fallingTresh = _generalVariables.fallingTresh;
    }

    // Convey all input data to other actions
    void Update()
    {
        if (Input.GetButton("Jump"))
        {
            _currentBoost = _jumped ? _fallingTresh : _jumpStrenght;
            _player.GetComponent<GeneralPhysics>().ChangeVelocity(_jumpHang, _currentBoost); ///USE DELEGATE INSTEAD OF GET COMPONENT
            if (!_jumped) { _jumped = true; }
        }
        else { _jumped = false; }
        if (Input.GetButtonDown("Fire1"))
            _interactionManager.Interacting();
        if (Input.GetButton("Sprint"))
            _movementManager.SetSprint(true);
        else
            _movementManager.SetSprint(false);
        _movementManager.SetWalk(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _movementManager.SetLook(Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * _sensivity + Input.GetAxis("Roll"), -Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * _sensivity + Input.GetAxis("Pitch"));
    }
}
