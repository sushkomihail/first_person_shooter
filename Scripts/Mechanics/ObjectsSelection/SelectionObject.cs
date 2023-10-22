using UnityEngine;

namespace Mechanics.ObjectsSelection
{
    public abstract class SelectionObject : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Outline _outline;
        [SerializeField] private Rigidbody _rigidbody;
        
        [Header("Selection")]
        [SerializeField] private Vector3 _holderPosition;
        [SerializeField] private Vector3 _positionInHolder;
        
        public bool IsPickedUp { get; private set; }

        public abstract void Initialize(Camera camera);
        
        public void SetActiveView(bool isActive)
        {
            _outline.enabled = isActive;
        }

        public void PickUp(Transform holder)
        {
            _rigidbody.isKinematic = true;

            holder.localPosition = _holderPosition;
            holder.localRotation = Quaternion.identity;

            transform.parent = holder;
            transform.localPosition = _positionInHolder;
            transform.localRotation = Quaternion.identity;

            IsPickedUp = true;
        }

        public void Drop()
        {
            transform.parent = null;
            _rigidbody.isKinematic = false;
            IsPickedUp = false;
        }
    }
}