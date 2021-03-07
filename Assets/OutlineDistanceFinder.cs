using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;
using Vector3 = UnityEngine.Vector3;

public class OutlineDistanceFinder : MonoBehaviour
{
    private Outline ol;
    [SerializeField] private GameObject originPoint;
    private float midPoint;
    [SerializeField] private GameObject endPoint;
    [SerializeField] private Color colourAtStart;
    [SerializeField] private Color colourAtMidpoint;
    [SerializeField] private Color colourAtEnd;
    private float minDist = 0;
    private float currentDist;
    private float distanceToMidPoint;
    private float maxDist = 15;
    
    void Start()
    {
        ol = GetComponent<Outline>();
    }

    void Update()
    {
        var endPos = endPoint.transform.position;
        currentDist = Vector3.Distance(transform.position, endPos);
        currentDist = (currentDist - minDist) / (maxDist - minDist);
        var newColour = Color.Lerp(colourAtEnd, colourAtStart, currentDist);
        ol.OutlineColor = newColour;
  
        print(currentDist);
    }
}
