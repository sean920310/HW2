using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    [SerializeField] PlayerManager pm;

    [SerializeField] Image HealthBarFiller;
    [SerializeField] TextMeshProUGUI healthText;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HealthBarFiller.fillAmount = (float)pm.Health / pm.MaxHealth;
        healthText.text = pm.Health.ToString() + " / " + pm.MaxHealth.ToString();
    }
}
