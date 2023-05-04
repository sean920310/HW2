using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamageEffect : MonoBehaviour
{
    static private VolumeProfile volume;

    public AnimationCurve lowHealthAnim;

    static private bool lowHealthActive;
    static private bool getDamageActive;

    private float timeCount = 0;
    static private Vignette vignette;

    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>().profile;
        volume.TryGet<Vignette>(out vignette);
        getDamageActive = false;
        LowHealth(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(lowHealthActive && !getDamageActive)
            UpdateLowHealth();
    }

    /// <summary>
    /// use StartCoroutine() to call
    /// </summary>
    static public IEnumerator GetDamage()
    {
        getDamageActive = true;
        vignette.active = true;
        vignette.color.value = new Color(1f, 0.2f, 0.2f);
        yield return new WaitForSeconds(0.18f);
        vignette.active = lowHealthActive;
        getDamageActive = false;
    }

    static public void LowHealth(bool active)
    {
        vignette.active = (active || getDamageActive);
        lowHealthActive = active;
    }

    private void UpdateLowHealth()
    {
        timeCount += Time.deltaTime;
        if (timeCount > 1f) timeCount -= 1f;
        vignette.color.value = new Color(1f, lowHealthAnim.Evaluate(timeCount), lowHealthAnim.Evaluate(timeCount)); 
    }
}
