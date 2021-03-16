using System;
using System.Collections;
using System.Collections.Generic;
using deVoid.UIFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AstDataPanel : AWindowController<AstDataProperty>
{
    private TextMeshProUGUI displayText;
    private void Awake()
    {
        displayText = GetComponent<TextMeshProUGUI>();
    }

    protected override void OnPropertiesSet()
    {
        displayText.SetText(
            "Name: " + Properties._data.Name.text
                     +"\n"+"Diameter: " +Properties._data.Diameter.text
                     +"\n"+"Velocity: " +Properties._data.RelVel.text
                     +"\n"+"Distance from Earth: " +Properties._data.CloseApproach.text
        );
    }
    
}
