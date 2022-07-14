using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    [SerializeField] protected float animation_amp;
    [SerializeField] protected float rotation_speed;
    //[SerializeField] protected GameObject innerObject;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        AnimateConsumable();
    }
    private void AnimateConsumable()
    {
        transform.position = startPosition + new Vector3(0, Mathf.Sin(Time.time) * animation_amp, 0);
        transform.Rotate(new Vector3(0, rotation_speed, 0) * Time.deltaTime);
    }
}
