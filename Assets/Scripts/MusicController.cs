using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;

    public void OnClick()
    {
        audioSource.Play();
    }
}
