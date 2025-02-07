using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectionSlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemText;
    [SerializeField] private CollectibleItem assignedItem;

    public void SetItem(CollectibleItem item)
    {
        assignedItem = item;
        UpdateSlot();
    }

    public void UpdateSlot()
    {
        if (assignedItem != null && assignedItem.isCollected)
        {
            itemImage.sprite = assignedItem.itemSprite;
            itemImage.color = Color.white;
            itemText.text = assignedItem.itemName;
        }
        else
        {
            itemImage.sprite = null;
            itemImage.color = Color.black;
            itemText.text = "???";
        }
    }
}
