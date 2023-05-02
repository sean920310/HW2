using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomElement : MonoBehaviour
{
    [SerializeField] CreateRoom createRoom;

    [SerializeField] GameObject passwordFieldPrefab;
    private GameObject tempPasswordField;

    private string roomPWD = "";

    public void RoomSelected()
    {
        roomPWD = transform.GetChild(3).GetComponent<TextMeshProUGUI>().text;

        if (roomPWD.Length > 0) // require password 
        {
            // show password field
            tempPasswordField = GameObject.Instantiate(passwordFieldPrefab);
            tempPasswordField.GetComponent<PasswordSection>().roomElement = gameObject;

            tempPasswordField.transform.SetParent(gameObject.transform.root, false);
            tempPasswordField.transform.SetAsLastSibling();
        }
        else
        {
            createRoom.updateJoinRoomName(transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
        }

    }
    private void OnDestroy()
    {
        if(tempPasswordField != null)
            Destroy(tempPasswordField); 
    }

    public void pwdLeaveButtonClick()
    {
        Destroy(tempPasswordField);
    }

    public bool pwdJoinButtonClick(string pwd)
    {
        if(pwd == roomPWD) // passwrod correct
        {
            createRoom.updateJoinRoomName(transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            createRoom.OnClickJoinRoom();
            return true;
        }
        createRoom.updateJoinRoomName("");
        return false;
    }
}
