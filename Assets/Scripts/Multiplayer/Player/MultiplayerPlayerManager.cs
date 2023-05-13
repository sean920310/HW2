using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerPlayerManager : MonoBehaviour
{

    [Header("Cam")]
    [SerializeField] GameObject _cmCam;
    [SerializeField] GameObject _miniMapCam;

    [Header("Weapon")]
    [SerializeField] GameObject _weapon;

    private PhotonView _pv;
    private PlayerStatesManager _psm;
    private bool playerAttack = false;

    private void Start()
    {
        _psm = GetComponent<PlayerStatesManager>();
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
        if(_psm.IsAttacking && !playerAttack)
        {
            playerAttack = true;
            CallRpcPlayerAttackStart();
        }
        if(playerAttack && !_psm.IsAttacking)
        {
            playerAttack = false;
            CallRpcPlayerAttackEnd();
        }

    }

    #region PunRPC

    public void CallRpcPlayerAttackStart()
    {
        _pv.RPC("RpcPlayerAttackStart", RpcTarget.All, _pv.Owner);
    }

    [PunRPC]
    void RpcPlayerAttackStart(Player player, PhotonMessageInfo info)
    {
        if(player == _pv.Owner)
            _weapon.SetActive(true);
    }


    public void CallRpcPlayerAttackEnd()
    {
        _pv.RPC("RpcPlayerAttackEnd", RpcTarget.All, _pv.Owner);
    }

    [PunRPC]
    void RpcPlayerAttackEnd(Player player, PhotonMessageInfo info)
    {
        if (player == _pv.Owner)
            _weapon.SetActive(false);
    }

    #endregion
}
