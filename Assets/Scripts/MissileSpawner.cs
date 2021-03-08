using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    public GameObject Rocket;

    private void Awake()
    {
        InvokeRepeating("SpawnMissile", 5, 5);
    }

    void SpawnMissile()
    {
        GameObject newMissile = Instantiate(Rocket, transform.position, Quaternion.identity);
    }
}
