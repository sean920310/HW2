using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadBehavior : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float force;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            print("jump pad");
            Rigidbody2D rb2D = collision.gameObject.GetComponent<Rigidbody2D>();
            collision.gameObject.GetComponent<Animator>().SetTrigger("isJump");
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            rb2D.AddForce(Vector2.up * force);
        }
    }
}
