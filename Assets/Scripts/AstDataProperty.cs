using System;
using System.Collections;
using System.Collections.Generic;
using deVoid.UIFramework;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class AstDataProperty : WindowProperties
{
    public DataViewer _data;

    public AstDataProperty(DataViewer data)
    {
        _data = data;
    }
}
