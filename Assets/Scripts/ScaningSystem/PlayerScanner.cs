using UnityEngine;
public class PlayerScanner : MonoBehaviour
{
    [SerializeField] private float scanDistance = 25f;
    [SerializeField] private LayerMask scanLayer;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject collectionUI;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            ScanForCollectible();
    }
    private void ScanForCollectible()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, scanDistance, scanLayer))
        {
            if (hit.collider.TryGetComponent(out CollectibleObject collectible))
                collectible.Collect();
        }
    }
    private void OnDrawGizmos()
    {
        if (playerCamera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * scanDistance);
        }
    }
}
