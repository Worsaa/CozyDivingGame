using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    public CollectibleItem collectibleData;
    private bool isCollected = false;

    public void Collect()
    {
        if (!isCollected)
        {
            isCollected = true;
            CollectionManager.Instance.CollectItem(collectibleData);
            //gameObject.SetActive(false); 
        }
    }
}
