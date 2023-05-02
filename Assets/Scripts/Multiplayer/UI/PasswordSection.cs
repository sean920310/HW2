using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PasswordSection : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI information;
    public GameObject roomElement;
    public string inputPWD { set; get; }

    public void Update()
    {
        if(roomElement == null) 
            Destroy(gameObject);
    }

    public void onJoinBTNClick()
    {

        bool isPwdCorrect = roomElement.GetComponent<RoomElement>().pwdJoinButtonClick(inputPWD);
        if(!isPwdCorrect)
        {
            information.text = "Password incorrect";
        }
        else
        {
            onLeaveBTNClick();
        }
    }

    public void onLeaveBTNClick()
    {
        roomElement.GetComponent<RoomElement>().pwdLeaveButtonClick();
    }
}
