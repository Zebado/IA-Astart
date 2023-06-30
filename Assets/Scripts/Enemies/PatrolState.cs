using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PatrolState : State
{
    List<Node> _wayPoint;
    Stack<Node> _path;
    Enemie _enemy;
    int currentIndex = 0;
    FOV _fov;
    ChaseState _chaseState;
    GoToTargetState _targetState;
    PathFindingState _pathFindingState;
    public event Action<bool> targetVisibility;
    PathFinding _pathFinding;
    Transform nextNode;

    public PatrolState(Enemie enemy, FOV fov)
    {
        _enemy = enemy;
        _wayPoint = enemy.wayPoints;
        _fov = fov;
    }

    public void OnEnter()
    {
        EnemiesManager.instance.TargetIsVisible += IsVisible;
        _pathFinding = new PathFinding();
        SetPath();
        nextNode = _path.Pop().transform;
    }

    private void IsVisible(Vector3 obj)
    {

        if (LOS.InLineOfSight(_enemy.transform.position, obj, GameManager.instance.BlockedNodeLayer))
        {
            _enemy.ChangeState(_targetState);
        }
        else
        {
            _enemy.ChangeState(_pathFindingState);
        }
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
        {
            Movement();
        }
    }

    public void OnExit()
    {
        EnemiesManager.instance.TargetIsVisible -= IsVisible;

    }
    void Movement()
    {
        Vector3 dir = nextNode.position - _enemy.transform.position;
        dir.y = 0;

        _enemy.transform.forward = dir;

        if (Vector3.Distance(_enemy.transform.position, nextNode.position) > EnemiesManager.instance.viewRadius)
        {
            _enemy.transform.position += _enemy.transform.forward * EnemiesManager.instance.speed * Time.deltaTime;
        }
        else
        {
            if (_path != null && _path.Count <= 0)
            {
                currentIndex++;
                if (currentIndex >= _wayPoint.Count)
                {
                    currentIndex = 0;
                }
                SetPath();
            }
            nextNode = _path.Pop().transform;
        }
    }

    private void SetPath()
    {
        var targetNode = NodeManager.instance.NodeClosetIsPosition(_wayPoint[currentIndex].transform.position, GameManager.instance.BlockedNodeLayer);
        var myNode = NodeManager.instance.NodeClosetIsPosition(_enemy.transform.position, GameManager.instance.BlockedNodeLayer);
        _path = _pathFinding.AStar(myNode, targetNode);
    }

    internal void Initialized(ChaseState chaseState, GoToTargetState goToTargetState, PathFindingState pathFindingState)
    {
        _chaseState = chaseState;
        _targetState = goToTargetState;
        _pathFindingState = pathFindingState;
    }
}
