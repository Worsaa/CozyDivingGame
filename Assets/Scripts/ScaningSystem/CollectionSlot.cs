using UnityEngine;
using UnityEngine.UI;

public class CollectionSlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private CollectibleItem assignedItem;

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
