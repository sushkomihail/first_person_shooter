using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Character.Avatar
{
    [Serializable]
    public class AvatarConnection
    {
        [SerializeField] private BoneType _boneType;
        [SerializeField] private Transform _parent;
        [SerializeField] private Transform _child;
        
        private Vector3 _connectPosition;
        private Quaternion _connectRotation;
        private bool _isConnectParametersSettled;
        
        private Vector3 _breakPosition;
        private Quaternion _breakRotation;

        public BoneType BoneType => _boneType;
        public Transform Child => _child;

        public void SetParameters(AvatarActionType type)
        {
            switch (type)
            {
                case AvatarActionType.Connect:
                    _connectPosition = _child.localPosition;
                    _connectRotation = _child.localRotation;
                    _isConnectParametersSettled = true;
                    break;
                case AvatarActionType.Break:
                    _breakPosition = _child.localPosition;
                    _breakRotation = _child.localRotation;
                    break;
            }
        }

        public void Connect()
        {
            _child.parent = _parent;

            if (_isConnectParametersSettled)
            {
                _child.localPosition = _connectPosition;
                _child.localRotation = _connectRotation;
            }
        }

        public void Break(Transform _breakParent)
        {
            _child.parent = _breakParent;
            _child.localPosition = _breakPosition;
            _child.localRotation = _breakRotation;
        }
    }
}