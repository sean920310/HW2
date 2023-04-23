using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    public Transform playeTF;
    public float moveSpeed = 17f;

    public CoinMove coinMove;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "CoinMagnet")
        {
            coinMove.enabled = true;
        }
    }
}
