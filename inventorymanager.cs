using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public GameObject[] blockPrefabs; // 7 Block prefabs assign karo
    public Image[] slotImages;        // Slot images reference
    private int selectedIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        for (int i = 0; i < 7; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                SelectSlot(i);
            }
        }
    }

    void SelectSlot(int index)
    {
        selectedIndex = index;
        HighlightSlot();
    }

    void HighlightSlot()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i].color = (i == selectedIndex) ? Color.yellow : Color.white;
        }
    }

    public GameObject GetSelectedBlockPrefab()
    {
        return blockPrefabs[selectedIndex];
    }
}
