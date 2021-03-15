using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidData : MonoBehaviour //For storing data of real objects that came close/hit
{
    public string scientificName;
    public string velocity;
    public string distanceFromEarth;
    public string estimatedDiameter;

    public void OnMouseDown()
    {
        DataViewer viewer = GameObject.FindGameObjectWithTag("AsteroidController").GetComponent<DataViewer>();
        viewer.Name.text = scientificName;
        viewer.RelVel.text = velocity + "Km/s";
        viewer.CloseApproach.text = distanceFromEarth + " LD";
        viewer.Diameter.text = estimatedDiameter;
        //FX here?
        Destroy(this.gameObject);
    }
}
