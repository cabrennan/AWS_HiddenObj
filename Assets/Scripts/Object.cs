using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "Object")]
public class Object : ScriptableObject
{

    new public string name = "New Object";    // Name of the item
    public Sprite icon = null;              // Item icon
    public bool showInInventory = true;
    public bool enabled = false; // If true can be clicked on
    public Vector3 pos;

    public GameObject obj;
    public Collider2D objCollider;
    private Camera cam;
    private Plane[] planes;


    public Vector3 center;
    // Called when the item is pressed in the inventory
    public virtual void Use()
    {
        // Use the item

    }


    // Call this method to remove the item from inventory
    public void RemoveFromInventory()
    {
        // Inventory.instance.Remove(this);
    }

    public void SetPosition()
    {
        Debug.Log("Positioning: " + this.name);

       
    }

}
