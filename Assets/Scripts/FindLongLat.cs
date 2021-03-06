using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class FindLongLat : MonoBehaviour
{
    private GameObject earth;
    private float countDown = 2.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        earth = GameObject.FindWithTag("Player");
        PrintCity();

    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;
        if (countDown <= 0.0f)
        {
            PrintCity();
            countDown = 2.0f;
        }


        // RaycastHit hit;
        // // Does the ray intersect any objects excluding the player layer
        // if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        // {
        //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        //    // Debug.Log("Did Hit");
        //
        //
        //     Vector3 dirVector = hit.point - hit.collider.gameObject.transform.position;
        //     Vector2 v1 = new Vector2(hit.collider.gameObject.transform.forward.x, hit.collider.gameObject.transform.forward.z);
        //     Vector2 v2 = new Vector2(dirVector.x, dirVector.z);
        //
        //     Debug.DrawRay(hit.collider.gameObject.transform.position, dirVector  * 20, Color.blue);
        //     
        //     float xAngle = Mathf.Acos(Vector2.Dot(v1.normalized, v2.normalized)) * Mathf.Rad2Deg;
        //     
        //     v1 = new Vector2(hit.collider.gameObject.transform.forward.y, hit.collider.gameObject.transform.forward.z);
        //     v2 = new Vector2(dirVector.y, dirVector.z);
        //     float yAngle = Mathf.Acos(Vector2.Dot(v1.normalized, v2.normalized)) * Mathf.Rad2Deg;
        //
        //     var loc = hit.collider.gameObject.transform.rotation * hit.point;
        //     if (loc.y < hit.collider.gameObject.transform.position.y)
        //         yAngle *= -1.0f;
        //     
        //     if (loc.x > hit.collider.gameObject.transform.position.x)
        //         xAngle *= -1.0f;
        //
        //     //Debug.Log("X: " + xAngle + ", Y: " + yAngle);
        //
        //
        // }
        // else
        // {
        //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        //     Debug.Log("Did not Hit");
        // }
    }

    void PrintCity()
    {
         //float rLong = Random.Range(-180.0f, 180.0f);
         //float rLat = Random.Range(-90.0f, 90.0f);
         float rLong = 22.4f;
         float rLat = -5.9f;
        var city = earth.GetComponent<CityLocations>().GetClosestCity(rLat, rLong);
        Debug.Log(city.name + ", " + city.country);
        Debug.Log("Lat: " + rLat + ", Long: " + rLong);
    }
    
}
