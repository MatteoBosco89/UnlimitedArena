using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    [SerializeField] protected AudioClip onPickupSound;

    public AudioClip Clip
    {
        get { return onPickupSound; }
    }
}
