using UnityEngine;

/// <summary>
/// Early test, Map Magic free version is mid. Maybe look at unity terrain tools instead.
/// Known bug: rocks only spawn on the first grid/chunk.
/// </summary>

public class RockSpawner : MonoBehaviour
{
    public GameObject[] rockPrefabs;
    public int spawnCount;

    [Header("Scale")]
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.5f;
    [SerializeField] private bool randomRotation = true;

    private Terrain terrain;

    void Start()
    {
        terrain = FindObjectOfType<Terrain>();

        Debug.Log("Terrain found: " + terrain.name);
        SpawnRocks();
    }

    void SpawnRocks()
    {
        if (terrain == null) return;

        TerrainData terrainData = terrain.terrainData;
        float terrainWidth = terrainData.size.x;
        float terrainLength = terrainData.size.z;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject rockPrefab = rockPrefabs[Random.Range(0, rockPrefabs.Length)];

            float randomX = Random.Range(0, terrainWidth);
            float randomZ = Random.Range(0, terrainLength);

            float terrainY = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + terrain.transform.position.y;

            Vector3 spawnPosition = new Vector3(randomX, terrainY, randomZ);

            GameObject rock = Instantiate(rockPrefab, spawnPosition, Quaternion.identity);

            float randomScale = Random.Range(minScale, maxScale);
            rock.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            if (randomRotation)
            {
                rock.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            }
        }

        Debug.Log("Rock spawned: "+ spawnCount );
    }
}