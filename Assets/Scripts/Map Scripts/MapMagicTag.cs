using UnityEngine;
using MapMagic.Core;

public class MapMagicTag : MonoBehaviour
{
    public string terrainTag = "Terrain";

    void Start()
    {
        Invoke("TagGeneratedTerrain", 2f);
    }

    void TagGeneratedTerrain()
    {
        Terrain terrain = FindObjectOfType<Terrain>();
        if (terrain != null)
        {
            terrain.gameObject.tag = terrainTag;
        }
        else
        {
            Debug.LogWarning("No Terrain Found, error.");
        }
    }
}