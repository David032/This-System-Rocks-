using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectiles : MonoBehaviour
{
    //public Transform RocketTarget;
    [HideInInspector]
    public GameObject MissileTarget;
    public Rigidbody MissileRB;

    public float turnSpeed = 1f;
    public float missileFlySpeed = 10f;

    private float iTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        //if (!MissileTarget)
        // Debug.Log("Please Set the Target");

        //MissileTarget = GameObject.FindGameObjectWithTag("target");

        //MissileLocalTrans = GetComponent<Transform>();
    }

    private void Update()
    {
        if(iTime > 0.0f)
            iTime -= Time.deltaTime;
    }

    void Fire()
    {
        if (!MissileTarget)
        {
            Destroy(gameObject);
            return;
        }

        MissileRB.velocity = transform.forward * missileFlySpeed;

            var missileTargetRotation = Quaternion.LookRotation(MissileTarget.transform.position - transform.position);

        MissileRB.MoveRotation(Quaternion.RotateTowards(transform.rotation, missileTargetRotation, turnSpeed));

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if (Input.GetKey(KeyCode.Space))
        // {
        //     Fire();
        // }
        

        Fire();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (iTime > 0.0f) return;
        
        if (col.gameObject.tag == "Asteroid")
        {
            var activeasteroids = Locator.Instance.AsteroidSpawner.ActiveAsteroids;
            if(activeasteroids.Contains(col.gameObject))
                activeasteroids.Remove(col.gameObject);
            Destroy(col.gameObject); 
        }
        Destroy(gameObject);
    }
}
