using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Call to spawn in UI interfaces.
    public static UIManager instance;
    
     private void Awake()
     {
         if (instance == null) instance = this;
     }

     public void CreateScreen()
     {
     }

     public void CreateLayer()
     {
         
     }

     public void CreateDialog()
     {
         
     }
    
    void Update()
    {
        
    }
}
