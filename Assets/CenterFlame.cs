using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFlame : MonoBehaviour
{
    AudioSource myAudio; // 오타 수정: AudioSoure -> AudioSource
    bool musicStart = false;

    private void Start()
    {
        myAudio = GetComponent<AudioSource>(); // 오타 수정
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!musicStart)
        {
            if (collision.CompareTag("Note"))
            {
                myAudio.Play();
                musicStart = true;
            }
        }
    }
}
