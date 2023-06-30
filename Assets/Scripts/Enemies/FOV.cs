using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    [SerializeField] float _viewRadius;
    [SerializeField] float _viewAngle;

    void FixedUpdate()
    {
        if (InFieldOfView(EnemiesManager.instance.targetPosition))
        {
            Debug.DrawLine(transform.position, EnemiesManager.instance.targetPosition, Color.red);
        }
    }

    public bool InFieldOfView(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;

        if (dir.sqrMagnitude > _viewRadius * _viewRadius) return false;

        if (!LOS.InLineOfSight(transform.position, targetPos, GameManager.instance.BlockedNodeLayer)) return false;

        return Vector3.Angle(transform.forward, dir) <= _viewAngle / 2;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);

        var realAngle = _viewAngle / 2;

        Vector3 lineLeft = GetDirFromAngle(-realAngle + transform.eulerAngles.y);
        Vector3 lineRight = GetDirFromAngle(realAngle + transform.eulerAngles.y);

        Gizmos.DrawLine(transform.position, transform.position + lineLeft * _viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + lineRight * _viewRadius);
    }
    Vector3 GetDirFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
