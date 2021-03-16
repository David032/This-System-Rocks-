using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Analytics;
using System;
using Unity.Mathematics;

public class CityLocations : MonoBehaviour
{
    [SerializeField] private string filename = "worldcities.csv";
    [SerializeField] private uint minPopulation = 10000;

    private const float LAT_TO_KM = 110.574f;
    private const double R = 6371e3;

    [SerializeField]
    private GameObject explo;
    [SerializeField]
    private GameObject cityB;
    public struct CityData : IEquatable<CityData> , IComparable<CityData>
    {
        public string name;
        public string country;
        public float latitude;
        public float longitude;
        public uint population;
        
        public int CompareTo(CityData other)
        {
            int ret = longitude.CompareTo(other.longitude);
            return ret != 0 ? ret : latitude.CompareTo(other.longitude);
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
    
    float GetDistance2(float long1, float lat1, float long2, float lat2)
    {
        float radius = 500.0f;

        var phi_1 = math.radians(lat1);
        var phi_2 = math.radians(lat2);

        var delta_phi = math.radians(lat1 - lat2);
        var delta_lambda = math.radians(long1 - long2);

        var a = math.sin(delta_phi / 2.0) + math.cos(phi_1) * math.cos(phi_2) * math.sin(delta_lambda / 2.0);

        var c = 2 * math.atan2(math.sqrt(a), math.sqrt(1 - a));

        return radius * (float)c;
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
            // if (explo)
            // {
            //     var go = Instantiate(explo, transform);
            //     
            //     go.transform.localPosition = llarToWorld(city.latitude, city.longitude) * 500;
            //     go.transform.localScale *= 10.0f;
            //     var exploData = go.GetComponent<ExplosionData>();
            //     exploData.city = city.name;
            //     exploData.country = city.country;
            //     exploData.latitude = city.latitude;
            //     exploData.longitude = city.longitude;
            // }
        }

        cities.Sort();
        if (cities.Count % 2 != 0)
        {
            cities.RemoveAt(cities.Count - 1);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //FireMouseRay();
    }

    public CityData GetClosestCity(Vector3 point)
    {
        var vec2 = GetLongLat(point);
        var city = CheckList(cities, vec2.x, vec2.y);
        if (explo)
        {
            var go = Instantiate(explo, transform);
            
            go.transform.localPosition = llarToWorld(vec2.y, vec2.x) * 500;
            go.transform.localScale *= 10.0f;
            var exploData = go.GetComponent<ExplosionData>();
            exploData.city = city.name;
            exploData.country = city.country;
            exploData.latitude = vec2.y;
            exploData.longitude = vec2.x;
            
            var go2 = Instantiate(cityB, transform);
            go2.transform.localPosition = llarToWorld(city.latitude, city.longitude) * 500;
            go2.transform.localScale *= 10.0f;
            exploData = go2.GetComponent<ExplosionData>();
            exploData.city = city.name;
            exploData.country = city.country;
            exploData.latitude = city.latitude;
            exploData.longitude = city.longitude;
        }

        //CheckList(cities, vec2.x, vec2.y);
        return city;
    }
    
    public CityData GetClosestCity(float latitude, float longitude)
    {
        CityData currentCity = cities[0];
        float currentDistance = Mathf.Infinity;

        foreach (var city in cities)
        {
            float distance = Vector3.Distance(llarToWorld(latitude, longitude) * 500 , llarToWorld(city.latitude, city.longitude) * 500);
            if (distance <
                currentDistance)
            {
                currentDistance = distance;
                currentCity = city;
            }
            Debug.Log("City: " + currentCity.name + ", Country: " + currentCity.country + ", Distance: " + distance);
        }
        
        Debug.Log("City: " + currentCity.name + ", Country: " + currentCity.country + 
                  ", Population: " + currentCity.population + ", Long: " + currentCity.longitude + ", Lat: " + currentCity.latitude);

        return currentCity;
        //return CheckList(cities, longitude, latitude);
    }

    public CityData GetClosestCity2(float latitude, float longitude)
    {
        CityData currentCity = cities[0];
        float currentDistance = Mathf.Infinity;

        foreach (var city in cities)
        {
            float distance = GetDistance(longitude, latitude, city.longitude, city.latitude);
            Debug.Log("City: " + currentCity.name + ", Country: " + currentCity.country + ", Distance: " + distance);
            if (distance <
                currentDistance)
            {
                currentDistance = distance;
                currentCity = city;
            }
        }
        
        Debug.Log("City: " + currentCity.name + ", Country: " + currentCity.country + 
                  ", Population: " + currentCity.population + ", Long: " + currentCity.longitude + ", Lat: " + currentCity.latitude);

        return currentCity;
        //return CheckList(cities, longitude, latitude);
    }
    
    CityData CheckList(List<CityData> c, float longitude, float latitude)
    {
        if (c.Count % 2 != 0)
        {
            c.RemoveAt(c.Count - 1);
        }
        
        if (c.Count <= 1000)
        {
            CityData currentCity = c[0];
            float currentDistance = Mathf.Infinity;

            foreach (var city in c)
            {
                float distance = Vector3.Distance(llarToWorld(latitude, longitude) * 500 , llarToWorld(city.latitude, city.longitude) * 500);
                if (distance <
                    currentDistance)
                {
                    currentDistance = distance;
                    currentCity = city;
                }
            }

            return currentCity;
        }
        
        if (c[c.Count / 4].longitude > longitude)
        {
            List<CityData> c1 = new List<CityData>();
            c1.AddRange(c.GetRange(0, c.Count / 2));
            return CheckList(c1, longitude, latitude);
        }
        List<CityData> c2 = new List<CityData>();
        c2.AddRange(c.GetRange(c.Count / 4, Mathf.Clamp((c.Count / 4) * 3, 0, c.Count - (c.Count / 4) * 3)));
        return CheckList(c2, longitude, latitude);
    }

    public Vector2 GetLongLat(Vector3 point)
    {
        Vector3 dirVector = point - gameObject.transform.position;
        Vector3 dirVector2 = gameObject.transform.forward - gameObject.transform.position;
        Vector2 v1 = new Vector2(dirVector2.x, dirVector2.z);
        Vector2 v2 = new Vector2(dirVector.x, dirVector.z);
        v1.Normalize();
        v2.Normalize();
        //Debug.DrawRay(gameObject.transform.position, dirVector  * 20, Color.blue);
        //float xAngle = Mathf.Acos(Vector2.Dot(v1.normalized, v2.normalized)) * Mathf.Rad2Deg;
        float xAngle = Mathf.Acos((v1.x * v2.x + v1.y * v2.y) / Mathf.Sqrt(v1.x * v1.x + v1.y * v1.y) * Mathf.Sqrt(v2.x * v2.x + v2.y * v2.y)) * Mathf.Rad2Deg;
        
        dirVector2 = gameObject.transform.up - gameObject.transform.position;
        v1 = new Vector2(dirVector2.x, dirVector2.y);
        v2 = new Vector2(dirVector.x, dirVector.y);
        v1.Normalize();
        v2.Normalize();
        float yAngle = 90.0f - Mathf.Acos((v1.x * v2.x + v1.y * v2.y) / Mathf.Sqrt(v1.x * v1.x + v1.y * v1.y) * Mathf.Sqrt(v2.x * v2.x + v2.y * v2.y)) * Mathf.Rad2Deg;

        var loc = gameObject.transform.rotation * point;
        // if (loc.y < gameObject.transform.position.y)
        //     yAngle *= -1.0f;
            
        if (loc.x > gameObject.transform.position.x)
            xAngle *= -1.0f;
        
        Debug.Log("Longitude: " + xAngle + ", Latitude: " + yAngle);
        return new Vector2(xAngle, yAngle);
        // Vector3 point2 = transform.worldToLocalMatrix * point;
        // Vector2 vec2 = new Vector2(Mathf.Atan2(point2.y, point2.x), Mathf.Acos(point2.z));
        // Debug.Log(point2);
        // Debug.Log(vec2);
        // return vec2;
    }
    
    Vector2 vector3toLonLat(Vector3 point)
    {
        point.Normalize();

        //longitude = angle of the vector around the Y axis
        //-( ) : negate to flip the longitude (3d space specific )
        //- PI / 2 to face the Z axis
        var lng = -( Math.Atan2( -point.z, -point.x ) ) - Math.PI / 2;

        //to bind between -PI / PI
        if( lng < - Math.PI )lng += Math.PI * 2;

        //latitude : angle between the vector & the vector projected on the XZ plane on a unit sphere

        //project on the XZ plane
        var p = new Vector3( point.x, 0, point.z );
        //project on the unit sphere
        p.Normalize();

        //commpute the angle ( both vectors are normalized, no division by the sum of lengths )
        var lat = Math.Acos( Vector3.Dot( p, point ) );

        //invert if Y is negative to ensure teh latitude is comprised between -PI/2 & PI / 2
        if( point.y < 0 ) lat *= -1;
        
        Debug.Log("Longitude: " + lng + ", Latitude: " + lat);
        return new Vector2((float)lng,(float)lat );

    }

    Vector3 llarToWorld(float lat, float lon)
    {
        Vector3 dirVector = Vector3.zero;

        Vector3 v1 = Quaternion.Euler(-lat, -lon, -lat) * transform.forward;
        //v1 = Quaternion.AngleAxis(lon, transform.up) * v1;

        return v1;
    }


    void FireMouseRay()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            //Vector3 direction = Input.mousePosition - Camera.main.transform.position;
 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 1500.0f))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);
                GetClosestCity(hit.point);
                //Debug.Log("Did Hit");
            }
            //Debug.DrawRay(ray.origin, ray.direction);
        }
        

    }
    

}
