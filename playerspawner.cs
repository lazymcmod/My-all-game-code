using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform player;

    void Start()
    {
        float noiseScale = 0.1f;
        int maxHeight = 15;

        int x = Mathf.FloorToInt(player.position.x);
        int z = Mathf.FloorToInt(player.position.z);

        int height = Mathf.FloorToInt(Mathf.PerlinNoise(x * noiseScale, z * noiseScale) * maxHeight);

        // Spawn above terrain
        player.position = new Vector3(x, height + 5, z);
    }
}
