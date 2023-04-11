using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    [SerializeField] PlayerManager pm;

    [SerializeField] Image HealthBarFiller;
    [SerializeField] Image HealthBarAnimationFiller;
    [SerializeField] TextMeshProUGUI healthText;

    [SerializeField] float healthAnimationWaitToDropTime;
    [SerializeField] float healthAnimationDropSpeed;
    [SerializeField] float healthAnimationDropAmount;
    [SerializeField] bool isBloodDropAnimateOn;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HealthBarFiller.fillAmount = (float)pm.Health / pm.MaxHealth;

        if(isBloodDropAnimateOn && HealthBarFiller.fillAmount != HealthBarAnimationFiller.fillAmount)
        {
            StartCoroutine(healthBarAnimation());
        }

        healthText.text = pm.Health.ToString() + " / " + pm.MaxHealth.ToString();
    }

    IEnumerator healthBarAnimation()
    {
        yield return new WaitForSeconds(healthAnimationWaitToDropTime);

        while (HealthBarFiller.fillAmount < HealthBarAnimationFiller.fillAmount)
        {
            HealthBarAnimationFiller.fillAmount -= healthAnimationDropAmount;
            yield return new WaitForSeconds(healthAnimationDropSpeed);
        }
        HealthBarAnimationFiller.fillAmount = HealthBarFiller.fillAmount;
    }
}
