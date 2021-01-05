using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSheeps : MonoBehaviour
{

    [SerializeField] List<GameObject> spawnPoints;
    [SerializeField] List<GameObject> sheeps;

    public void StartSpawnSheeps(int sheepAmount)
    {
        GameObject spawnPoint;
        GameObject sheep;
        for (var i = 0; i < sheepAmount; i++)
        {
            spawnPoint = GetAvailableSpawnPoint();
            sheep = GetAvailableSheep();

            if (sheep == null || spawnPoint == null)
                return;

            sheep.transform.position = new Vector3(spawnPoint.transform.position.x, sheep.transform.position.y, spawnPoint.transform.position.z);

            spawnPoint.SetActive(false);
            sheep.SetActive(true);
        }
    }

    GameObject GetAvailableSpawnPoint()
    {
        GameObject spawnPoint;

        for(int i = 0; i < 50; i++)
        {
            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            if (spawnPoint.activeInHierarchy)
                return spawnPoint;
        }

        return null;
    }

    GameObject GetAvailableSheep()
    {
        GameObject sheep;
        
        for (int i = 0; i < 50; i++)
        {
            sheep = sheeps[Random.Range(0, sheeps.Count)];
            if (!sheep.activeInHierarchy)
                return sheep;
        }

        return null;
    }

    public void RestartSheeps()
    {
        foreach (GameObject sheep in sheeps)
        {
            if (sheep.activeInHierarchy)
                sheep.GetComponent<Sheep>().RestartSheep();
        }

        foreach (GameObject spawnPoint in spawnPoints)
        {
            if (!spawnPoint.activeInHierarchy)
                spawnPoint.SetActive(true);
        }
    }

}
