using UnityEngine;

[CreateAssetMenu(fileName = "NewCollectible", menuName = "Collectible Item")]
public class CollectibleItem : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public bool isCollected;
}
