using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectiles : MonoBehaviour
{
    //public Transform RocketTarget;
    public GameObject MissileTarget;
    public Rigidbody MissileRB;

    public float turnSpeed = 1f;
    public float missileFlySpeed = 10f;

    private Transform MissileLocalTrans;

    // Start is called before the first frame update
    void Start()
    {
        if (!MissileTarget)
         Debug.Log("Please Set the Target");

        MissileTarget = GameObject.FindGameObjectWithTag("target");

        MissileLocalTrans = GetComponent<Transform>();
    }

    void Fire()
    {
        if (!MissileRB)
            return;
        MissileRB.velocity = MissileLocalTrans.forward * missileFlySpeed;

            var missileTargetRotation = Quaternion.LookRotation(MissileTarget.transform.position - MissileLocalTrans.position);

        MissileRB.MoveRotation(Quaternion.RotateTowards(MissileLocalTrans.rotation, missileTargetRotation, turnSpeed));

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Fire();
        }

    }

    private void OnCollisionEnter(Collision col) 
    {
        Destroy(gameObject);
    }
}
