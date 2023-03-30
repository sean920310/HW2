using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] PlayerStatesManager psm;

    [SerializeField] int maxHealth;
    [ReadOnly]
    [SerializeField] int health;

    public int MaxHealth { get => maxHealth;}
    public int Health { get => health;}

    private void Start()
    {
        health = MaxHealth;
    }

    private void Update()
    {

    }

    public void damage(int damage)
    {
        psm.SwitchState(psm.Factory.Hurt());
        health -= damage;
    }
}
