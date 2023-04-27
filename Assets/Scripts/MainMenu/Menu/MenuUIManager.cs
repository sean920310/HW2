using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject PlayMenu;
    [SerializeField] private GameObject OptionsMenu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            PlayMenu.SetActive(false);
            OptionsMenu.SetActive(false);
        }
    }

    public void onPlayBTNClick()
    {
        PlayMenu.SetActive(true);
    }
    public void onSettingsBTNClick()
    {
        OptionsMenu.SetActive(true);
    }
    public void onQuitBTNClick()
    {
        Application.Quit();
    }
}
