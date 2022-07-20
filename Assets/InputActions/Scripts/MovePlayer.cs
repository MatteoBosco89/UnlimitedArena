using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Character
{
    public class MovePlayer : NetworkBehaviour
    {
        [SerializeField] protected float _baseSpeed = 15.0f;
        [SerializeField] protected float _baseJump = 5.0f;
        [SerializeField] protected float _maxJump = 10.0f;
        [SerializeField] protected float _gravity = 9.81f;
        [SerializeField] protected Camera _camera;
        [SerializeField] protected float _weight = 3.0f;
        [SerializeField] protected float _runSpeedBuff = 2.0f;
        [SerializeField] protected GameObject _powerupManager;
        [SerializeField] protected float _jumpCooldown = 0.2f;
        protected PowerUpManager powerUp;
        protected CharacterController _charController;
        protected Animator animator;
        protected CharacterStatus status;
        protected SpeedCalc sc;
        protected PlayerManagerScript pms;
        protected float _speed;
        private float groundedTimer;
        private float verticalVelocity;
        CursorLockMode lockMode;
        protected float _animSpeed;

        void Awake()
        {
            lockMode = CursorLockMode.Locked;
            Cursor.lockState = lockMode;
            _charController = GetComponent<CharacterController>();
            status = GetComponent<CharacterStatus>();
            sc = GetComponent<SpeedCalc>();
            pms = GetComponent<PlayerManagerScript>();
            powerUp = _powerupManager.GetComponent<PowerUpManager>();
        }

        private void FixedUpdate()
        {
            if (isLocalPlayer)
            {
                Vector3 movement = new Vector3(0, 0, 0);

                verticalVelocity = VerticalVelocityCalc();

                if (status.IsRunning) sc.AddBuff("run", _runSpeedBuff);
                else if (!status.IsRunning) sc.RemoveBuff("run");

                if (powerUp.GetTimeRemaining("Speed") > 0) sc.AddBuff("SpeedPowerUp", powerUp.GetAura("Speed"));
                else sc.RemoveBuff("Speed");

                if (status.IsRotating)
                {
                    RotateChar(status.Rotation.x, status.Rotation.y);
                }

                _speed = sc.CalcSpeed(_baseSpeed);

                if (status.IsMoving)
                {
                    movement.z = status.Movement.z * _speed * Time.deltaTime;
                    movement.x = status.Movement.x * _speed * Time.deltaTime;
                }

                if (status.IsJumping && _charController.isGrounded)
                {
                    if (groundedTimer <= 0)
                    {
                        groundedTimer = 0;
                        if(verticalVelocity < _maxJump) verticalVelocity += Mathf.Sqrt(_baseJump * 2 * _gravity);
                    }
                }

                movement.y = verticalVelocity;
                AnimatorSpeed(movement);
                MoveChar(movement);
            }
            
        }

        private void AnimatorSpeed(Vector3 movement)
        {
            animator = pms.GetComponent<Animator>();
            _ = movement.z > movement.x ? _animSpeed = movement.z : _animSpeed = movement.x;
            animator.SetFloat("speed", _animSpeed);
        }

        private void MoveChar(Vector3 movement)
        { 
            movement = transform.TransformDirection(movement);
            _charController.Move(movement * Time.deltaTime);
        }

        private float VerticalVelocityCalc()
        {
            if (!_charController.isGrounded) groundedTimer = _jumpCooldown;
            if (groundedTimer > 0) groundedTimer -= Time.deltaTime;
            if (_charController.isGrounded && verticalVelocity < 0) verticalVelocity = 0f;
            verticalVelocity -= _gravity * _weight * Time.deltaTime;
            return verticalVelocity;
        }

        private void RotateChar(float x, float y)
        {
            transform.localRotation = Quaternion.Euler(0, x, 0);
            _camera.transform.localRotation = Quaternion.Euler(-y, 0, 0);
        }

    }

}


