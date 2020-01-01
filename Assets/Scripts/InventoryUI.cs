using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    public GameObject inventoryUI;  //the entire UI
    Inventory inventory;


    // Use this for initialization
    void Start()
    {

        Debug.Log("Starting inventory UI");
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;
        UpdateUI();

    }

    private void SetSlotText(InventorySlot slot)
    {
        if (inventory.itemQueue.Count > 0)
        {

            Item thisItem;
            thisItem = inventory.itemQueue.Dequeue();
            Debug.Log("Setting slot with item: " + thisItem.name);
            slot.AddItem(thisItem);
        }
        else
        {
            slot.ClearSlot();
        }
    }

    void UpdateUI()
    {
        InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].textmesh)
            {
                SetSlotText(slots[i]);
            }
            else
            {
                bool keep = false;
                foreach (Item item in inventory.items)
                {
                    if (slots[i].textmesh.text == item.name)
                    {
                        keep = true;
                    }
                }
                if (!keep)
                {
                    SetSlotText(slots[i]);
                }
            }
        }
    }

}
