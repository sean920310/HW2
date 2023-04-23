using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class moneyTextUpdate : MonoBehaviour
{
    [SerializeField] PlayerManager pm;
    TextMeshProUGUI TM;

    // Start is called before the first frame update
    void Start()
    {
        TM = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        TM.text = "x " + pm.Money.ToString();
    }
}
