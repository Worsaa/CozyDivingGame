using UnityEngine;
using System;
using System.Collections.Generic;
public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance { get; private set; }
    public List<CollectibleItem> allItems;
    public event Action<CollectibleItem> OnItemCollected;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void CollectItem(CollectibleItem item)
    {
        if (item == null || !item.isCollected)
            return;
        OnItemCollected?.Invoke(item);
    }
}
