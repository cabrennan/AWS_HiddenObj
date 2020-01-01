using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;

public class Inventory : MonoBehaviour
{

    #region Singleton

    // items list will need Sprite association for game image
    // In heiarchy attach approriate collider to sprite
    // Attach interactibile script to item in heiarchy
    // Attach itempickup script to item in heiarchy
    // In inspector insert Inventory/items/item to variable in ItemPickup(script) dialog box. 

    public static Inventory instance;
    public List<Item> items;
    public Queue<Item> itemQueue;

    void Awake()
    {
        instance = this;
        itemQueue = new Queue<Item>();
        GetSceneItems();
        Debug.Log("after Inventory awake");
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 15; // number of inventory slots 
    public Vector3 position;

    private void Start()
    {


    }

    public void Add(Item item)
    {
        items.Add(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }

    }

    public void Remove(Item item)
    {
        items.Remove(item);
        item.enabled = false;

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }

    }

    public void GetSceneItems()
    {
        string[] itemGuids = AssetDatabase.FindAssets("t:Item");
        foreach (string s in itemGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(s);
            Item i = AssetDatabase.LoadAssetAtPath<Item>(path);
            this.Add(i);
            itemQueue.Enqueue(i);
        }

    }

}