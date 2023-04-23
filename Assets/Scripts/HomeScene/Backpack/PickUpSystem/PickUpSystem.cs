using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;
    private void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 12);
        Physics2D.IgnoreLayerCollision(9, 12);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.transform.root.GetComponent<Item>();

        if (item != null)
        {
            int reminder = 0;

            if (!item.used)
                reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity);

            if (reminder == 0)
            {
                item.DestroyItem();
                item.used = true;
            }
            else
                item.Quantity = reminder;
        }
    }
}