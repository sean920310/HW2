using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Inventory.UI;
using Inventory.Model;
using Unity.VisualScripting;
using System;

namespace Inventory
{
    public class BackpackController : MonoBehaviour, IDataPersistence
    {
        [SerializeField]
        private BackpackPageUI inventoryUI;

        [SerializeField]
        public InventorySO inventoryData;

        public List<InventoryItem> initItems = new List<InventoryItem>();

        [SerializeField]
        private AudioClip dropClip;

        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private Item ItemPrefab;

        [SerializeField]
        private PickUpSystem pickUpSystem;

        public bool isInit = false;

        private void Start()
        {
            if (!isInit)
            {
                PrepareUI();
                PrepareInventoryData();
            }
        }

        public void LoadData(GameData data)
        {
            isInit = true;
            initItems = data.backpackItems;
            PrepareUI();
            PrepareInventoryData();
        }

        public void SaveData(GameData data)
        {
            data.backpackItems = inventoryData.inventoryItems;
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            int i = -1;
            foreach (InventoryItem item in initItems)
            {
                i++;
                if (item.IsEmpty) { continue; }

                inventoryData.AddItemByIdx(item, i);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeBackpackUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                inventoryUI.ShowItemAction(itemIndex);
                //inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
            }

        }

        private void DropItem(int itemIndex, int quantity)
        {
            pickUpSystem.dontPickup = true;

            Item tmpItem = Instantiate(ItemPrefab);

            tmpItem.InventoryItem = inventoryData.GetItemAt((int)itemIndex).item;
            tmpItem.Quantity= quantity;

            tmpItem.transform.position = transform.position + Vector3.up * 2;

            StartCoroutine(ItemCoolDown(tmpItem));

            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();
            audioSource.PlayOneShot(dropClip);
        }

        IEnumerator ItemCoolDown(Item tmpItem)
        {
            tmpItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(1.0f, 1.0f) * 2f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(1.0f);
            pickUpSystem.dontPickup = false;

        }
        public void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject, inventoryItem.itemState);
                audioSource.PlayOneShot(itemAction.actionSFX);
                if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                    inventoryUI.ResetSelection();
            }
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }
        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem);
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage,
                item.name, description);
        }
        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem.item.Description);
            sb.AppendLine();
            for (int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName} " +
                    $": {inventoryItem.itemState[i].value} / " +
                    $"{inventoryItem.item.DefaultParametersList[i].value}");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (inventoryUI.isActiveAndEnabled == false)
                {
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UpdateData(item.Key,
                            item.Value.item.ItemImage,
                            item.Value.quantity);
                    }
                    inventoryUI.Show();
                }
                else
                {
                    inventoryUI.Hide();
                }

            }
        }
        public void OnDestroy()
        {

            inventoryData.OnInventoryUpdated -= UpdateInventoryUI;
        }
    }
}