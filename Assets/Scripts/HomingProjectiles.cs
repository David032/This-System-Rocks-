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

    private Transform rocketLocalTrans;

    // Start is called before the first frame update
    void Start()
    {
        if (!MissileTarget)
         Debug.Log("Please Set the Target");

        MissileTarget = GameObject.FindGameObjectWithTag("target");

        rocketLocalTrans = GetComponent<Transform>();
    }

    void Fire()
    {
        if (!MissileRB)
            return;
        MissileRB.velocity = rocketLocalTrans.forward * missileFlySpeed;

            var missileTargetRotation = Quaternion.LookRotation(MissileTarget.transform.position - rocketLocalTrans.position);

        MissileRB.MoveRotation(Quaternion.RotateTowards(rocketLocalTrans.rotation, missileTargetRotation, turnSpeed));

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
