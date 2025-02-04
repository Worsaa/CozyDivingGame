using UnityEngine;
using System.Collections.Generic;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance;
    public List<CollectibleItem> allItems;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void CollectItem(CollectibleItem item)
    {
        item.isCollected = true;
        UpdateUI();
    }

    public void UpdateUI()
    {
        FindObjectOfType<CollectionUI>().RefreshUI();
    }
}
