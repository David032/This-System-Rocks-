using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct Data
{
    public string Name;
    public string CloseApproach;
    public string NominalDistance;
    public string MinimumDistance;
    public string RelativeVel;
    public string VelInfinite;
    public string Magnitude;
    public string EstimatedDiameter;
}

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject Asteroid;
    public List<Data> PotentialRocks = new List<Data>();
    [SerializeField] private string filename;

    private void Awake()
    {
        FetchData();
        InvokeRepeating("SpawnAsteroid", 1, 5);
    }

    void SpawnAsteroid() 
    {
        //Should be a better way to do this
        int randomSelection = Random.Range(0, PotentialRocks.Count);
        Data selectedData = PotentialRocks[randomSelection];

        GameObject newRock = Instantiate(Asteroid, transform.position, new Quaternion());
        AsteroidData rockData = GetComponent<AsteroidData>();
        float randX = Random.Range(0f, 1000f);
        float randY = Random.Range(0f, 1000f);
        float randZ = Random.Range(0f, 1000f);

        rockData.scientificName = selectedData.Name;
        rockData.distanceFromEarth = selectedData.NominalDistance;
        rockData.estimatedDiameter = selectedData.EstimatedDiameter;
        rockData.velocity = selectedData.RelativeVel;

        newRock.GetComponent<Rigidbody>().AddForce(randX,randY,randZ);
        PotentialRocks.Remove(selectedData);
    }

    void FetchData() 
    {
        if (filename.Length <= 0)
            return;

        string filepath = "Assets/Databases/" + filename;

        StreamReader streamreader = new StreamReader(filepath);

        bool endOfFile = false;
        while (!endOfFile)
        {
            Data entry = new Data();
            string dataString = streamreader.ReadLine();
            if (dataString == null)
            {
                endOfFile = true;
                break;
            }

            var dataValues = dataString.Split(',');
            entry.Name = dataValues[0];
            entry.CloseApproach = dataValues[1];
            entry.NominalDistance = dataValues[2];
            entry.MinimumDistance = dataValues[3];
            entry.RelativeVel = dataValues[4];
            entry.VelInfinite = dataValues[5];
            entry.Magnitude = dataValues[6];
            entry.EstimatedDiameter = dataValues[7];
        }
    }
}
