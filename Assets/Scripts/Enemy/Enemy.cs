using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System.Linq;
/// <summary>
/// Regular enemy behaviour, attacks when player is in sight and close enough
/// 
/// Sources used:
/// https://forum.unity.com/threads/cast-ray-from-one-specific-object-to-another.55188/
/// https://docs.unity3d.com/Manual/nav-BuildingNavMesh.html
/// https://docs.unity3d.com/ScriptReference/RequireComponent.html
/// https://answers.unity.com/questions/697830/how-to-calculate-direction-between-2-objects.html
/// </summary>

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : EnemyBaseClass
{
    //Navmesh
    [Tooltip("The points that the AI will walk, not used when agent is set to Idle.")]
    [SerializeField]
    GameObject[] _points;
    NavMeshAgent _agent;
    int _currentDest;
    bool _wasDistracted;
    [Tooltip("Randomly pick between points")]
    [SerializeField]
    bool _randomTarget = true;
    [SerializeField]
    [Tooltip("Tweakability of AI")]
    float _thinkFor = 5f, _distractionDistance = 10f, _undistractable = 5f, _rememberOffset = 0.5f, _attackingDistance = 1, _visionDistance = 5, _damageDealt = 20;
    //

    //StateMachine
    EnumState _lastState;
    RaycastHit _hit;
    int _layerMask = 1 << 8;
    List<Distraction> Distractions = new List<Distraction>();
    Distraction _distractedBy;
    Vector3 _lastKnownPos, _lastPos, _origin, _lastDistractPos;
    //

    //PlayerReferences
    GameObject _player;
    PlayerHealth _hpMod;
    //

    //Generic
    [Tooltip("Speed of the AI")]
    [SerializeField]
    float _mySpeed = 3.5f;
    [Tooltip("Define if these ai require a player in scene or not")]
    [SerializeField]
    bool _titleScreen;
    //

    //Set up the AI
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _layerMask = ~_layerMask;
        _lastKnownPos = transform.position;
        if (GameObject.FindObjectsOfType<Distraction>().Length > 0)
            Distractions = (GameObject.FindObjectsOfType<Distraction>().ToList());
        if (!_titleScreen) _player = (GameObject.FindObjectsOfType<ActiveInstance>()[0].gameObject);
        if (!_titleScreen) _hpMod = (_player.GetComponent<PlayerHealth>());
        _agent.speed = _mySpeed;
        _origin = transform.position;
        if (_points.Length <= 0) _state = EnumState.Idling;
        if (_state == EnumState.Idling) return;
        _currentDest = Random.Range(0, _points.Length);
        NextPoint();
    }

    //Keep track of the distractions
    private void CheckDistractions()
    {
        if (Distractions.Count == 0) return;
        foreach (Distraction distraction in Distractions)
        {
            if (Vector3.Distance(transform.position, distraction.transform.position) <= _distractionDistance && Vector3.Distance(distraction.transform.position, _lastDistractPos) > _rememberOffset && !_wasDistracted)
            {
                _agent.SetDestination(distraction.transform.position);
                _distractedBy = distraction;
                _lastDistractPos = distraction.transform.position;
                _state = EnumState.Distracted;
            }
        }
    }

    //Walk from point to point
    protected override void Patroling()
    {
        _lastState = _state; //Remember state
        if (Vector3.Distance(_agent.destination, transform.position) <= _agent.stoppingDistance || transform.position == _lastPos)
        {
            NextPoint();
        }
        if (CanSeePlayer()) _state = EnumState.Attacking;
        CheckDistractions();
        _lastPos = transform.position; //Avoid character being stuck
    }

    //Fetch next point
    private void NextPoint()
    {
        _currentDest = _randomTarget ? Random.Range(0, _points.Length) : (_currentDest + 1 >= _points.Length ? 0 : _currentDest + 1);
        _agent.SetDestination(_points[_currentDest].transform.position);
    }

    //Being distracted
    IEnumerator Inspect()
    {
        _wasDistracted = true;
        yield return new WaitForSeconds(_thinkFor);
        _state = _lastState; //Remember state
        _agent.SetDestination(_lastPos);
        yield return new WaitForSeconds(_undistractable);
        _wasDistracted = false;
    }

    //Standing still until sees player or when is no longer at desired point
    protected override void Idling()
    {
        _lastState = _state;
        if (transform.position != _origin)
        {
            _agent.SetDestination(_origin);
        }
        if (CanSeePlayer()) _state = EnumState.Attacking;
        CheckDistractions();
        _lastPos = _origin;
    }

    //Being distracted and staying close to it
    protected override void Distracted()
    {
        if (Vector3.Distance(_agent.destination, transform.position) <= _agent.stoppingDistance + 1)
        {
            StartCoroutine(Inspect());
        }
        if (Vector3.Distance(transform.position, _distractedBy.transform.position) <= _distractionDistance)
        {
            _agent.SetDestination(_distractedBy.transform.position);
        }
    }

    //Not really used
    protected override void Walking()
    {
        _lastState = _state;
    }

    //See if player is visible within range and not obstructed
    private bool CanSeePlayer()
    {
        if (_titleScreen) return false;
        //A linecast did not seem to work... :(
        return (Physics.Raycast(transform.position, (_player.transform.position - transform.position), out _hit, _visionDistance) && Vector3.Distance(_player.transform.position, transform.position) < _visionDistance && _hit.transform.GetComponent<Player>());
    }

    //Drain health and stay close, when lost track of player, go to last known position
    protected override void Attacking()
    {
        CheckDistractions();
        if (CanSeePlayer())
        {
            _agent.speed = _mySpeed * 2;
            _lastKnownPos = _player.transform.position;
            if (Vector3.Distance(_player.transform.position, transform.position) < _attackingDistance)
            {
                _hpMod.UpdateHP(-_damageDealt * Time.deltaTime);
            }
            _agent.SetDestination(_player.transform.position);
        }
        else
        {
            _agent.SetDestination(_lastKnownPos);
            if (transform.position == _lastKnownPos || transform.position == _lastPos)
            {
                _agent.speed = _mySpeed;
                _state = _lastState;
            }
            _lastPos = transform.position; //Avoid character being stuck
        }
    }
}
