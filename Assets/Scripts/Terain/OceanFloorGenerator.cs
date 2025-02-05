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
    public int plantDensity = 50; 

    private Material sharedMaterial;

    private void Start()
    {
        sharedMaterial = GetComponent<MeshRenderer>().sharedMaterial;
        GenerateChunks();
        PlacePrefabs();
        PlacePlants();
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

                MeshFilter meshFilter = chunk.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = chunk.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = sharedMaterial;

                meshFilter.mesh = GenerateMesh(x, z);
            }
        }
    }

    Mesh GenerateMesh(int offsetX, int offsetZ)
    {
        Mesh mesh = new Mesh();
        int vertCountX = Mathf.Min(chunkSize + 1, width - offsetX + 1);
        int vertCountZ = Mathf.Min(chunkSize + 1, depth - offsetZ + 1);
        Vector3[] vertices = new Vector3[vertCountX * vertCountZ];
        int[] triangles = new int[chunkSize * chunkSize * 6];

        for (int z = 0, i = 0; z < vertCountZ; z++)
        {
            for (int x = 0; x < vertCountX; x++, i++)
            {
                float y = Mathf.PerlinNoise((offsetX + x) * 0.1f, (offsetZ + z) * 0.1f) * scale;
                vertices[i] = new Vector3(x, y, z);
            }
        }

        int tris = 0;
        for (int z = 0, vert = 0; z < chunkSize && z + offsetZ < depth; z++, vert++)
        {
            for (int x = 0; x < chunkSize && x + offsetX < width; x++, vert++)
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

        HashSet<int> usedIndices = new HashSet<int>();

        foreach (GameObject prefab in prefabs)
        {
            int randomX, randomZ, randomIndex;

            do
            {
                randomX = Random.Range(0, width);
                randomZ = Random.Range(0, depth);
                randomIndex = randomZ * (width + 1) + randomX;
            } while (usedIndices.Contains(randomIndex));

            usedIndices.Add(randomIndex);

            float y = Mathf.PerlinNoise(randomX * 0.1f, randomZ * 0.1f) * scale;
            Vector3 spawnPosition = new Vector3(randomX, -100, randomZ);

            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }

    void PlacePlants()
    {
        if (plants == null || plants.Count == 0)
            return;

        for (int i = 0; i < plantDensity; i++)
        {
            int randomX = Random.Range(0, width);
            int randomZ = Random.Range(0, depth);

            float y = Mathf.PerlinNoise(randomX * 0.1f, randomZ * 0.1f) * scale;
            Vector3 spawnPosition = new Vector3(randomX, -100, randomZ);

            GameObject plantPrefab = plants[Random.Range(0, plants.Count)];
            Instantiate(plantPrefab, spawnPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
        }
    }
}
