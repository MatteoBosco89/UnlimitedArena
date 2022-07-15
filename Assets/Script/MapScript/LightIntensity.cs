using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensity : MonoBehaviour
{
    [SerializeField] protected int _initialIntensity = 1;
    [SerializeField] protected int _finalIntensity = 4;
    [SerializeField] protected bool _isRotating = false;
    [SerializeField] protected float _rotationAngle = 300.0f;
    [SerializeField] protected float _rotationRate = 1.0f;
    [SerializeField] protected float _initialRotation = 100.0f;
    protected Light _thisLight;
    protected bool intensityUp = true;
    protected bool rotateForward = true;
    protected float rotate;

    private void Awake()
    {
        _thisLight = GetComponent<Light>();
        _thisLight.intensity = _initialIntensity;
        _initialRotation = 0;
        rotate = _initialRotation;
        _rotationAngle = Quaternion.Euler(_rotationAngle, 0, 0).x;
        if(_isRotating) _thisLight.transform.localRotation = Quaternion.Euler(_initialRotation, 0, 0);
    }

    private void FixedUpdate()
    {
        ChangeLight();
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
                if (_thisLight.transform.localRotation.x >= _initialRotation + _rotationAngle) rotateForward = false;
            }
            else if (!rotateForward)
            {
                rotate -= _rotationRate;
                _thisLight.transform.localRotation = Quaternion.Euler(rotate, 0, 0);
                if (_thisLight.transform.localRotation.x <= _initialRotation) rotateForward = true;
            }
        }

    }

}
