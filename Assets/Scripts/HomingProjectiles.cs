using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectiles : MonoBehaviour
{
    public Transform RocketTarget;
    public Rigidbody RocketRB;

    public float turnSpeed = 1f;
    public float rocketFlySpeed = 10f;

    private Transform rocketLocalTrans;

    // Start is called before the first frame update
    void Start()
    {
        if (!RocketTarget)
         Debug.Log("Please Set the Target");

        rocketLocalTrans = GetComponent<Transform>();
    }

    void Fire()
    {
        if (!RocketRB)
            return;
            RocketRB.velocity = rocketLocalTrans.forward * rocketFlySpeed;

            var rocketTargetRotation = Quaternion.LookRotation(RocketTarget.position - rocketLocalTrans.position);

            RocketRB.MoveRotation(Quaternion.RotateTowards(rocketLocalTrans.rotation, rocketTargetRotation, turnSpeed));

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
