using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingHandler : MonoBehaviour
{

    protected float rotation = 0.0f;
    protected Vector2 size = new Vector2(1f, 1f);
    protected bool enbiggen = false;
    [SerializeField] protected float rotationSpeed = 3.0f;
    [SerializeField] protected bool isRight = false;
    [SerializeField] protected bool isRotate = false;
    [SerializeField] protected float resizeSpeed = 0.001f;
    [SerializeField] protected bool isResize = false;
    [SerializeField] protected float maxSize = 2.0f;
    [SerializeField] protected float minSize = 0.3f;
    [SerializeField] protected bool isProgressBar = false;
    [SerializeField] protected float progressBarWidth = 800.0f;
    [SerializeField] protected float progressBarSpeed = 200.0f;

    void Rotate()
    {
        if (isRotate)
        {
            rotation += rotationSpeed;
            if (!isRight) transform.localRotation = Quaternion.Euler(0, 0, rotation);
            if (isRight) transform.localRotation = Quaternion.Euler(0, 0, -rotation);
        }
    }

    void Resize()
    {
        if (isResize)
        {
            transform.localScale = size;
        }
    }

    void CalcSize()
    {
        if(enbiggen)
        {
            size.x += resizeSpeed;
            size.y += resizeSpeed;
            if (size.x >= maxSize) enbiggen = false;
        }
        else
        {
            size.x -= resizeSpeed;
            size.y -= resizeSpeed;
            if (size.x <= minSize) enbiggen = true;
        }    
    }

    void CalcProgress()
    {
        size.y = transform.localScale.y;
        if (enbiggen)
        {
            size.x += progressBarSpeed;
            if (size.x >= progressBarWidth) enbiggen = false;
        }
        else
        {
            size.x -= progressBarSpeed;
            if (size.x <= 1) enbiggen = true;
        }
    }

    private void FixedUpdate()
    {
        if (!isProgressBar) CalcSize(); 
        else CalcProgress();
        Rotate();
        Resize();

    }

}
