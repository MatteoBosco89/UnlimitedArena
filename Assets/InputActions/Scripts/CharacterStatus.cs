using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

namespace Character
{
    public class CharacterStatus : NetworkBehaviour // Manager del Character Controller
    {

        [SerializeField] protected float walkSpeed = 15.0f;
        [SerializeField] protected float rotationSensitivity = 0.2f;
        [SerializeField] protected float cameraClamp = 25.0f;
        protected bool isMoving; 
        protected bool isRotating; 
        protected bool isRunning; 
        protected bool isJumping;
        protected bool isFiring;
        protected bool isChangingWeaponPre;
        protected bool isChangingWeaponNext;
        protected Vector3 movement; 
        protected Vector3 rotation; 

        public bool IsChangingWeaponsPre
        {
            get { return isChangingWeaponPre; }
        }

        public bool IsChangingWeaponsNext
        {
            get { return isChangingWeaponNext; }
        }

        public bool IsMoving
        {
            get { return isMoving; }
        }

        public bool IsJumping
        {
            get { return isJumping; }
        }
        public bool IsFiring
        {
            get { return isFiring; }
        }
        public bool IsRunning
        {
            get { return isRunning; }
        }

        public bool IsRotating
        {
            get { return isRotating; }
        }
        public Vector3 Movement
        {
            get { return movement; }
        }
        public Vector3 Rotation
        {
            get { return rotation; }
        }
        void Update()
        {
            isMoving = (movement.z != 0 || movement.x != 0);
            isRotating = (rotation.y != 0 || rotation.x != 0);
        }
        public void OnMove(InputAction.CallbackContext value)
        {
            Vector2 inputs = value.ReadValue<Vector2>();
            movement.z = inputs.y * walkSpeed;
            movement.x = inputs.x * walkSpeed;
        }

        public void OnRun(InputAction.CallbackContext value)
        {
            if (value.started) isRunning = true;
            else if (value.canceled) isRunning = false;
        }

        public void OnLook(InputAction.CallbackContext value)
        {
            Vector3 inputs = value.ReadValue<Vector2>();
            rotation.x += inputs.x * rotationSensitivity;
            rotation.y += inputs.y * rotationSensitivity;
            rotation.y = Mathf.Clamp(rotation.y, -cameraClamp, cameraClamp);
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (value.started) isJumping = true;
            else if (value.canceled) isJumping = false;
        }

        public void OnFire(InputAction.CallbackContext value)
        {
            if (value.started) isFiring = true;
            else if (value.canceled) isFiring = false;
        }
        public void OnChangeWeaponPre(InputAction.CallbackContext value)
        {
            if (value.started) isChangingWeaponPre = true;
            else if (value.canceled) isChangingWeaponPre = false;
        }

        public void OnChangeWeaponNext(InputAction.CallbackContext value)
        {
            if (value.started) isChangingWeaponNext = true;
            else if (value.canceled) isChangingWeaponNext = false;
        }


    }
}
