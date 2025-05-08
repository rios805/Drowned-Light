using UnityEngine;
using System.Collections.Generic;

public class RandomSpawns : MonoBehaviour
{
    [SerializeField] private Transform[] statueSpawnPoints;
    [SerializeField] private GameObject[] statuePrefabs;

    [SerializeField] private Transform[] coinSpawnPoints;
    [SerializeField] private GameObject[] coinPrefabs;

    void Awake()
    {
        if (statueSpawnPoints == null || statueSpawnPoints.Length == 0)
        {
            statueSpawnPoints = GetComponentsInChildren<Transform>(tag:null);
        }

        if (coinSpawnPoints == null || coinSpawnPoints.Length == 0)
        {
            coinSpawnPoints = GetComponentsInChildren<Transform>(tag:null);
        }
    }

    void Start()
    {
        SpawnStatues();
        SpawnCoins();
    }

    private void SpawnStatues()
    {
        int statues = statuePrefabs.Length;
        if (statueSpawnPoints.Length < statues)
        {
            Debug.LogError($"Not enough statue spawn points: need {statues}, have {statueSpawnPoints.Length}");
            return;
        }

        // shuffle indices
        var indices = CreateShuffledIndices(statueSpawnPoints.Length);

        for (int i = 0; i < statues; i++)
        {
            var pt = statueSpawnPoints[indices[i]];
            var inst = Instantiate(statuePrefabs[i], pt.position, pt.rotation);

            var pick = inst.GetComponent<Pickupable>();
            if (pick != null)
            {
                pick.statueID = (i + 1).ToString();
            }
            else
            {
                Debug.LogWarning($"Spawned statue '{inst.name}' has no Pickupable component");
            }
        }
    }

    private void SpawnCoins()
    {
        int coins = coinPrefabs.Length;
        if (coinSpawnPoints.Length < coins)
        {
            Debug.LogError($"Not enough coin spawn points: need {coins}, have {coinSpawnPoints.Length}");
            return;
        }

        var indices = CreateShuffledIndices(coinSpawnPoints.Length);

        for (int i = 0; i < coins; i++)
        {
            var pt = coinSpawnPoints[indices[i]];
            var inst = Instantiate(coinPrefabs[i], pt.position, pt.rotation);
        }
    }

    private List<int> CreateShuffledIndices(int count)
    {
        var list = new List<int>(count);
        for(int i = 0; i < count; i++)
        {
            list.Add(i);
        }

        for (int i = 0; i < count - 1; i++)
        {
            int r = Random.Range(i, count);
            int tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }

        return list;
    }

    private Transform[] GetComponentsInChildren<TransformExceptSelf>(string tag)
    {
        var all = GetComponentsInChildren<Transform>();
        var filtered = new List<Transform>();
        for (int i = 1; i < all.Length; i++)
        {
            if (string.IsNullOrEmpty(tag) || all[i].CompareTag(tag))
            {
                filtered.Add(all[i]);
            }
        }
        return filtered.ToArray();
    }
}
