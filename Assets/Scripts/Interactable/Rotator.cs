using UnityEngine;
/// <summary>
/// Script for flipping the player when above
/// </summary>
public class Rotator : AboveChecker
{
    GameObject _lastHit;
    public override void Hit()
    {
        //Check if can be flipped
        if (_hit.transform.GetComponent<GeneralPhysics>() && _lastHit != _hit.transform.gameObject)
        {
            //Flip object above
            Vector3 tmp = _hit.transform.eulerAngles - transform.eulerAngles;
            //I kinda wanna lerp the camera, but unsure as to how
            _hit.transform.GetComponent<GeneralPhysics>().ChangeDownward(-tmp + new Vector3(0, 0, 180), true);
            _lastHit = _hit.transform.gameObject;
        }
    }

    public override void NothingHit()
    {
        _lastHit = null;
    }
}
