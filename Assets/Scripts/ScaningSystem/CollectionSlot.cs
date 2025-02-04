using UnityEngine;
using UnityEngine.UI;

public class CollectionSlot : MonoBehaviour
{
    public Image itemImage;
    public CollectibleItem assignedItem;

    public void UpdateSlot()
    {
        if (assignedItem.isCollected)
        {
            itemImage.sprite = assignedItem.itemSprite;
            itemImage.color = Color.white;
        }
        else
        {
            itemImage.sprite = null;
            itemImage.color = Color.black;
        }
    }
}
