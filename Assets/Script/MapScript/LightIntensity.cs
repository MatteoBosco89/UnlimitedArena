using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensity : MonoBehaviour
{
    [SerializeField] protected float _duration = 0.1f;
    [SerializeField] protected int _initialIntensity = 1;
    [SerializeField] protected int _finalIntensity = 4;
    [SerializeField] protected bool _isRotating = false;
    [SerializeField] protected float _rotationAngle = 30.0f;
    [SerializeField] protected float _rotationRate = 1.0f;
    protected Light _thisLight;
    protected bool intensityUp = true;
    protected bool rotateForward = true;
    protected float initialRotation;
    protected float rotate;

    private void Awake()
    {
        _thisLight = GetComponent<Light>();
        _thisLight.intensity = _initialIntensity;
        initialRotation = _thisLight.transform.localRotation.x;
        rotate = initialRotation;
        InvokeRepeating("ChangeLight", 0.0f, _duration);
    }

    protected void ChangeLight()
    {
        if (intensityUp)
        {
            _thisLight.intensity += 0.1f;
            if (_thisLight.intensity >= _finalIntensity) intensityUp = false;
        }
        else if(!intensityUp)
        {
            _thisLight.intensity -= 0.1f;
            if (_thisLight.intensity <= _initialIntensity) intensityUp = true;
        }

        if (_isRotating)
        {
            if (rotateForward)
            {
                rotate += _rotationRate;
                _thisLight.transform.localRotation = Quaternion.Euler(rotate, 0, 0);
                if (_thisLight.transform.rotation.x >= initialRotation + _rotationAngle) rotateForward = false;
            }
            else if (!rotateForward)
            {
                rotate -= _rotationRate;
                _thisLight.transform.localRotation = Quaternion.Euler(rotate, 0, 0);
                if (_thisLight.transform.rotation.x <= initialRotation) rotateForward = true;
            }

        }

    }

}
