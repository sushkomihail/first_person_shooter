using System.Collections;
using InputSystem;
using UnityEngine;

namespace Character.Player
{
    public class PlayerController : Human
    {
        [Header("Components")]
        [SerializeField] private PlayerInputSystem _input; 
        [SerializeField] private CharacterController _controller;
        [SerializeField] private Camera _camera;
        
        [Header("Movement")]
        [SerializeField] private int _moveSpeed = 10;
        [SerializeField] private int _strafeSpeed = 7;
        [SerializeField] private int _deceleration = 3;
        
        [Header("Jump")]
        [SerializeField] private int _jumpHeight = 7;
        [SerializeField] private float _jumpTime = 0.9f;
        
        [Header("Dash")]
        [SerializeField] private int _dashDistance = 10;
        [SerializeField] private int _dashSpeed = 35;
        [SerializeField] private float _dashCoolDownTime = 1.5f;
        
        private Vector2 _moveInputVector;
        private Vector3 _moveVector;
        private float _speed;
        private bool _isMoving;
        
        private float _gravity;
        private const float GroundedGravity = -0.05f;
        private float _jumpVelocity;
        private float _previousJumpVelocity;
        private bool _isJumping;
        
        private bool _canDash = true;
        private bool _isDashing;
        
        private Vector2 _lookInputVector;
        private float _xSensitivity = 7.5f; //!
        private float _ySensitivity = 7.5f; //!
        private const int MaxVerticalLookAngle = 90;
        private const int MinVerticalLookAngle = -90;
        private float _xLook;
        private float _yLook;

        public override void Initialize()
        {
            _input.Input.PlayerControls.Dash.performed += _ => PerformDash();
            
            SetUpJumpVariables();
        }

        private void Update()
        {
            OnMove();
            OnJump();
            _controller.Move(_moveVector * Time.deltaTime);
            
            Look();
            
            HumanAnimator.PlayBodyAnimation(transform, _moveInputVector, _speed);
            
            if (_isMoving || _isJumping || _isDashing)
            {
                //PlayerUI.Instance.AllowAnimateCrosshair(true);
            }
            else
            {
                //PlayerUI.Instance.AllowAnimateCrosshair(false);
            }
        }

        private void OnMove()
        {
            Vector2 inputVector = _input.Input.PlayerControls.Move.ReadValue<Vector2>();

            _isMoving = inputVector != Vector2.zero;

            _moveInputVector = inputVector == Vector2.zero
                ? Vector2.MoveTowards(_moveInputVector, inputVector, _deceleration * Time.deltaTime)
                : inputVector;

            _speed = Mathf.Sqrt(Mathf.Pow(_moveInputVector.y * _moveSpeed, 2)
                                     + Mathf.Pow(_moveInputVector.x * _strafeSpeed, 2));
            Vector3 moveVector = transform.GlobalToLocalDirection(_moveInputVector) * _speed;
            _moveVector.x = moveVector.x;
            _moveVector.z = moveVector.z;
            ApplyGravity();
        }
        
        private void ApplyGravity()
        {
            if (_controller.isGrounded)
            {
                _moveVector.y = GroundedGravity;
            }
            else
            {
                float newVelocity = _moveVector.y + _gravity * Time.deltaTime;
                float nextVelocity = (_previousJumpVelocity + newVelocity) * 0.5f;
                _moveVector.y = nextVelocity;
            }
            _previousJumpVelocity = _moveVector.y;
        }

        private void SetUpJumpVariables()
        {
            float timeToApex = _jumpTime / 2;
            _gravity = -2 * _jumpHeight / Mathf.Pow(timeToApex, 2);
            _jumpVelocity = 2 * _jumpHeight / timeToApex;
        }

        private bool IsJumpButtonPressed()
        {
            return _input.Input.PlayerControls.Jump.IsPressed();
        }

        private void OnJump()
        {
            if (!_isJumping && _controller.isGrounded && IsJumpButtonPressed())
            {
                _isJumping = true;
                _moveVector.y = _jumpVelocity * 0.5f;
                _previousJumpVelocity = _moveVector.y;
            }
            else if (_isJumping && _controller.isGrounded && !IsJumpButtonPressed())
            {
                _isJumping = false;
            }
        }

        private IEnumerator ResetDash()
        {
            yield return new WaitForSeconds(_dashCoolDownTime);
            _canDash = true;
        }

        private IEnumerator Dash()
        {
            if (_moveInputVector == Vector2.zero) yield break;
            
            _canDash = false;
            _isDashing = true;

            Vector3 moveVector = _moveVector.normalized;
            moveVector.y = 0;

            float coveredDistance = 0;
            while (coveredDistance < _dashDistance)
            {
                float moveStep = _dashSpeed * Time.deltaTime;
                _controller.Move(moveVector * moveStep);
                coveredDistance += moveStep;
                yield return null;
            }
            _isDashing = false;
            yield return null;
        }

        private void PerformDash()
        {
            if (_canDash)
            {
                StartCoroutine(Dash());
                if (!_canDash) StartCoroutine(ResetDash());
            }
        }
        
        private void Look()
        {
            _lookInputVector = _input.Input.PlayerControls.Look.ReadValue<Vector2>();

            _yLook += _lookInputVector.x * _ySensitivity * Time.deltaTime;
            _yLook = Mathf.Repeat(_yLook, 360);

            _xLook -= _lookInputVector.y * _xSensitivity * Time.deltaTime;
            _xLook = Mathf.Clamp(_xLook, MinVerticalLookAngle, MaxVerticalLookAngle);
            
            transform.rotation = Quaternion.Euler(0, _yLook, 0);

            Vector3 cameraLocalAngles = _camera.transform.localEulerAngles;
            cameraLocalAngles.x = _xLook;
            _camera.transform.localEulerAngles = cameraLocalAngles;
        }

        protected override void OnDeath()
        {
            
        }
    }
}
