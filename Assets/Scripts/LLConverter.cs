using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LLConverter : MonoBehaviour
{
    [SerializeField]
    private GameObject asteroid;

    private int amount = 50;

    [SerializeField] [Range(-180.0f, 180.0f)] private float angleX = 0.0f;
    [SerializeField] [Range(-180.0f, 180.0f)] private float angleY = 0.0f;
    private Vector3 direction = new Vector3(0, 0, 1);
    private Quaternion quat = new Quaternion();
    [SerializeField][Range(0.0f, 1.0f)]
    private float time = 0.0f;
    
    private float prevAngleX = 360.0f;
    private float prevAngleY = 360.0f;
        
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 dir = transform.forward;
            Quaternion quaty = Quaternion.AngleAxis(Random.Range(-180, 180), transform.up);
            quaty *= Quaternion.AngleAxis(Random.Range(-180, 180), transform.right);

            dir = quaty * dir;
            dir.Normalize();
            float distance = Random.Range(350, 1000);
            var go = Instantiate(asteroid, transform.position + dir * distance, Quaternion.identity);
            float size = GetSize(distance);
            go.transform.localScale = new Vector3(size, size, size);
            go.name = "ass" + i;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (prevAngleX != angleX || prevAngleY != angleY)
        {
            direction = transform.forward;
            quat = Quaternion.AngleAxis(angleX, transform.up);
            quat *= Quaternion.AngleAxis(angleY, transform.right);

            direction = quat * direction;
            direction.Normalize();
            prevAngleX = angleX;
            prevAngleY = angleY;
        }
        
        transform.position = Vector3.Lerp(Vector3.zero, Vector3.zero + direction * 100, time);
    }

    float GetSize(float distance)
    {
        float minDist = 350.0f, maxdist = 1000.0f;

        float normalised = (distance - minDist) / (maxdist - minDist);
        float minMaxSize = 5, maxMaxSize = 30;
        
        return Random.Range(0.5f, Mathf.Lerp(minMaxSize, maxMaxSize, normalised));
    }
    
}
