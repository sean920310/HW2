using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    Transform cam;
    Vector3 camStartPos;
    float distance;

    public GameObject[] backgrounds;
    Material[] mat;
    float[] backSpeed;

    float farthestBack;

    [Range(0.01f, 0.05f)]
    public float movingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backCount = backgrounds.Length;
        mat = new Material[backCount];
        backSpeed = new float[backCount];

        for(int i=0;i<backCount;i++)
        {
            mat[i] = backgrounds[i].GetComponent<SpriteRenderer>().material;
        }
        BackSpeedCal(backCount);
    }

    void BackSpeedCal(int backCount)
    {
        for(int i=0;i<backCount;i++)
        {
            if(backgrounds[i].transform.position.z - cam.position.z > farthestBack)
            {
                farthestBack = backgrounds[i].transform.position.z - cam.position.z;
            }
        }
        for (int i = 0; i < backCount; i++)
        {
            backSpeed[i] = 1- ((backgrounds[i].transform.position.z - cam.position.z) / farthestBack) + 0.1f;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        distance = cam.position.x - camStartPos.x;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backSpeed[i] * movingSpeed;
            mat[i].SetVector("_TexOffset", new Vector2(distance, 0) * speed);
        }
    }
}
