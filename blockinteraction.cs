using UnityEngine;

public class BlockInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactRange = 5f;
    public LayerMask blockLayerMask;

    // Block prefabs
    public GameObject grassBlockPrefab;
    public GameObject dirtPrefab;
    public GameObject sandPrefab;
    public GameObject logPrefab;
    public GameObject leavesPrefab;
    public GameObject deepslatePrefab;
    public GameObject stonePrefab;

    // Particle prefabs for each block
    public GameObject grassParticle;
    public GameObject dirtParticle;
    public GameObject sandParticle;
    public GameObject logParticle;
    public GameObject leavesParticle;
    public GameObject deepslateParticle;
    public GameObject stoneParticle;

    private GameObject heldBlockPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) BreakBlock();

        // Optional block switching (not necessary for particles)
        if (Input.GetKeyDown(KeyCode.Alpha1)) heldBlockPrefab = grassBlockPrefab;
        if (Input.GetKeyDown(KeyCode.Alpha2)) heldBlockPrefab = dirtPrefab;
        if (Input.GetKeyDown(KeyCode.Alpha3)) heldBlockPrefab = sandPrefab;
        if (Input.GetKeyDown(KeyCode.Alpha4)) heldBlockPrefab = logPrefab;
        if (Input.GetKeyDown(KeyCode.Alpha5)) heldBlockPrefab = leavesPrefab;
        if (Input.GetKeyDown(KeyCode.Alpha6)) heldBlockPrefab = deepslatePrefab;
        if (Input.GetKeyDown(KeyCode.Alpha7)) heldBlockPrefab = stonePrefab;
    }

    void BreakBlock()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, blockLayerMask))
        {
            GameObject brokenBlock = hit.collider.gameObject;
            string blockName = brokenBlock.name.Replace("(Clone)", "").Trim();
            Vector3 spawnPos = brokenBlock.transform.position;

            // Spawn matching particle effect
            switch (blockName)
            {
                case "GrassBlock":
                    heldBlockPrefab = grassBlockPrefab;
                    SpawnParticle(grassParticle, spawnPos);
                    break;
                case "Dirt":
                    heldBlockPrefab = dirtPrefab;
                    SpawnParticle(dirtParticle, spawnPos);
                    break;
                case "Sand":
                    heldBlockPrefab = sandPrefab;
                    SpawnParticle(sandParticle, spawnPos);
                    break;
                case "Log":
                    heldBlockPrefab = logPrefab;
                    SpawnParticle(logParticle, spawnPos);
                    break;
                case "Leaves":
                    heldBlockPrefab = leavesPrefab;
                    SpawnParticle(leavesParticle, spawnPos);
                    break;
                case "Deepslate":
                    heldBlockPrefab = deepslatePrefab;
                    SpawnParticle(deepslateParticle, spawnPos);
                    break;
                case "Stone":
                    heldBlockPrefab = stonePrefab;
                    SpawnParticle(stoneParticle, spawnPos);
                    break;
                default:
                    heldBlockPrefab = grassBlockPrefab;
                    SpawnParticle(grassParticle, spawnPos);
                    break;
            }

            Destroy(brokenBlock);
        }
    }

    void SpawnParticle(GameObject particlePrefab, Vector3 position)
    {
        if (particlePrefab != null)
        {
            GameObject p = Instantiate(particlePrefab, position, Quaternion.identity);
            Destroy(p, 1f); // Auto destroy after 2 seconds
        }
    }
}
