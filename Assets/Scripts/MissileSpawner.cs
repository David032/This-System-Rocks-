using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    public GameObject Rocket;

    private float cooldown = 0.0f;

    private void Awake()
    {
        //InvokeRepeating("SpawnMissile", 5, 5);
    }

    private void Update()
    {
        if(cooldown > 0.0f)
            cooldown -= Time.deltaTime;
        
    }

    void SpawnMissile()
    {
        GameObject newMissile = Instantiate(Rocket, transform.position, Quaternion.identity);
    }
    
    public void SpawnMissile(GameObject target)
    {
        if (cooldown > 0.0f)
            return;
        
        GameObject newMissile = Instantiate(Rocket, transform.position, Quaternion.identity);
        newMissile.GetComponent<HomingProjectiles>().MissileTarget = target;
        cooldown = 2.0f;
        Debug.Log("Fired Missile");

    }
}
