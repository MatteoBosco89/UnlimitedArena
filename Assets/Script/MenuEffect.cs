using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEffect : MonoBehaviour
{
    [SerializeField] protected float speed = 3.0f;
    [SerializeField] protected float translationSpeed = 0.05f;
    [SerializeField] protected GameObject objectToRotate;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        objectToRotate.transform.Rotate(speed, speed, speed);
        objectToRotate.transform.Translate(translationSpeed, 0, 0, Space.Self);
    }
}
