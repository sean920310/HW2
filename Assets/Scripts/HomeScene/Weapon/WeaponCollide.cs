using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollide : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((enemyLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            if(collision.gameObject.transform.position.x > transform.parent.transform.position.x) // enemy in right side
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 100f, ForceMode2D.Impulse);
            }
            else
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -100f, ForceMode2D.Impulse);
            }

            Debug.Log("Enemy Hurt: " + collision.gameObject.name);
            this.enabled = false;
        }
        
    }
}
