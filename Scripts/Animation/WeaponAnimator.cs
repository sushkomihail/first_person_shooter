using UnityEngine;

namespace Animation
{
    public class WeaponAnimator : MonoBehaviour
    {
        [Header("Sway")]
        [SerializeField] private float _swayValue;
        [SerializeField] private Vector2 _swayRange;
        [SerializeField] private float _swaySpeed;
        
        [Header("Bob")]
        [SerializeField] private Vector2 _bobRange;
        [SerializeField] private Vector3 _bobTravelRange;
        [SerializeField] private Vector3 _bobRotationRange;
        [SerializeField] private float _bobSpeed;

        [Header("Recoil")]
        [SerializeField] private float _recoilTravel;
        [SerializeField] private float _recoilSpeed;

        private Transform _handler;
        private Vector3 _defaultHandlerPosition;
        
        private Vector3 _swayAngles;
        
        private float _bobFunctionArgument;
        private Vector3 _bobPosition;
        private Vector3 _bobAngles;

        private Vector3 _recoilPosition;

        public void SetHandler(Transform handler)
        {
            _handler = handler;
            _defaultHandlerPosition = _handler.localPosition;
        }

        private void AnimateSway(Vector2 lookInputVector)
        {
            float xSway = -_swayValue * lookInputVector.y;
            xSway = Mathf.Clamp(xSway, -_swayRange.x, _swayRange.x);

            float ySway = _swayValue * lookInputVector.x;
            ySway = Mathf.Clamp(ySway, -_swayRange.y, _swayRange.y);

            _swayAngles = new Vector3(xSway, ySway, ySway);
        }

        private void AnimateBob(Vector3 defaultBobAngles, Vector2 inputVector, float speed)
        {
            _bobFunctionArgument += speed * Time.deltaTime;
            _bobFunctionArgument = Mathf.Repeat(_bobFunctionArgument, 2 * Mathf.PI);
            
            _bobPosition = _defaultHandlerPosition;
            _bobAngles = defaultBobAngles;

            if (inputVector != Vector2.zero)
            {
                _bobPosition.x += Mathf.Cos(_bobFunctionArgument) * _bobRange.x - inputVector.x * _bobTravelRange.x;
                _bobPosition.y += Mathf.Sin(_bobFunctionArgument * 2) * _bobRange.y;
                _bobPosition.z -= inputVector.y * _bobTravelRange.z;

                _bobAngles.x += Mathf.Sin(_bobFunctionArgument * 2) * _bobRotationRange.x;
                _bobAngles.y += Mathf.Cos(_bobFunctionArgument) * _bobRotationRange.y;
                _bobAngles.z += Mathf.Cos(_bobFunctionArgument) * _bobRotationRange.z * inputVector.x;
            }
        }

        private void AnimateRecoil()
        {
            
        }

        private void JoinWeaponHandlerAnimations()
        {
            _handler.localPosition =
                Vector3.Lerp(_handler.localPosition, _bobPosition, _bobSpeed * Time.deltaTime);

            _handler.localRotation = Quaternion.Slerp(_handler.localRotation,
                Quaternion.Euler(_swayAngles) * Quaternion.Euler(_bobAngles), _swaySpeed * Time.deltaTime);
        }

        public void Animate(Vector3 defaultBobAngles, Vector2 moveInputVector, Vector2 lookInputVector, float speed)
        {
            AnimateSway(lookInputVector);
            AnimateBob(defaultBobAngles, moveInputVector, speed);
            JoinWeaponHandlerAnimations();
        }
    }
}