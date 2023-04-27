using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject BackPackPanel;
    [SerializeField] GameObject GameSettingsPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (BackPackPanel.active)
            {
                BackPackPanel.SetActive(false);
            }
            else
            {
                if (GameSettingsPanel.active)
                    GameSettingsPanel.SetActive(false);
                else
                    GameSettingsPanel.SetActive(true);
            }
        }
    }
}
