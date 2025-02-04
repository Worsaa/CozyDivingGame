using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CollectionUI : MonoBehaviour
{
    public List<CollectionSlot> slots;

    public void RefreshUI()
    {
        foreach (var slot in slots)
        {
            slot.UpdateSlot();
        }
    }
}
