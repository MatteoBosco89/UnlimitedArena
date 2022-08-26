using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

namespace Character
{
    public class CharacterStatus : NetworkBehaviour 
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
        protected bool isAlive = true;
        protected bool activate;
        protected bool activatePre;
        protected bool activateNext;
        protected bool scoreTable;
        protected bool isPaused = false;
        protected Vector3 movement; 
        protected Vector3 rotation;
        protected PlayerLifeManager lifeManager;

        public bool IsPaused
        {
            get { return isPaused; }
            set { isPaused = value; }
        }
        public bool ScoreTable
        {
            get { return scoreTable; }
        }

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        public bool IsChangingWeaponsPre
        {
            get { return isChangingWeaponPre; }
        }

        public bool IsChangingWeaponsNext
        {
            get { return isChangingWeaponNext; }
        }

        public bool Activate
        {
            get { return activate; }
        }

        public bool ActivatePre
        {
            get { return activatePre; }
        }

        public bool ActivateNext
        {
            get { return activateNext; }
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

        public void OnScoreTable(InputAction.CallbackContext value)
        {
            if (value.started) scoreTable = true;
            else if (value.canceled) scoreTable = false;
        }

        public void OnActivate(InputAction.CallbackContext value)
        {
            if (value.started) activate = true;
            else if (value.canceled) activate = false;
        }

        public void OnActivatePre(InputAction.CallbackContext value)
        {
            if (value.started) activatePre = true;
            else if (value.canceled) activatePre = false;
        }

        public void OnActivateNext(InputAction.CallbackContext value)
        {
            if (value.started) activateNext = true;
            else if (value.canceled) activateNext = false;
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

        public void OnPause(InputAction.CallbackContext value)
        {
            if (isPaused) isPaused = false;
            else isPaused = true;
        }
    }
}
