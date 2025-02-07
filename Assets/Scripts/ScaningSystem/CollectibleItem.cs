using UnityEngine;

public enum RarityGrade
{
    Trash,
    Common,
    Green,
    Blue,
    Epic,
    Legendary
}

[CreateAssetMenu(fileName = "NewCollectible", menuName = "Collectible Item")]
public class CollectibleItem : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public bool isCollected;
    public RarityGrade rarity;
}
