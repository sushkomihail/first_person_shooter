using Character.Avatar;
using Initializator;
using InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mechanics.ObjectsSelection
{
    public class ObjectsSelection : Initializable
    {
        [SerializeField] private PlayerInputSystem _input;
        [SerializeField] private HumanAvatar _avatar;
        [SerializeField] private BoneType _holderBone;
        [SerializeField] private LayerMask _selectionMask;
        [SerializeField] private Camera _camera;
        [SerializeField] private float _selectionDistance;
        
        private Transform _holder;
        private SelectionObject _lastSelectedObject;
        private SelectionObject _inHandObject;

        public override void Initialize()
        {
            _input.Input.PlayerControls.Pick.performed += _ => PerformSelection();
            
            _holder = _avatar.FindConnection(_holderBone).Child;
        }

        private void FixedUpdate()
        {
            CastSelectionRay();
        }

        private void CastSelectionRay()
        {
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            if (Physics.Raycast(ray, out RaycastHit hit, _selectionDistance, _selectionMask.value))
            {
                if (_lastSelectedObject != null) return;
                
                if (hit.transform.TryGetComponent(out SelectionObject selection))
                {
                    if (!selection.IsPickedUp)
                    {
                        _lastSelectedObject = selection;
                        _lastSelectedObject.SetActiveView(true);
                    }
                }
            }
            else if (_lastSelectedObject != null)
            {
                _lastSelectedObject.SetActiveView(false);
                _lastSelectedObject = null;
            }
        }

        private void PerformSelection()
        {
            if (_lastSelectedObject == null && _inHandObject == null) return;
            
            if (_lastSelectedObject == null)
            {
                DropObject();
                return;
            }
            
            if (_inHandObject == null)
            {
                PickUpObject();
            }
            else
            {
                DropObject();
                PickUpObject();
            }
        }
        
        private void PickUpObject()
        {
            if (_holder == null) return;
            
            _avatar.BreakBoneConnection(_holderBone);
            _holder.parent = _camera.transform;
            
            _lastSelectedObject.PickUp(_holder);
            
            _inHandObject = _lastSelectedObject;
            _inHandObject.Initialize(_camera);
            _inHandObject.SetActiveView(false);

            _lastSelectedObject = null;
        }

        private void DropObject()
        {
            if (_holder == null) return;

            _inHandObject.Drop();
            _inHandObject = null;
            
            _avatar.SetBoneConnection(_holderBone);
        }
    }
}