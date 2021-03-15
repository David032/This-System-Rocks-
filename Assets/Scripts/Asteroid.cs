using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Earth")
        {
            var cityLoc = other.gameObject.GetComponent<CityLocations>();
            //var vec2 = cityLoc.GetLongLat(other.GetContact(0).point);
            var city = cityLoc.GetClosestCity(other.GetContact(0).point);
            Debug.Log("City: " + city.name + ", Country: " + city.country + ", Population: " + city.population);
            Destroy(gameObject);
        }
    }
}
