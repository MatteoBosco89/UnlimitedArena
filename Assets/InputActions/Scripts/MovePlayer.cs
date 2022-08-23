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
        [SerializeField] protected float _jumpCooldown = 0.2f;
        [SerializeField] protected string SPEED = "MOVEMENT_SPEED";
        [SerializeField] protected string JUMP = "JUMP_HEIGHT";
        protected float _currentSpeed = 0;
        protected CharacterController _charController;
        protected Animator animator;
        protected CharacterStatus status;
        protected PlayerManagerScript pms;
        protected float _speed;
        protected float _jump;
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
            pms = GetComponent<PlayerManagerScript>();
        }

        private void FixedUpdate()
        {
            if (isLocalPlayer && status.IsAlive)
            {
                Vector3 movement = new Vector3(0, 0, 0);

                verticalVelocity = VerticalVelocityCalc();
                _currentSpeed = _baseSpeed;

                if (status.IsRunning) _currentSpeed *= _runSpeedBuff;

                if (status.IsRotating)
                {
                    RotateChar(status.Rotation.x, status.Rotation.y);
                }

                _speed = _currentSpeed * ComputateFeature(SPEED);

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
                        _jump = _baseJump * ComputateFeature(JUMP);
                        if (verticalVelocity < _maxJump) verticalVelocity += Mathf.Sqrt(_jump * 2 * _gravity);
                    }
                }

                movement.y = verticalVelocity;
                AnimatorSpeed(movement);
                MoveChar(movement);
            }
            
        }

        protected float ComputateFeature(string feature)
        {
            return pms.FeatureValue(feature);
        }

        private void AnimatorSpeed(Vector3 movement)
        {
            animator = pms.GetComponent<Animator>();
            _animSpeed = movement.z;
            animator.SetFloat("speed", _animSpeed);
        }

        public void TeleportToRespawn(Vector3 respawnPos)
        {
            _charController.enabled = false;
            transform.position = respawnPos;
            _charController.enabled = true;
            CmdRespawn(transform.position);
        }

        [Command]
        public void CmdRespawn(Vector3 respawnPos)
        {
            RpcRespawn(respawnPos);
        }

        [ClientRpc]
        protected void RpcRespawn(Vector3 respawnPos)
        {
            if(isLocalPlayer)
            {
                _charController.enabled = false;
                transform.position = respawnPos;
                _charController.enabled = true;
            }
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


