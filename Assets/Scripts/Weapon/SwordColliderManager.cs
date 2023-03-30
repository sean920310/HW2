using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordColliderManager : MonoBehaviour
{
    [SerializeField] GameObject attack1Collider;
    [SerializeField] GameObject attack2Collider;
    [SerializeField] GameObject attack3Collider;


    public void attack1Start() => attack1Collider.SetActive(true);
    public void attack2Start() => attack2Collider.SetActive(true);
    public void attack3Start() => attack3Collider.SetActive(true);
    public void attack1End() => attack1Collider.SetActive(false);
    public void attack2End() => attack2Collider.SetActive(false);
    public void attack3End() => attack3Collider.SetActive(false);
}
