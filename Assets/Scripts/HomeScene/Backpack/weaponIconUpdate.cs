using Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weaponIconUpdate : MonoBehaviour
{
    [SerializeField] GameObject itemPanel;
    [SerializeField] BackpackController bc;
    [SerializeField] int backpackSizeShiftByEnd;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!bc.inventoryData.inventoryItems[bc.inventoryData.inventoryItems.Count - backpackSizeShiftByEnd].IsEmpty)
        {
            this.GetComponent<Image>().sprite = bc.inventoryData.inventoryItems[bc.inventoryData.inventoryItems.Count - backpackSizeShiftByEnd].item.ItemImage;
            this.GetComponent<Image>().color = Color.white;
        }
        else
        {
            this.GetComponent<Image>().color = Color.clear;
        }
    }
}
