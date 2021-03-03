using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LLConverter : MonoBehaviour
{
    [SerializeField]
    private GameObject asteroid;

    private int amount = 100;

    [SerializeField] [Range(0.0f, 360.0f)] private float angleX = 0.0f;
    [SerializeField] [Range(0.0f, 360.0f)] private float angleY = 0.0f;
    private Vector3 direction = new Vector3(0, 0, 1);
    private Quaternion quat = new Quaternion();
    [SerializeField][Range(0.0f, 1.0f)]
    private float time = 0.0f;
    
    private float prevAngleX = -1.0f;
    private float prevAngleY = -1.0f;
        
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 dir = transform.forward;
            Quaternion quaty = Quaternion.AngleAxis(Random.Range(0, 360), transform.up);
            quaty *= Quaternion.AngleAxis(Random.Range(0, 360), transform.right);

            dir = quaty * dir;
            dir.Normalize();
            Instantiate(asteroid, transform.position + dir * Random.Range(350, 600), Quaternion.identity);
            asteroid.transform.localScale = new Vector3(4, 4, 4);
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
        
        transform.position = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 0, 0) + direction * 100, time);
    }
}
