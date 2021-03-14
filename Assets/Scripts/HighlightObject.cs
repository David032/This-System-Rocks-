using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObject : MonoBehaviour
{
    private Outline outline;

    private Vector2 mouse;

    private float distance = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Outline>() == null)
        {
            gameObject.AddComponent<Outline>();
        }
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseOver()
    {
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        outline.enabled = false;
    }

    private void OnMouseDown()
    {
        Camera.main.transform.position = transform.position + transform.right * (distance * transform.lossyScale.x);
        Camera.main.transform.LookAt(transform);
    }
}
