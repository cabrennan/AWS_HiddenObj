using UnityEngine;
using TMPro;

public class InventorySlot : MonoBehaviour
{

    Item item;
    public TextMeshProUGUI textmesh;
    public Vector3 pos;
    public GameObject obj;


    public void AddItem(Item newItem)
    {
        Debug.Log("Inventory Slot, adding item: " + newItem.name);

        item = newItem;
        textmesh = GetComponent<TextMeshProUGUI>();
        textmesh.SetText(newItem.name);
        item.enabled = true;
        //item.SetPosition();
        //item.NewSetPosition();
        //item.SPos();

    }

    public void ClearSlot()
    {
        textmesh.SetText("");
    }
}
