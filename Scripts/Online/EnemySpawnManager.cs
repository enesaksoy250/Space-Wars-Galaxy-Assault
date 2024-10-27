using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    public List<Transform> enemySpawnPoints1;
    public List<Transform> enemySpawnPoints2;

    public static EnemySpawnManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}
