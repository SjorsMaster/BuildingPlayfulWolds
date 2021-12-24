using UnityEngine;
/// <summary>
/// Shoot object above up by playing with the physics script
/// </summary>
public class Launcher : AboveChecker
{
    [Tooltip("Modify velocity, could be in either direction but mainly used in the negative range")]
    [SerializeField]
    float _launchStrenght = -0.5f;

    public override void Hit()
    {
        if (_hit.transform.GetComponent<GeneralPhysics>())
            _hit.transform.GetComponent<GeneralPhysics>().ChangeVelocity(0, _launchStrenght);
    }
}