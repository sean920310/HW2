using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource attackSound1;
    [SerializeField] AudioSource attackSound2;
    [SerializeField] AudioSource attackSound3;

    public void attack1Play()
    {
        attackSound1.Play();
    }
    public void attack2Play()
    {
        attackSound2.Play();
    }
    public void attack3Play()
    {
        attackSound3.Play();
    }
}
