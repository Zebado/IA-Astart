using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public Vector3 targetPosition;
    public Action<Vector3> TargetIsVisible;
    public float speed = 5;
    public float viewRadius = 2f;

    public static EnemiesManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
}

