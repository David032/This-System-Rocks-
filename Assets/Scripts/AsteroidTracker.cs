using System.Collections;
using System.Collections.Generic;
using deVoid.UIFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AsteroidTracker : APanelController
{
    public TextMeshProUGUI asteroidsRemaining;
    protected override void Awake()
    {
        asteroidsRemaining = new TextMeshProUGUI();
    }

    private void Start()
    {
        asteroidsRemaining = GetComponent<TextMeshProUGUI>();
        asteroidsRemaining.SetText("Asteroids: 0");
        Locator.Instance.GameEvents.asteroidSpawnMsg += GetCountFromThingy;
    }

    public void GetCountFromThingy(int newNum)
    {
        
        asteroidsRemaining.SetText( "Asteroids: " +newNum.ToString());
    }

    protected override void HierarchyFixOnShow()
    {
        print("Ayyyyyyyy");
    }
}
