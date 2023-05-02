using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomUIManager : MonoBehaviour
{
    [SerializeField] Button PlayButton;

    [SerializeField] RectTransform PlayerList;
    [SerializeField] TextMeshProUGUI RoomText;
    [SerializeField] GameObject PlayerElementPrefab;

    private GameObject PlayerElementPrefabTemp;


    void Update()
    {

    }

    public void RoomNameUpdate(string name)
    {
        RoomText.text = name;
    }

    public void isMasterUpdate(bool isMaster)
    {
        PlayButton.interactable = isMaster;
    }

    public void UpdatePlayerList(Dictionary<int, Photon.Realtime.Player> playerList)
    {
        foreach (Transform child in PlayerList)
        {
            Destroy(child.gameObject);
        }


        foreach (KeyValuePair<int, Photon.Realtime.Player> player in playerList)
        {
            PlayerElementPrefabTemp = Instantiate(PlayerElementPrefab);
            PlayerElementPrefabTemp.transform.parent = PlayerList.transform;
            PlayerElementPrefabTemp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = player.Value.NickName.ToString();
            PlayerElementPrefabTemp.SetActive(true);
        }
    }
}
