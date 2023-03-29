using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onStartBTNClick()
    {
        SceneManager.LoadScene(1);
    }
    public void onSettingsBTNClick()
    {

    }
    public void onQuitBTNClick()
    {
        Application.Quit();
    }
}
