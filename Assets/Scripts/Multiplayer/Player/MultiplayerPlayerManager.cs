using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MultiplayerPlayerManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] MultiPlayerStatesManager psm;
    [SerializeField] GameObject lose;

    [SerializeField] float blockingRatio = 0.5f;

    [SerializeField] int _maxHealth;
    [SerializeField] int _lowHealth;

    [Header("Cam")]
    [SerializeField] GameObject _cmCam;
    [SerializeField] GameObject _miniMapCam;

    [ReadOnly]
    [SerializeField] int _health;

    [SerializeField] int _money;

    private PhotonView _pv;
    public int MaxHealth { get => _maxHealth;}
    public int Health { get => _health; }
    public int Money { get => _money; }

    private void Start()
    {
        _health = MaxHealth;
        _pv = GetComponent<PhotonView>();

        if(_pv.IsMine)
        {
            _cmCam.SetActive(true);
            _miniMapCam.SetActive(true);
            _cmCam.transform.SetParent(transform.parent);
            _miniMapCam.transform.SetParent(transform.parent);
        }
        else
        {
            Rigidbody2D _rb = GetComponent<Rigidbody2D>();
            _rb.isKinematic = true;
        }
    }

    private void Update()
    {
        //GetComponent<PlayerInput>() = GameObject.Find("A").GetComponent<PlayerInput>();
        DamageEffect.LowHealth(_health <= _lowHealth);

        if(this.transform.position.y < -20 || _health <= 0)
        {
            //dead
            lose.SetActive(true);
            Time.timeScale = 0;
        }
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
        if (psm.CurrentState.ToString() == "PlayerBlockingState")
            _health -= (int)((float)damage * blockingRatio);
        else
            _health -= damage;
        psm.SwitchState(psm.Factory.Hurt());
        StartCoroutine(DamageEffect.GetDamage());
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
