using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] PlayerStatesManager psm;

    [SerializeField] float blockingRatio = 0.5f;

    [SerializeField] int _maxHealth;
    [ReadOnly]
    [SerializeField] int _health;

    [SerializeField] int _money;

    public int MaxHealth { get => _maxHealth;}
    public int Health { get => _health; }
    public int Money { get => _money; }

    private void Start()
    {
        _health = MaxHealth;
    }

    private void Update()
    {

    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
        this._money = data.coinCount;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = this.transform.position;
        data.coinCount = this._money;
    }

    public void GetDamage(int damage)
    {
        psm.SwitchState(psm.Factory.Hurt());

        if (psm.CurrentState.ToString() == "PlayerBlockingState")
            _health -= (int)((float)damage * blockingRatio);
        else
            _health -= damage;
    }
    public void AddHealth(int val)
    {
        _health += Mathf.Abs( val);
    }
    public void AddMoney(int val)
    {
        _money += Mathf.Abs(val);
    }
}
