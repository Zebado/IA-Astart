using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathFindingState : State
{
    Enemie _enemy;
    PathFinding _pathFinding;
    Stack<Node> _path;
    Transform nextWaypoint;
    PatrolState _patrolState;
    FOV _fov;
    ChaseState _chaseState;
    GoToTargetState _goToTargetState;

    public PathFindingState(Enemie enemy, FOV fov)
    {
        _enemy = enemy;
        _fov = fov;
    }
    public void OnEnter()
    {
        nextWaypoint = null;
        _pathFinding = new PathFinding();
        EnemiesManager.instance.TargetIsVisible += IsVisible;
    }

    public void OnUpdate()
    {
        if (_fov.InFieldOfView(GameManager.instance.target.transform.position))
        {
            _enemy.ChangeState(_chaseState);
            EnemiesManager.instance.targetPosition = GameManager.instance.target.transform.position;
            EnemiesManager.instance.TargetIsVisible?.Invoke(GameManager.instance.target.transform.position);
        }
        else
            Movement();
    }

    public void OnExit()
    {
        EnemiesManager.instance.TargetIsVisible -= IsVisible;
    }
    void Movement()
    {
        if (nextWaypoint == null)
        {
            nextWaypoint = GetWayPoint();
        }

        Vector3 dir = nextWaypoint.position - _enemy.transform.position;
        dir.y = 0;
        _enemy.transform.forward = dir;

        if (Vector3.Distance(_enemy.transform.position, nextWaypoint.position) >= EnemiesManager.instance.viewRadius)
        {
            _enemy.transform.position += _enemy.transform.forward * EnemiesManager.instance.speed * Time.deltaTime;
        }
        else if (_path.Count > 0)
        {
            nextWaypoint = _path.Pop().transform;
        }
        else if (_path.Count <= 0)
            _enemy.ChangeState(_patrolState);

    }
    private Transform GetWayPoint()
    {
        var targetNode = NodeManager.instance.NodeClosetIsPosition(EnemiesManager.instance.targetPosition, GameManager.instance.BlockedNodeLayer);
        var myNode = NodeManager.instance.NodeClosetIsPosition(_enemy.transform.position, GameManager.instance.BlockedNodeLayer);
        _path = _pathFinding.AStar(myNode, targetNode);
        if (_path != null && _path.Count > 0)
        {
            nextWaypoint = _path.Pop().transform;
        }

        return nextWaypoint;
    }

    internal void Initialized(PatrolState patrolState, ChaseState chaseState, GoToTargetState goToTarget)
    {
        _patrolState = patrolState;
        _chaseState = chaseState;
        _goToTargetState = goToTarget;
    }
    private void IsVisible(Vector3 obj)
    {

        if (LOS.InLineOfSight(_enemy.transform.position, obj, GameManager.instance.BlockedNodeLayer))
        {
            _enemy.ChangeState(_goToTargetState);
        }
        else
        {
            _enemy.ChangeState(this);
        }
    }
}
