using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    private Spawner[] spawners;
    private bool spawning = false;

    public void StartSpawn()
    {
        if (!spawning)
        {
            spawning = true;
            spawners = FindObjectsOfType<Spawner>();

            foreach (Spawner spawner in spawners)
            {
                spawner.StartSpawning();
            }
        }
    }
}
