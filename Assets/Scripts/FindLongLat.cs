using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.UIElements;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class FindLongLat : MonoBehaviour
{
    private GameObject earth;
    private float countDown = 2.0f;
    
    [SerializeField]
    private float sensitivity;
    [SerializeField]
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        earth = GameObject.FindWithTag("Player");
        //PrintCity();

    }
    
    float rLong = 0.0f;
    float rLat = 0.0f;

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;
        if (countDown <= 0.0f)
        {
            PrintCity();
            countDown = 2.0f;
        }


        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
           // Debug.Log("Did Hit");
        
        
            Vector3 dirVector = hit.point - hit.collider.gameObject.transform.position;
            Vector2 v1 = new Vector2(hit.collider.gameObject.transform.forward.x, hit.collider.gameObject.transform.forward.z);
            Vector2 v2 = new Vector2(dirVector.x, dirVector.z);
        
            //Debug.DrawRay(hit.collider.gameObject.transform.position, dirVector  * 20, Color.blue);
            
            float xAngle = Mathf.Acos(Vector2.Dot(v1.normalized, v2.normalized)) * Mathf.Rad2Deg;
            
            v1 = new Vector2(hit.collider.gameObject.transform.forward.y, hit.collider.gameObject.transform.forward.z);
            v2 = new Vector2(dirVector.y, dirVector.z);
            float yAngle = Mathf.Acos(Vector2.Dot(v1.normalized, v2.normalized)) * Mathf.Rad2Deg;
        
            var loc = hit.collider.gameObject.transform.rotation * hit.point;
            if (loc.y < hit.collider.gameObject.transform.position.y)
                yAngle *= -1.0f;
            
            if (loc.x > hit.collider.gameObject.transform.position.x)
                xAngle *= -1.0f;
        
            //Debug.Log("X: " + xAngle + ", Y: " + yAngle);
            rLat = yAngle;
            rLong = xAngle;

        }
        // else
        // {
        //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        //     Debug.Log("Did not Hit");
        // }
    }

    void FixedUpdate ()
    {
        float rotateHorizontal = Input.GetAxis ("Mouse X");
        float rotateVertical = Input.GetAxis ("Mouse Y");
        transform.Rotate(transform.up, rotateHorizontal * sensitivity); //use transform.Rotate(-transform.up * rotateHorizontal * sensitivity) instead if you dont want the camera to rotate around the player
       // transform.Rotate(transform.right, rotateVertical * sensitivity); // again, use transform.Rotate(transform.right * rotateVertical * sensitivity) if you don't want the camera to rotate around the player

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * speed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * speed;
        }

    }

    void PrintCity()
    {
        // float rLong = Random.Range(-180.0f, 180.0f);
        // float rLat = Random.Range(-90.0f, 90.0f);
        var city = earth.GetComponent<CityLocations>().GetClosestCity(rLat, rLong);
        Debug.Log(city.name + ", " + city.country);
        Debug.Log("Lat: " + rLat + ", Long: " + rLong);
    }
    
}
