using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class RoomManager : MonoBehaviourPunCallbacks
{
    private Dictionary<int, Player> playerList;

    [SerializeField] RoomUIManager roomUIManager;

    private void Start()
    {
    }

    void Update()
    {
        if(PhotonNetwork.InRoom)
        {
            roomUIManager.RoomNameUpdate(PhotonNetwork.CurrentRoom.Name);
            roomUIManager.isMasterUpdate(PhotonNetwork.IsMasterClient);
        }
    }

    void UpdatePlayerList()
    {
        if (!PhotonNetwork.IsConnected) return;

        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null) return;

        roomUIManager.UpdatePlayerList(PhotonNetwork.CurrentRoom.Players);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        UpdatePlayerList();
    }

    // This is for remote player
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdatePlayerList();
    }

    // This is for remote player
    public override void OnPlayerLeftRoom(Player oldPlayer)
    {
        base.OnPlayerLeftRoom(oldPlayer);
        UpdatePlayerList();
    }

    // This is for local player
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        UpdatePlayerList();
    }

    // This is for local player
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        UpdatePlayerList();
    }

    public void LeaveButtonClick()
    {
        PhotonNetwork.LeaveRoom(true);
    }

    public void PlayButtonClick()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }
    }
}
