using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    public Camera playerCamera;
    public float placeRange = 5f;
    public LayerMask blockLayerMask; // ✅ Assign this to only "Block" layer in Inspector

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(ray, out RaycastHit hit, placeRange, blockLayerMask)) // ✅ Use layer mask
            {
                // Calculate place position aligned to grid
                Vector3 placePosition = hit.point + hit.normal / 2f;
                placePosition = new Vector3(
                    Mathf.Round(placePosition.x),
                    Mathf.Round(placePosition.y),
                    Mathf.Round(placePosition.z)
                );

                // ✅ Check if a block already exists at that spot
                Collider[] colliders = Physics.OverlapBox(placePosition, Vector3.one * 0.45f);
                if (colliders.Length == 0)
                {
                    GameObject blockToPlace = InventoryManager.Instance.GetSelectedBlockPrefab();
                    if (blockToPlace != null)
                    {
                        Instantiate(blockToPlace, placePosition, Quaternion.identity);
                    }
                }
            }
        }
    }
}
