using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [HideInInspector]
    public Vector3 forceDir;
    Vector3 startPos;

    private bool targeted = false;
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(startPos, forceDir * 30.0f, Color.yellow);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Earth")
        {
            var cityLoc = other.gameObject.GetComponent<CityLocations>();
            //var vec2 = cityLoc.GetLongLat(other.GetContact(0).point);
            var city = cityLoc.GetClosestCity(other.GetContact(0).point);
            //Debug.Log("City: " + city.name + ", Country: " + city.country + ", Population: " + city.population);
            
            var activeasteroids = Locator.Instance.AsteroidSpawner.ActiveAsteroids;
            if(activeasteroids.Contains(gameObject))
                activeasteroids.Remove(gameObject);
            
            Locator.Instance.GameEvents.asteroidDestroyedMsg?.Invoke(GetComponent<AsteroidData>());
            Destroy(gameObject);
        }
    }
    
    void OnMouseDown()
    {
        if(!targeted)
            Locator.Instance.MissileManager.FireMissle(gameObject);

        targeted = true;
        Debug.Log("Targeted");
    }
    
}
