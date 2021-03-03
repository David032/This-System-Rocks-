using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject Asteroid;

    private void Awake()
    {
        InvokeRepeating("SpawnAsteroid", 1, 15);
    }

    void SpawnAsteroid() 
    {
        GameObject newRock = Instantiate(Asteroid, transform.position, new Quaternion());
        float randX = Random.Range(0f, 1000f);
        float randY = Random.Range(0f, 1000f);
        float randZ = Random.Range(0f, 1000f);
        newRock.GetComponent<Rigidbody>().AddForce(randX,randY,randZ);
    }
}
