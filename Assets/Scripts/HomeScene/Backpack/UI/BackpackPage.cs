using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// reference: https://github.com/SunnyValleyStudio/Unity-Inventory-system-using-SO-and-MVC
public class BackpackPage : MonoBehaviour
{
    [SerializeField]
    private BackpackItem itemPrefab;

    [SerializeField]
    private RectTransform contentPanel;

    [SerializeField] private BackpackDescription backpackDescription;

    [SerializeField] private MouseFollowerBehaviour mouseFollowerBehaviour;

    List<BackpackItem> listOfUIItems = new List<BackpackItem>();

    private int currentDragItemIdx = -1;

    public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;
    public event Action<int, int> OnSwapItems;

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
    void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
    {
        backpackDescription.SetDescription(itemImage, name, description);
        //DeselectAllItems();
        listOfUIItems[itemIndex].Select();
    }
    private void ResetSelection()
    {
        backpackDescription.ResetDescription();
        DeselectAllItem();
    }
    private void DeselectAllItem()
    {
        foreach(BackpackItem item in listOfUIItems)
        {
            item.Deselect();
        }
    }
    #region handler
    private void handleShowItemAction(BackpackItem BackpackItemUI)
    {

    }

    private void handleSwap(BackpackItem BackpackItemUI)
    {
        int index = listOfUIItems.IndexOf(BackpackItemUI);

        if (index == -1)
        {
            return;
        }

        OnSwapItems?.Invoke(currentDragItemIdx, index);
    }

    private void ResetDraggeditem()
    {
        mouseFollowerBehaviour.Toggle(false);
        currentDragItemIdx = -1;
    }

    private void handleItemBeginDrag(BackpackItem BackpackItemUI)
    {
        int index = listOfUIItems.IndexOf(BackpackItemUI);

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
