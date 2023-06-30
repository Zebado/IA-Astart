using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTargetState : State
{
    Enemie _enemy;
    FOV _fov;
    ChaseState _chaseState;
    PathFindingState _pathFindingState;

    public GoToTargetState(Enemie enemy, FOV fov)
    {
        _enemy = enemy;
        _fov = fov;
    }
    public void OnEnter()
    {
        EnemiesManager.instance.TargetIsVisible += IsVisible;
    }
    private void IsVisible(Vector3 obj)
    {

        if (LOS.InLineOfSight(_enemy.transform.position, obj, GameManager.instance.BlockedNodeLayer))
        {
            _enemy.ChangeState(this);
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
            _enemy.transform.forward = EnemiesManager.instance.targetPosition - _enemy.transform.position;
            _enemy.transform.position += (EnemiesManager.instance.targetPosition - _enemy.transform.position).normalized * EnemiesManager.instance.speed * Time.deltaTime;
        }
    }

    public void OnExit()
    {
        EnemiesManager.instance.TargetIsVisible -= IsVisible;

    }

    internal void Initialized(PathFindingState pathFindingState, ChaseState chaseState)
    {
        _pathFindingState = pathFindingState;
        _chaseState = chaseState;
    }
}
