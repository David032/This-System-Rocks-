using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public delegate void GameStartMsg();

    public GameStartMsg gameStartMsg;

    public delegate void GameEndMsg();

    public GameEndMsg gameEndMsg;
    
    public delegate void GamePauseMsg();

    public GamePauseMsg gamePauseMsg;

    public delegate void AsteroidSpawnMsg(int n);

    public AsteroidSpawnMsg asteroidSpawnMsg;

    public delegate void AsteroidDestroyedMsg(AsteroidData data);

    public AsteroidDestroyedMsg asteroidDestroyedMsg;

    public delegate void AsteroidCollisionMsg();

    public AsteroidCollisionMsg asteroidCollisionMsg;

    public delegate void AsteroidClickMsg(DataViewer data);

    public AsteroidClickMsg asteroidClickMsg;

    public delegate void MissileLaunchedMsg();

    public MissileLaunchedMsg missileLaunchedMsg;

    private void Start()
    {
        gameStartMsg?.Invoke();
    }
}


