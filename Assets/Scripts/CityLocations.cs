using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Analytics;
using System;

public class CityLocations : MonoBehaviour
{
    [SerializeField] private string filename = "worldcities.csv";
    [SerializeField] private uint minPopulation = 10000;

    private const float LAT_TO_KM = 110.574f;
    private const double R = 6371e3;
    
    
    public struct CityData : IEquatable<CityData> , IComparable<CityData>
    {
        public string name;
        public string country;
        public float latitude;
        public float longitude;
        public uint population;
        
        public int CompareTo(CityData pop1)
        {
            return longitude.CompareTo(pop1.longitude);
        }

        public bool Equals(CityData other)
        {
            return name == other.name && country == other.country && latitude.Equals(other.latitude) && longitude.Equals(other.longitude) && population == other.population;
        }

        public override bool Equals(object obj)
        {
            return obj is CityData other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (name != null ? name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (country != null ? country.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ latitude.GetHashCode();
                hashCode = (hashCode * 397) ^ longitude.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) population;
                return hashCode;
            }
        }
    }

    private List<CityData> cities = new List<CityData>();

    private void Awake()
    {
        if (filename.Length <= 0)
            return;

        GetCityData();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    float GetDistance(float long1, float lat1, float long2, float lat2)
    {
        float lat1Rad = lat1 * Mathf.PI / 180.0f;
        float lat2Rad = lat2 * Mathf.PI / 180.0f;
        float changeLatRad = (lat2 - lat1) * Mathf.PI / 180.0f;
        float changeLongRad = (long2 - long1) * Mathf.PI / 180.0f;

        float a = Mathf.Sin(changeLatRad / 2.0f) * (float)Math.Sin(changeLatRad / 2.0f) +
                  Mathf.Cos(lat1Rad) * Mathf.Cos(lat2Rad) *
                  Mathf.Sin(changeLongRad / 2.0f) * Mathf.Sin(changeLongRad / 2.0f);

        float c = 2.0f * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        
        return (float)R * c;
    }
    
    
    private void GetCityData()
    {
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
            i++;
            //Debug.Log(i);
            if (i == 1)
            {
                continue;
            }


            uint pop;
            if (!uint.TryParse(dataValues[9], out pop) || pop < minPopulation)
            {
                continue;
            }
            CityData city = new CityData();            
            city.name = dataValues[0];
            city.country = dataValues[4];
            if (!float.TryParse(dataValues[2], out city.latitude))
            {
                
            }
            if (!float.TryParse(dataValues[3], out city.longitude)) 
            {
                
            }

            city.population = pop;
            cities.Add(city);
            // if (cities.Count == 0)
            // {
            //     cities.Add(city);
            // }
            // else
            // {
            //
            //     int index = 0;
            //     if
            //     cities.Insert();    
            // }

            // if (!uint.TryParse(dataValues[9], out city.Population))
            // {
            //     city.Population = 0;
            // }
            // city.Name = dataValues[0].Substring(1, dataValues[0].Length - 2);
            // city.Country = dataValues[4].Substring(1, dataValues[4].Length - 2);
            // if (float.TryParse(dataValues[2].Substring(1, dataValues[2].Length - 2), out city.Latitude))
            // {
            //     int y = 0;
            // }
            // if (float.TryParse(dataValues[2].Substring(1, dataValues[2].Length - 2), out city.Longitude)) 
            // {
            //     int y = 0;
            // }
            // if (uint.TryParse(dataValues[9].Substring(1, dataValues[9].Length - 2), out city.Population)) 
            // {
            //     int y = 0;
            // }
            //city.Latitude = float.Parse(dataValues[2].Substring(1, dataValues[2].Length - 2));
            //city.Longitude = float.Parse(dataValues[3].Substring(1, dataValues[3].Length - 2));
            //city.Population = uint.Parse(dataValues[9].Substring(1, dataValues[9].Length - 2));
            //cities.Add(city);
        }

        cities.Sort();
        if (cities.Count % 2 != 0)
        {
            cities.RemoveAt(cities.Count - 1);
        }
        
        // cities.Sort(delegate(CityData x, CityData y) {
        //     if (x.Population >= y.Population) return 0;
        //     else return 1;
        //});
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CityData GetClosestCity(Vector3 point)
    {
        var vec2 = GetLongLat(point);
        return GetClosestCity(vec2.y, vec2.x);
        
        // List<CityData> c1 = new List<CityData>();
        // List<CityData> c2 = new List<CityData>();
        // c1.AddRange(cities.GetRange(0, cities.Count / 2));
        // c2.AddRange(cities.GetRange(cities.Count / 2, cities.Count / 2));
        
        
    }
    
    public CityData GetClosestCity(float latitude, float longitude)
    {

        return CheckList(cities, longitude, latitude);
        
        // List<CityData> c1 = new List<CityData>();
        // List<CityData> c2 = new List<CityData>();
        // c1.AddRange(cities.GetRange(0, cities.Count / 2));
        // c2.AddRange(cities.GetRange(cities.Count / 2, cities.Count / 2));
        
        
    }

    CityData CheckList(List<CityData> c, float longitude, float latitude)
    {
        if (c.Count <= 100)
        {
            CityData currentCity = c[0];
            float currentDistance = 10000000.0f;

            foreach (var city in c)
            {
                float distance = GetDistance(longitude, latitude, city.longitude, city.latitude);
                if (distance <
                    currentDistance)
                {
                    currentDistance = distance;
                    currentCity = city;
                }
            }

            return currentCity;
        }
        
        if (c[c.Count / 2].longitude > longitude)
        {
            List<CityData> c1 = new List<CityData>();
            c1.AddRange(c.GetRange(0, c.Count / 2));
            return CheckList(c1, longitude, latitude);
        }
        List<CityData> c2 = new List<CityData>();
        c2.AddRange(c.GetRange(c.Count / 2, Mathf.Clamp(c.Count / 2, 0, c.Count - c.Count / 2)));
        return CheckList(c2, longitude, latitude);
    }

    public Vector2 GetLongLat(Vector3 point)
    {
        Vector3 dirVector = point - gameObject.transform.position;
        Vector2 v1 = new Vector2(gameObject.transform.forward.x, gameObject.transform.forward.z);
        Vector2 v2 = new Vector2(dirVector.x, dirVector.z);
        
        //Debug.DrawRay(gameObject.transform.position, dirVector  * 20, Color.blue);
            
        float xAngle = Mathf.Acos(Vector2.Dot(v1.normalized, v2.normalized)) * Mathf.Rad2Deg;
            
        v1 = new Vector2(gameObject.transform.forward.y, gameObject.transform.forward.z);
        v2 = new Vector2(dirVector.y, dirVector.z);
        float yAngle = Mathf.Acos(Vector2.Dot(v1.normalized, v2.normalized)) * Mathf.Rad2Deg;
        
        var loc = gameObject.transform.rotation * point;
        if (loc.y < gameObject.transform.position.y)
            yAngle *= -1.0f;
            
        if (loc.x > gameObject.transform.position.x)
            xAngle *= -1.0f;
        
        return new Vector2(xAngle, yAngle);
    }
    

}
