using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
public class OceanFloorGenerator : MonoBehaviour
{
    public int width = 200;
    public int depth = 200;
    public float scale = 5f;
    public Vector3 startPosition = Vector3.zero;
    public int chunkSize = 50;

    public List<GameObject> prefabs;
    public List<GameObject> plants;
    public List<GameObject> stones;
    public int plantDensity = 50;
    public int stoneDensity = 50;

    public float canyonDepth = 5f;
    public float canyonThreshold = 0.7f;
    public float canyonScale = 0.05f;

    private Material sharedMaterial;
    public Material causticsMaterial;
    private Dictionary<Vector2Int, float> heightMap = new Dictionary<Vector2Int, float>();

    private void Start()
    {
        sharedMaterial = GetComponent<MeshRenderer>().sharedMaterial;
        GenerateHeightMap();
        GenerateChunks();
        PlacePrefabs();
        PlacePlants();
        PlaceStones();
    }

    void GenerateHeightMap()
    {
        for (int z = 0; z <= depth; z++)
        {
            for (int x = 0; x <= width; x++)
            {
                float baseHeight = Mathf.PerlinNoise((x + startPosition.x) * 0.1f, (z + startPosition.z) * 0.1f) * scale;
                float canyonNoise = Mathf.PerlinNoise(x * canyonScale, z * canyonScale);
                if (canyonNoise > canyonThreshold)
                {
                    float factor = (canyonNoise - canyonThreshold) / (1f - canyonThreshold);
                    baseHeight -= factor * canyonDepth;
                }
                heightMap[new Vector2Int(x, z)] = baseHeight;
            }
        }
    }

    void GenerateChunks()
    {
        for (int x = 0; x < width; x += chunkSize)
        {
            for (int z = 0; z < depth; z += chunkSize)
            {
                GameObject chunk = new GameObject($"Chunk_{x}_{z}");
                chunk.transform.parent = transform;
                chunk.transform.position = new Vector3(startPosition.x + x, startPosition.y, startPosition.z + z);
                chunk.layer = LayerMask.NameToLayer("Ground");

                MeshFilter mf = chunk.AddComponent<MeshFilter>();
                MeshRenderer mr = chunk.AddComponent<MeshRenderer>();

                mr.sharedMaterials = new Material[] { sharedMaterial, causticsMaterial };

                Mesh mesh = GenerateMesh(x, z);
                mf.mesh = mesh;

                MeshCollider mc = chunk.AddComponent<MeshCollider>();
                mc.sharedMesh = mesh;
            }
        }
    }

    Mesh GenerateMesh(int offsetX, int offsetZ)
    {
        Mesh mesh = new Mesh();
        int vertCountX = chunkSize + 1;
        int vertCountZ = chunkSize + 1;
        Vector3[] vertices = new Vector3[vertCountX * vertCountZ];
        int[] triangles = new int[chunkSize * chunkSize * 6];

        for (int z = 0, i = 0; z < vertCountZ; z++)
        {
            for (int x = 0; x < vertCountX; x++, i++)
            {
                int globalX = offsetX + x;
                int globalZ = offsetZ + z;
                float y = 0;
                if (heightMap.TryGetValue(new Vector2Int(globalX, globalZ), out float h))
                    y = h;
                vertices[i] = new Vector3(x, y, z);
            }
        }

        int tris = 0;
        int vert = 0;
        for (int z = 0; z < chunkSize; z++, vert++)
        {
            for (int x = 0; x < chunkSize; x++, vert++)
            {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + vertCountX;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + vertCountX;
                triangles[tris + 5] = vert + vertCountX + 1;
                tris += 6;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }

    void PlacePrefabs()
    {
        if (prefabs == null || prefabs.Count == 0)
            return;
        HashSet<Vector2Int> usedPositions = new HashSet<Vector2Int>();
        foreach (GameObject prefab in prefabs)
        {
            Vector2Int pos;
            do { pos = new Vector2Int(Random.Range(0, width), Random.Range(0, depth)); }
            while (usedPositions.Contains(pos));
            usedPositions.Add(pos);
            if (!heightMap.TryGetValue(pos, out float y))
                continue;

            Vector3 spawnPos = new Vector3(pos.x + startPosition.x, y, pos.y + startPosition.z);
            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }

    void PlaceStones()
    {
        if (stones == null || stones.Count == 0)
            return;
        for (int i = 0; i < stoneDensity; i++)
        {
            Vector2Int pos = new Vector2Int(Random.Range(0, width), Random.Range(0, depth));
            if (!heightMap.TryGetValue(pos, out float y))
                continue;
            Vector3 spawnPos = new Vector3(pos.x + startPosition.x, y, pos.y + startPosition.z);
            GameObject stone = stones[Random.Range(0, stones.Count)];
            Instantiate(stone, spawnPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
        }
    }

    void PlacePlants()
    {
        if (plants == null || plants.Count == 0)
            return;
        float threshold = 0.01f;
        for (int i = 0; i < plantDensity; i++)
        {
            Vector2Int pos = new Vector2Int(Random.Range(0, width), Random.Range(0, depth));
            if (!heightMap.TryGetValue(pos, out float y))
                continue;
            float noise = Mathf.PerlinNoise(pos.x * 0.05f, pos.y * 0.05f);
            if (noise < threshold)
                continue;
            Vector3 spawnPos = new Vector3(pos.x + startPosition.x, y, pos.y + startPosition.z);
            GameObject plant = plants[Random.Range(0, plants.Count)];
            Instantiate(plant, spawnPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
        }
    }
}
