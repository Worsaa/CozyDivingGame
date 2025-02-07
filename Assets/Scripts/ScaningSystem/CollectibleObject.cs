using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    [SerializeField] private CollectibleItem collectibleData;

    public void Collect()
    {
        if (collectibleData.isCollected)
            return;
        collectibleData.isCollected = true;
        CollectionManager.Instance.CollectItem(collectibleData);
    }
}
