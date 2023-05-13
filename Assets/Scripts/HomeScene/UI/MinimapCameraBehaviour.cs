using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinimapCameraBehaviour : MonoBehaviour
{
    [SerializeField] private Shader minimapShader;
    [SerializeField] private Transform player;

    void Start()
    {
        GetComponent<Camera>().SetReplacementShader(minimapShader, "");
    }

    // Update is called once per frame
    void Update()
    {
        if(player!=null)
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
