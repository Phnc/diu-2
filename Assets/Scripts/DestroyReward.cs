using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyReward : MonoBehaviour
{
    public float timeAlive;

    void Start()
    {
        Destroy(gameObject, timeAlive);
    }
}
