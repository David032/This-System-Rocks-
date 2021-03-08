using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;
using Vector3 = UnityEngine.Vector3;

public class OutlineDistanceFinder : MonoBehaviour
{
    private Outline ol;
    private float midPoint;
    public GameObject endPoint;
    [SerializeField] private Color colourAtStart;
    [SerializeField] private Color colourAtEnd;
    
    public GameObject maxDistMarker;
    [SerializeField] private float minDist = 10;
    private float currentDist;
    private float distanceToMidPoint;
    [SerializeField] private float maxDist = 175;
    
    public void Start()
    {
        ol = GetComponent<Outline>();
    }

    void Update()
    {
        var endPos = endPoint.transform.position;
        currentDist = Vector3.Distance(transform.position, endPos);
        if (currentDist <= maxDist)
        {
            currentDist = (currentDist - minDist) / (maxDist - minDist);
            var newColour = Color.Lerp(colourAtEnd, colourAtStart, currentDist);
            ol.OutlineColor = newColour;
        }
        else
        {
            ol.OutlineColor = Color.blue;
        }

        // print(currentDist);
    }
}
