using UnityEngine;
/// <summary>
/// Script for handling the movement of the player
/// 
/// Sources:
/// https://docs.unity3d.com/ScriptReference/Cursor-lockState.html
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    //Data
    [Tooltip("General settings")]
    [SerializeField]
    float _rcLength, _speedMod = 4f, _sprintSpeed = 12f;
    [Tooltip("Camera of the player")]
    [SerializeField]
    GameObject _cameraPlayer;
    Vector2 _playerMovement, _playerLooking;
    Vector3 _lookDir;
    //

    //Sprinting
    //Local reference to the default speed
    float _defaultSpeed;
    bool _sprinting;
    //

    //Head stuff
    [Tooltip("Player's head for getting proper forward direction")]
    [SerializeField]
    GameObject _head;
    int _range = 90;
    //


    //Prepare stuff
    private void Awake()
    {
        _defaultSpeed = _speedMod;
        if (!_cameraPlayer) _cameraPlayer = Camera.main.gameObject;
        _rcLength = transform.localScale.x / 2;
    }

    //Look at input, do stuff based on that
    private void Update()
    {
        _head.transform.localEulerAngles = new Vector3(0 - _cameraPlayer.transform.localEulerAngles.x, 0, 0 - _cameraPlayer.transform.localEulerAngles.z);

        _lookDir += new Vector3(_playerLooking.x, _playerLooking.y);
        _lookDir.y = Mathf.Clamp(_lookDir.y, -_range, _range);
        _cameraPlayer.transform.localRotation = Quaternion.Euler(_lookDir.y, _lookDir.x, 0);

        _speedMod = _sprinting ? _sprintSpeed : _defaultSpeed;
        MovePlayer();
    }

    //Walking variables
    public void SetWalk(float inputA, float inputB)
    {
        _playerMovement.x = inputA;
        _playerMovement.y = inputB;
    }
    //Looking variables
    public void SetLook(float inputA, float inputB)
    {
        _playerLooking.x = inputA;
        _playerLooking.y = inputB;
    }
    //Check if we're sprinting or not
    public void SetSprint(bool input)
    {
        _sprinting = input;
    }

    //Fix forward based on camera
    private void MovePlayer()
    {
        if (_playerMovement.x != 0) //a
            MovePlayer(_playerMovement.x, Vector3.right);
        if (_playerMovement.y != 0) //a
            MovePlayer(_playerMovement.y, Vector3.forward);
        SideCorrectors(Vector3.right);
        SideCorrectors(Vector3.forward);
    }

    //Push out stuck characters
    private void SideCorrectors(Vector3 direction)
    {
        Vector3 currentPos = transform.position;
        bool a = CheckCollider(_head.transform.position, -direction);
        bool b = CheckCollider(_head.transform.position, direction);
        if (a || b) transform.localPosition -= (direction == Vector3.forward ? _head.transform.forward : _head.transform.right) * (a ? -.5f : .5f) * Time.deltaTime * _speedMod;
    }

    //Move around the player
    private void MovePlayer(float movement, Vector3 direction)
    {
        bool a = !CheckCollider(_head.transform.position, -direction);
        bool b = !CheckCollider(_head.transform.position, direction);
        if (a && b) transform.localPosition += (direction == Vector3.forward ? _head.transform.forward : _head.transform.right) * (movement < 0 ? -Mathf.Abs(movement) : Mathf.Abs(movement)) * Time.deltaTime * _speedMod;
    }

    //This sends out two raycasts, one for the upper part of the body, one for the lower part. This is for colission.
    private bool CheckCollider(Vector3 position, Vector3 direction)
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        return (Physics.Raycast(position/* + transform.up * ((transform.localScale.y / 2) + _rayOffset)*/, _head.transform.TransformDirection(direction), _rcLength * 1.15f, layerMask) || Physics.Raycast(position - (transform.up * ((transform.localScale.y / 2))), _head.transform.TransformDirection(direction), _rcLength * 1.15f, layerMask)) || Physics.Raycast(position - (transform.up * (transform.localScale.y)), _head.transform.TransformDirection(direction), _rcLength * 0.8f, layerMask);
    }
}
