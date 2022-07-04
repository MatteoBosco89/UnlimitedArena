using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEffect : MonoBehaviour
{
    [SerializeField] protected float speed = 0.05f;
    float rot = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rot += speed;
        if(rot > 360) rot = 0; 
        rot %= 360;
        RenderSettings.skybox.SetFloat("_Rotation", rot);
    }
}
