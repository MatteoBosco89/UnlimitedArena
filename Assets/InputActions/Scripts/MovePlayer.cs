using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class MovePlayer : MonoBehaviour
    {
        [SerializeField] protected float _baseSpeed = 15.0f;
        [SerializeField] protected float _baseJump = 5.0f;
        [SerializeField] protected float _gravity = 9.81f;
        [SerializeField] protected Camera _camera;
        [SerializeField] protected float _weight = 3.0f;
        [SerializeField] protected float _runSpeedBuff = 2.0f;
        protected CharacterController _charController;
        protected Animator animator;
        protected CharacterStatus status;
        protected SpeedCalc sc;
        protected JumpCalc jc;
        protected PlayerManagerScript pms;
        protected float _speed;
        protected float _jump = 5.0f;
        private float groundedTimer;
        private float verticalVelocity;
        CursorLockMode lockMode;
        protected float _animSpeed;
        protected GameObject _spawnedChar;

        void Awake()
        {
            lockMode = CursorLockMode.Locked;
            Cursor.lockState = lockMode;
            _charController = GetComponent<CharacterController>();
            status = GetComponent<CharacterStatus>();
            sc = GetComponent<SpeedCalc>();
            jc = GetComponent<JumpCalc>();
            pms = GetComponent<PlayerManagerScript>();
        }

        private void FixedUpdate()
        {

            _spawnedChar = pms.SpawnedChar;

            Vector3 movement = new Vector3(0, 0, 0);

            verticalVelocity = VerticalVelocityCalc();

            if (status.IsRunning) sc.AddBuff("run", _runSpeedBuff);
            else if (!status.IsRunning) sc.RemoveBuff("run");

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
                if (groundedTimer > 0)
                {
                    groundedTimer = 0;
                    verticalVelocity += Mathf.Sqrt(_jump * 2 * _gravity);
                }
            }

            movement.y = verticalVelocity;
            AnimatorSpeed(movement);
            MoveChar(movement);
        }

        private void AnimatorSpeed(Vector3 movement)
        {
            if (_spawnedChar == null) return;
            animator = _spawnedChar.GetComponent<Animator>();
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
            if (_charController.isGrounded) groundedTimer = 0.2f;
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


