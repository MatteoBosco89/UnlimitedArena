using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class ShootingScript : MonoBehaviour
    {
        [SerializeField] protected float dmg = 1.0f;
        [SerializeField] protected float range = 100.0f;
        [SerializeField] protected Camera _camera;
        protected CharacterStatus status;

        private void Awake()
        {
            status = GetComponent<CharacterStatus>();
        }

        
        void FixedUpdate()
        {
            if (status.IsFiring)
            {
                Shoot();
            }
        }

        void Shoot()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hitInfo, range))
                Debug.Log(hitInfo.transform.name);
        }

        
    }
}

