using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject playMenu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onPlayBTNClick()
    {
        playMenu.SetActive(true);
    }
    public void onSettingsBTNClick()
    {

    }
    public void onQuitBTNClick()
    {
        Application.Quit();
    }
}
