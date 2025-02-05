using System.Collections.ObjectModel;
using UnityEngine;

public class PlayerScanner : MonoBehaviour
{
    public float scanDistance = 25f;
    public LayerMask scanLayer;
    public Camera playerCamera;
    public GameObject collectionUI;
    public CollectionUI ui;
    public KeyCode toggleKey = KeyCode.Tab;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ScanForCollectible();
        }
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleUI();
            ui.RefreshUI();
        }
    }

    public void ToggleUI()
    {
        collectionUI.SetActive(!collectionUI.activeSelf);
    }
    void ScanForCollectible()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, scanDistance, scanLayer))
        {
            CollectibleObject collectible = hit.collider.GetComponent<CollectibleObject>();
            if (collectible != null)
            {
                collectible.Collect();
            }
        }
    }

    void OnDrawGizmos()
    {
        if (playerCamera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * scanDistance);
        }
    }
}
