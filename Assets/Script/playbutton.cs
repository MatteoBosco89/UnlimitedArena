using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playbutton : MonoBehaviour
{
    protected new AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.Play();
    }
    
}
