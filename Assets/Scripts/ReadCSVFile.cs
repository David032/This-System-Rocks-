using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadCSVFile : MonoBehaviour
{
    
    [SerializeField] private string filename;

    // Start is called before the first frame update
    void Start()
    {
        if (filename.Length <= 0)
            return;
        
        string filepath = "Assets/Databases/" + filename;
        
        StreamReader streamreader = new StreamReader(filepath);
        
        bool endOfFile = false;
        while (!endOfFile)
        {
            string dataString = streamreader.ReadLine();
            if (dataString == null)
            {
                endOfFile = true;
                break;
            }
        
            var dataValues = dataString.Split(',');
            Debug.Log(dataValues[0]);
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
