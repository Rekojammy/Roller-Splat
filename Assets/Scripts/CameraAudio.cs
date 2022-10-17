using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAudio : MonoBehaviour
{
    private AudioSource generalAudio;

    // Start is called before the first frame update
    void Start()
    {
        generalAudio = GetComponent<AudioSource>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isFinished)
        {
            Debug.Log("Stopped Song");

            generalAudio.Stop();
        }
    }
}
