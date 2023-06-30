using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ChaseState : State
{
    Enemie _enemy;
    PatrolState _patrolState;
    FOV _fov;

    public ChaseState(Enemie enemy, FOV fov)
    {
        _enemy = enemy;
        _fov = fov;
    }
    public void OnEnter()
    {
    }
    public void OnUpdate()
    {
        if (_fov.InFieldOfView(GameManager.instance.target.transform.position))
        {
            _enemy.transform.forward = GameManager.instance.target.transform.position - _enemy.transform.position;
            _enemy.transform.position += (GameManager.instance.target.transform.position - _enemy.transform.position).normalized * EnemiesManager.instance.speed * Time.deltaTime;
        }
        else
        {
            _enemy.ChangeState(_patrolState);
        }
    }

    public void OnExit()
    {
    }

    internal void Initialized(PatrolState patrolState)
    {
        _patrolState = patrolState;

    }
}
