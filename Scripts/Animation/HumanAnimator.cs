using Initializator;
using UnityEngine;

namespace Animation
{
    public class HumanAnimator : Initializable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _stepLength;
        [SerializeField] private Transform _foots;
        
        private int _yHash;
        private int _deathHash;

        private float _defaultAnimationSpeed;
        private float _animationSpeedMultiplier;

        public override void Initialize()
        {
            _yHash = Animator.StringToHash("y");
            _deathHash = Animator.StringToHash("Death");

            _defaultAnimationSpeed = _animator.speed;
        }

        private void SetFootsRotation(Transform humanTransform, Vector2 moveDirection)
        {
            if (moveDirection == Vector2.zero || moveDirection is { y: < 0, x: 0 })
            {
                _foots.localRotation = Quaternion.identity;
                return;
            }
            
            Vector3 projection = humanTransform.GlobalToLocalDirection(moveDirection);
            Vector3 lookVector = moveDirection.y < 0 ? -projection : projection;
            _foots.rotation = Quaternion.LookRotation(lookVector);
        }
        
        public void PlayBodyAnimation(Transform humanTransform, Vector2 moveDirection, float speed)
        {
            SetFootsRotation(humanTransform, moveDirection);

            float animatorValue = moveDirection.y != 0 ? moveDirection.y : moveDirection.x;
            _animator.SetFloat(_yHash, animatorValue);

            if (animatorValue == 0)
            {
                _animator.speed = 1;
            }
            else
            {
                _animationSpeedMultiplier = speed / _stepLength;
                _animator.speed = _defaultAnimationSpeed * _animationSpeedMultiplier;
            }
        }

        private void OnDeath()
        {
            _animator.SetTrigger(_deathHash);
        }
    }
}