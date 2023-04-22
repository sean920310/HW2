using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weaponIconUpdate : MonoBehaviour
{
    [SerializeField] GameObject itemPanel;
    Image spriteImgFromItemPanel;
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (itemPanel.active == false)
            return;

        if(spriteImgFromItemPanel == null)
            spriteImgFromItemPanel = itemPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();

        if (itemPanel.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.active)
        {
            this.GetComponent<Image>().sprite = spriteImgFromItemPanel.sprite;
            this.GetComponent<Image>().color = spriteImgFromItemPanel.color;
        }
        else
        {
            this.GetComponent<Image>().color = Color.clear;
        }
    }
}
