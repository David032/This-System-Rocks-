using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileManager : MonoBehaviour
{
    private List<MissileSpawner> missileSpawners = new List<MissileSpawner>();

    public List<MissileSpawner> MissileSpawners => missileSpawners;

    void Awake()
    {
        missileSpawners.AddRange(GetComponentsInChildren<MissileSpawner>());
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireMissle(GameObject asteroid)
    {
        MissileSpawner spawner = missileSpawners[0];
        float distance = Mathf.Infinity;
        for (int i = 0; i < missileSpawners.Count; i++)
        {
            var dis = Vector3.Distance(missileSpawners[i].gameObject.transform.position, asteroid.transform.position);
            if (dis < distance)
            {
                distance = dis;
                spawner = missileSpawners[i];
                Debug.Log("Spawner: " + i);
            }
        }
        
        spawner.SpawnMissile(asteroid);
    }
}
