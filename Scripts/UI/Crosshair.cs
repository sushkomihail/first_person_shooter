using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class Crosshair : MonoBehaviour
    {
        [SerializeField] private WeaponType _type;
        
        [Header("Hit marker")]
        [SerializeField] private HitMarker _hitMarker;
        
        [Header("Crosshair")]
        [SerializeField] private Image[] _crosshairImages;

        protected RectTransform Rect;
        protected Vector2 DefaultSize;
        protected Vector2 CurrentSize;
        protected bool CanAnimate;

        private void Awake()
        {
            Rect = GetComponent<RectTransform>();
            DefaultSize = Rect.sizeDelta;
            CurrentSize = DefaultSize;
        }
        
        public WeaponType GetWeaponType()
        {
            return _type;
        }

        public HitMarker GetHitMarker()
        {
            return _hitMarker;
        }

        public void SetColor(Color color)
        {
            foreach (Image image in _crosshairImages)
            {
                image.color = color;
            }
        }

        public void AllowAnimate(bool canAnimate)
        {
            CanAnimate = canAnimate;
        }

        public virtual void Animate() { }
    }
}
