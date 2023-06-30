using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [field: SerializeField] public LayerMask BlockedNodeLayer { get; private set; }

    public GameObject target;
    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }
}
