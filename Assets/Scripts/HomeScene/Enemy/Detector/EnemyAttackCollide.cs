using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCollide : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    float _damage;
    public void SetDamage(float damage) => _damage = damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            Debug.Log("Player Hurt: " + collision.gameObject.name);
            collision.gameObject.GetComponent<PlayerManager>().GetDamage((int)_damage);
            this.enabled = false;
        }
    }
}
