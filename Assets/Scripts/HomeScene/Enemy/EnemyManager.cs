using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyStateManager esm;

    [SerializeField] private int _maxHealth;
    [ReadOnly]
    [SerializeField] private int _health;

    public int MaxHealth { get => _maxHealth; }
    public int Health { get => _health; }

    // Start is called before the first frame update
    void Start()
    {
        _health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDamage(int damage)
    {
        //esm.SwitchState(psm.Factory.Hurt());
        _health -= damage;
    }
}
