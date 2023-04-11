using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// reference: https://github.com/SunnyValleyStudio/Unity-Inventory-system-using-SO-and-MVC/blob/main/UI/MouseFollower.cs
public class MouseFollowerBehaviour : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private BackpackItem item;

    public void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
        item = GetComponentInChildren<BackpackItem>();
    }

    public void SetData(Sprite sprite, int quantity)
    {
        item.SetData(sprite, quantity);
    }
    void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            Input.mousePosition,
            canvas.worldCamera,
            out position
                );
        transform.position = canvas.transform.TransformPoint(position);
    }

    public void Toggle(bool val)
    {
        gameObject.SetActive(val);
    }
}
