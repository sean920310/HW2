using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// reference: https://github.com/SunnyValleyStudio/Unity-Inventory-system-using-SO-and-MVC

namespace Inventory.UI
{
    public class BackpackPageUI : MonoBehaviour
    {
        [SerializeField]
        private BackpackItem itemPrefab;

        [SerializeField]
        private RectTransform contentPanel;
        [SerializeField]
        private RectTransform mainWeaponPanel;
        [SerializeField]
        private RectTransform blockingWeaponPanel;
        [SerializeField]
        private RectTransform potionPanel;

        [SerializeField] private BackpackDescription backpackDescription;

        [SerializeField] private MouseFollowerBehaviour mouseFollowerBehaviour;

        List<BackpackItem> listOfUIItems = new List<BackpackItem>();

        private int currentDragItemIdx = -1;

        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;
        public event Action<int, int> OnSwapItems;

        [SerializeField]
        private ItemActionPanel actionPanel;

        private void Awake()
        {
            backpackDescription.ResetDescription();
            mouseFollowerBehaviour.Toggle(false);
        }

        public void InitializeBackpackUI(int inventorysize)
        {

            for (int i = 0; i < inventorysize; i++)
            {
                BackpackItem uiItem =
                    Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);


                if (i >= inventorysize - 3)
                {
                    if(i == inventorysize - 3)
                    {
                        uiItem.transform.SetParent(mainWeaponPanel);

                    }
                    else if (i == inventorysize - 2)
                    {
                        uiItem.transform.SetParent(blockingWeaponPanel);
                    }
                    else if (i == inventorysize - 1)
                    {
                        uiItem.transform.SetParent(potionPanel);
                    }
                    uiItem.GetComponent<RectTransform>().sizeDelta = new Vector2(100,100);
                    uiItem.GetComponent<RectTransform>().localPosition = Vector3.zero;
                    uiItem.GetComponent<Image>().color = new Color(0,0,0,0);
                }
                else
                    uiItem.transform.SetParent(contentPanel);

                uiItem.OnItemClicked += handleItemSelection;
                uiItem.OnItemBeginDrag += handleItemBeginDrag;
                uiItem.OnItemDroppedOn += handleSwap;
                uiItem.OnItemEndDrag += handleItemEndDrag;
                uiItem.OnItemRightMouseBtnClicked += handleShowItemAction;

                listOfUIItems.Add(uiItem);
            }
        }
        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }
        internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            backpackDescription.SetDescription(itemImage, name, description);
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
        }
        private void HandleItemSelection(BackpackItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke(index);
        }
        public void ResetSelection()
        {
            backpackDescription.ResetDescription();
            DeselectAllItems();
        }
        public void AddAction(string actionName, Action performAction)
        {
            actionPanel.AddButon(actionName, performAction);
        }

        public void ShowItemAction(int itemIndex)
        {
            actionPanel.Toggle(true);
            actionPanel.transform.position = listOfUIItems[itemIndex].transform.position;
        }

        private void DeselectAllItems()
        {
            foreach (BackpackItem item in listOfUIItems)
            {
                item.Deselect();
            }
            actionPanel.Toggle(false);
        }
        #region handler
        private void handleShowItemAction(BackpackItem BackpackItemUI)
        {

            Debug.Log("SHOW");
            int index = listOfUIItems.IndexOf(BackpackItemUI);

            if (index == -1)
            {
                return;
            }

            OnItemActionRequested?.Invoke(index);
        }

        private void handleSwap(BackpackItem BackpackItemUI)
        {
            int index = listOfUIItems.IndexOf(BackpackItemUI);

            Debug.Log("END " + index.ToString());

            if (index == -1)
            {
                return;
            }

            OnSwapItems?.Invoke(currentDragItemIdx, index);
            handleItemSelection(BackpackItemUI);
        }

        private void ResetDraggeditem()
        {
            mouseFollowerBehaviour.Toggle(false);
            currentDragItemIdx = -1;
        }

        private void handleItemBeginDrag(BackpackItem BackpackItemUI)
        {
            int index = listOfUIItems.IndexOf(BackpackItemUI);
            Debug.Log("BEGIN");
            if (index == -1)
                return;

            currentDragItemIdx = index;
            handleItemSelection(BackpackItemUI);
            OnStartDragging?.Invoke(index);
        }
        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollowerBehaviour.SetData(sprite, quantity);
            mouseFollowerBehaviour.Toggle(true);
        }

        private void handleItemEndDrag(BackpackItem BackpackItemUI)
        {
            ResetDraggeditem();
        }

        private void handleItemSelection(BackpackItem BackpackItemUI)
        {
            int index = listOfUIItems.IndexOf(BackpackItemUI);

            if (index == -1)
                return;

            OnDescriptionRequested?.Invoke(index);
        }
        internal void ResetAllItems()
        {
            foreach (var item in listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        private void OnDestroy()
        {
            foreach (var item in listOfUIItems)
            {

                item.OnItemClicked -= handleItemSelection;
                item.OnItemBeginDrag -= handleItemBeginDrag;
                item.OnItemDroppedOn -= handleSwap;
                item.OnItemEndDrag -= handleItemEndDrag;
                item.OnItemRightMouseBtnClicked -= handleShowItemAction;
            }
            ResetAllItems();
        }

        #endregion

        #region UI Show/Hide
        public void Show()
        {
            backpackDescription.ResetDescription();
            ResetSelection();

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            ResetDraggeditem();
        }

        #endregion
    }

}