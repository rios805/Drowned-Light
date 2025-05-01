using UnityEngine;
using System.Collections.Generic;

public class StatueSpawns : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints; 
    [SerializeField] private GameObject[] statuePrefabs;
    
    void Awake()
    {
        if(spawnPoints == null || spawnPoints.Length == 0)
        {
            var children = new List<Transform>();
            foreach (Transform t in transform)
            {
                children.Add(t);
            }
            spawnPoints = children.ToArray();
        }
    }

    void Start()
    {
        SpawnStatues(); 
    }

    public void SpawnStatues()
    {
        int statueCount = statuePrefabs.Length;
        int pointCount  = spawnPoints.Length;

        if (statueCount == 0)
        {
            Debug.LogWarning("No statuePrefabs assigned!");
            return;
        }
        if (pointCount < statueCount)
        {
            Debug.LogError($"Need at least {statueCount} spawn points, but only have {pointCount}.");
            return;
        }

        var indices = new List<int>(pointCount);
        for (int i = 0; i < pointCount; i++) indices.Add(i);
        for (int i = 0; i < pointCount; i++)
        {
            int r = Random.Range(i, pointCount);
            int tmp = indices[i];
            indices[i] = indices[r];
            indices[r] = tmp;
        }

        // Spawn each statuePrefab at a distinct point, assign ID = i+1
        for (int i = 0; i < statueCount; i++)
        {
            Transform spawn = spawnPoints[ indices[i] ];
            GameObject stat = Instantiate(statuePrefabs[i],
                                          spawn.position,
                                          spawn.rotation);

            // assign the StatueID
            var pick = stat.GetComponent<Pickupable>();
            if (pick != null)
            {
                pick.statueID = (i + 1).ToString();
            }
            else
            {
                Debug.LogWarning($"{stat.name} has no Pickupable – can't assign statueID.");
            }
        }
    }
}
