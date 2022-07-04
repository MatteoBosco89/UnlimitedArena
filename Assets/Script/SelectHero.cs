using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHero : MonoBehaviour
{

    [SerializeField] protected float rotationSpeed = -1.0f;
    [SerializeField] protected GameObject[] characthers;
    private int currentChar;
    

    public void Start()
    {
        currentChar = 0;
        characthers[currentChar].SetActive(true);
    }

    public void right()
    {
        characthers[currentChar].SetActive(false);
        currentChar++;
        if (currentChar >= characthers.Length) currentChar = 0;
        characthers[currentChar].SetActive(true);
        characthers[currentChar].transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
    }

    public void left()
    {
        characthers[currentChar].SetActive(false);
        currentChar--;
        if (currentChar < 0) currentChar = characthers.Length - 1;
        characthers[currentChar].SetActive(true);
        characthers[currentChar].transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
    }

    void FixedUpdate()
    {
        characthers[currentChar].transform.Rotate(0, rotationSpeed, 0);
    }
}
