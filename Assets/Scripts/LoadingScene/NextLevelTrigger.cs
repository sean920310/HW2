using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] GameObject winCanvas;
    DataPersistenceManager gd;
    // Start is called before the first frame update
    void Start()
    {
        gd = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            //winCanvas.SetActive(true);
            //Time.timeScale = 0;
            if(gd)
                gd.preventSceneChangeOnce = true;
            GetComponent<LoadingScene>().LoadScene(2);
        }
    }
}
