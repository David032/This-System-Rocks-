using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLayerPanel : UIPanelLayer
{
    private bool setted;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Jump") != 0)
        {
            setted = true;
        }

        if (setted)
        {
            if (UIManager.instance.IsPanelOpen("test"))
            {
                print("YES");
            }
        }
    }
    private void Awake()
    {
        Initialise();
    }
    
}
