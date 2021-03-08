using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Analytics;

public struct Data
{
    public string Name;
    public string CloseApproach;
    public string NominalDistance;
    public string RelativeVel;
    public string VelInfinite;
    public string Magnitude;
    public string EstimatedDiameter;

    public Data(string v1, string v2, string v3, string v4, string v5, string v6, string v7) : this()
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
    public List<Data> PotentialRocks = new List<Data>();
    [SerializeField] private string filename;

    private void Awake()
    {
        PotentialRocks.Add(new Data());
        FetchData();
        InvokeRepeating("SpawnAsteroid", 1, 5);
    }

    void SpawnAsteroid() 
    {
        //Should be a better way to do this
        int randomSelection = Random.Range(0, PotentialRocks.Count);
        Data selectedData = PotentialRocks[randomSelection];

        GameObject newRock = Instantiate(Asteroid, transform.position, new Quaternion());
        newRock.AddComponent<AsteroidData>();
        AsteroidData rockData = newRock.GetComponent<AsteroidData>();
        float randX = Random.Range(0f, 1000f);
        float randY = Random.Range(0f, 1000f);
        float randZ = Random.Range(0f, 1000f);
        //print(randX + " " + randY + " " + randZ);

        rockData.scientificName = selectedData.Name;
        rockData.distanceFromEarth = selectedData.NominalDistance;
        rockData.estimatedDiameter = selectedData.EstimatedDiameter;
        rockData.velocity = selectedData.RelativeVel;

        newRock.GetComponent<Rigidbody>().AddForce(randX,randY,randZ);
        newRock.AddComponent<Asteroid>();
        PotentialRocks.Remove(selectedData);
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
            print(dataValues[0] + " " + dataValues[1] + " " + dataValues[2] + " " + dataValues[3] 
                + " " + dataValues[4] + " " + dataValues[5] + " " + dataValues[6]);
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
}
