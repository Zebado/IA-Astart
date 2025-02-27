using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] float _speed = 5;
    Vector3 _startPosition;
    void Start()
    {
        _startPosition = transform.position;
    }
    void Update()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(-vInput, 0f, hInput).normalized * _speed * Time.deltaTime;

        transform.Translate(movement,Space.World);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = _startPosition;
        }
        if(Mathf.Abs(hInput) > 0.1f || Mathf.Abs(vInput) > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement.normalized, Vector3.up);

            transform.rotation = targetRotation;
        }
    }
}
