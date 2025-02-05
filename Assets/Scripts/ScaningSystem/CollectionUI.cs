using UnityEngine;
using System.Collections.Generic;

public class CollectionUI : MonoBehaviour
{
    [SerializeField] private List<CollectionSlot> slots;

    private void OnEnable()
    {
        CollectionManager.Instance.OnItemCollected += RefreshUI;
    }

    private void OnDisable()
    {
        CollectionManager.Instance.OnItemCollected -= RefreshUI;
    }

    public void RefreshUI(CollectibleItem collectedItem = null)
    {
        foreach (var slot in slots)
        {
            slot.UpdateSlot();
        }
    }
}
