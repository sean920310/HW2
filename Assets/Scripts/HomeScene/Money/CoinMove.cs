using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMove : MonoBehaviour
{
    public int CoinValue = 1;
    public coin coinScript;
    bool used = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, coinScript.playeTF.position, coinScript.moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CoinCollector")
        {
            StartCoroutine(playSound());
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }
    IEnumerator playSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        while (audio.isPlaying)
        {
            yield return new WaitForSeconds(audio.clip.length);
        }

        if(!used)
        {
            coinScript.playeTF.gameObject.GetComponent<PlayerManager>().AddMoney(CoinValue);
            used = true;
        }

        Destroy(gameObject);
    }
}

