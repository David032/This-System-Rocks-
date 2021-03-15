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
    private MissileSpawner missileSpawner;

    public MissileSpawner MissileSpawner
    {
        get => missileSpawner;
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
