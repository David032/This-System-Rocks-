using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Analytics;
using Random = UnityEngine.Random;

public struct Data
{
    public string Name;
    public string CloseApproach;
    public string NominalDistance;
    public string RelativeVel;
    public string VelInfinite;
    public string Magnitude;
    public string EstimatedDiameter;

    public Data(string v1, string v2, string v3, string v4, string v5, string v6, string v7)
    {
        Name = v1;
        CloseApproach = v2;
        NominalDistance = v3;
        RelativeVel = v4;
        VelInfinite = v5;
        Magnitude = v6;
        EstimatedDiameter = v7;
    }
}

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject Asteroid;
    [SerializeField] private GameObject earth;
    public List<Data> PotentialRocks = new List<Data>();
    [SerializeField] private string filename;

    private List<GameObject> activeAsteroids = new List<GameObject>();

    public List<GameObject> ActiveAsteroids => activeAsteroids;
    
    private void Awake()
    {
        FetchData();
        InvokeRepeating("SpawnAsteroid", 0.1f, 5);
    }

    private void Start()
    {
     
    }

    void SpawnAsteroid() 
    {
        //Should be a better way to do this
        int randomSelection = Random.Range(0, PotentialRocks.Count - 1);
        Data selectedData = PotentialRocks[randomSelection];
//        print(selectedData.Name);

        GameObject newRock = Instantiate(Asteroid, GetSpawnPos(), new Quaternion());
        activeAsteroids.Add(newRock);
        var outlineData = newRock.GetComponent<OutlineDistanceFinder>();
        outlineData.endPoint = earth;
        
        AsteroidData rockData = newRock.GetComponent<AsteroidData>();
        float randX = Random.Range(0f, 1000f);
        float randY = Random.Range(0f, 1000f);
        float randZ = Random.Range(0f, 1000f);

        rockData.scientificName = selectedData.Name;
        rockData.distanceFromEarth = selectedData.NominalDistance;
        rockData.estimatedDiameter = selectedData.EstimatedDiameter;
        rockData.velocity = selectedData.RelativeVel;

        Vector3 forceDir = earth.transform.position - transform.position;
        forceDir = Quaternion.Euler(Random.Range(-45, 45), Random.Range(-45, 45), Random.Range(-45, 45)) * forceDir;
         newRock.GetComponent<Asteroid>().forceDir = forceDir;
        newRock.GetComponent<Rigidbody>().AddForce(forceDir * Random.Range(4.0f, 10.0f));
        newRock.AddComponent<Asteroid>();
        PotentialRocks.Remove(selectedData);
        
        Locator.Instance.GameEvents.asteroidSpawnMsg?.Invoke(activeAsteroids.Count);

    }

    void FetchData() 
    {
        if (filename.Length <= 0)
            return;

        string filepath = "Assets/Databases/" + filename;

        StreamReader streamreader = new StreamReader(filepath);

        bool endOfFile = false;
        int i = 0;
        while (!endOfFile)
        {
            string dataString = streamreader.ReadLine();
            if (dataString == null)
            {
                endOfFile = true;
                break;
            }

            var dataValues = dataString.Split(',');

            Data entry = new Data(dataValues[0],dataValues[1], dataValues[2],dataValues[3],dataValues[4],dataValues[5],dataValues[6]);

            i++;
            if (i <= 2)
                continue;

            entry.Name = dataValues[0];
            entry.CloseApproach = dataValues[1];
            entry.NominalDistance = dataValues[2];
            entry.RelativeVel = dataValues[3];
            entry.VelInfinite = dataValues[4];
            entry.Magnitude = dataValues[5];
            entry.EstimatedDiameter = dataValues[6];
            PotentialRocks.Add(entry);
        }
    }

    Vector3 GetSpawnPos()
    {
        float radius = Random.Range(110.0f, 130.0f);
        float min = 0.0f;
        float angle = Random.Range(0, 90.0f);
        Debug.Log(angle);
        angle += 135.0f;
        float x = radius * Mathf.Sin(angle* Mathf.Deg2Rad);
        float z = radius * Mathf.Cos(angle* Mathf.Deg2Rad);
        return new Vector3(x, 0.0f, z);
    }
}
