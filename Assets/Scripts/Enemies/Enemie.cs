using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class Enemie : MonoBehaviour
{
    [field: SerializeField] public List<Node> wayPoints { get; private set; }
    [SerializeField] EnemiesManager _enemieManager;
    [SerializeField] FOV _fov;
    [SerializeField] LOS _los;

    PatrolState _patrolState;
    ChaseState _chaseState;
    GoToTargetState _goToTargetState;
    PathFindingState _pathFindingState;

    State _currentState;

    private void Start()
    {
        _pathFindingState = new PathFindingState(this, _fov);
        _goToTargetState = new GoToTargetState(this, _fov);
        _chaseState = new ChaseState(this,_fov);
        _patrolState = new PatrolState(this, _fov);

        _pathFindingState.Initialized(_patrolState, _chaseState, _goToTargetState);
        _goToTargetState.Initialized(_pathFindingState, _chaseState);
        _chaseState.Initialized(_patrolState);
        _patrolState.Initialized(_chaseState, _goToTargetState, _pathFindingState);

        ChangeState(_patrolState);
    }


    private void Update()
    {
        _currentState.OnUpdate();
    }

    public void ChangeState(State nextState)
    {
        if (_currentState != null)
        {
            _currentState.OnExit();
        }
        _currentState = nextState;
        _currentState.OnEnter();
    }
}
