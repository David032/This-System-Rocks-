using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locator : MonoBehaviour
{
    // makes this script a static instance
    private static Locator instance;

    public static Locator Instance
    {
        get => instance;
    }

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    private MissileManager missileManager;

    public MissileManager MissileManager
    {
        get => missileManager;
    }
    
    [SerializeField]
    private AsteroidSpawner asteroidSpawner;

    public AsteroidSpawner AsteroidSpawner
    {
        get => asteroidSpawner;
    }

    [SerializeField] private GameEvents gameEvents;

    public GameEvents GameEvents
    {
        get => gameEvents;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
