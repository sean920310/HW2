using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.UI
{
    // reference: https://github.com/SunnyValleyStudio/Unity-Inventory-system-using-SO-and-MVC
    public class BackpackItem : MonoBehaviour, IPointerClickHandler,
            IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        [SerializeField] Image borderImage;
        [SerializeField] Image itemImage;
        [SerializeField] TMP_Text quantityText;

        public event Action<BackpackItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnItemRightMouseBtnClicked;


        private bool empty = true;

        public void Awake()
        {

        }

        public void ResetData()
        {
            itemImage.gameObject.SetActive(false);
            empty = true;
        }
        public void Deselect()
        {
            borderImage.enabled = false;
        }

        public void Select()
        {
            borderImage.enabled = true;
        }

        public void SetData(Sprite sprite, int quantity)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            quantityText.text = quantity.ToString();
            empty = false;
        }
        public void OnDestroy()
        {
            ResetData();
        }
        #region Event Handler
        public void OnDrag(PointerEventData eventData)
        {

        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (empty) { return; }
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            if (pointerEventData.button == PointerEventData.InputButton.Right)
            {
                OnItemRightMouseBtnClicked?.Invoke(this);
            }
            else
            {
                OnItemClicked?.Invoke(this);
            }
        }
        #endregion
    }

}
