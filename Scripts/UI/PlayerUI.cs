using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        [Header("Player stats")]
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _armorBar;

        [Header("Crosshairs")]
        [SerializeField] private Crosshair[] _crosshairs;
        [SerializeField] private Color _crosshairColor;

        private Crosshair _currentCrosshair;
        private HitMarker _currentHitMarker;

        public static PlayerUI Instance;

        public HitMarker HitMarker => _currentHitMarker;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
            InstantiateCrosshair(WeaponType.Hand);
        }

        private void Update()
        {
            if (_currentCrosshair != null)
            {
                _currentCrosshair.Animate();
            }
        }

        public void UpdateHealth(float healthPercent, float armorPercent)
        {
            _armorBar.fillAmount = armorPercent;
            _healthBar.fillAmount = healthPercent;
        }
        
        private Crosshair TryGetCrosshair(WeaponType type)
        {
            foreach (Crosshair crosshair in _crosshairs)
            {
                if (crosshair.GetWeaponType() == type)
                {
                    return crosshair;
                }
            }

            return null;
        }

        public void InstantiateCrosshair(WeaponType type)
        {
            if (_currentCrosshair != null)
            {
                Destroy(_currentCrosshair.gameObject);
                _currentHitMarker = null;
            }
            
            Crosshair newCrosshair = TryGetCrosshair(type);

            if (newCrosshair != null)
            {
                _currentCrosshair = Instantiate(newCrosshair, transform);
                _currentCrosshair.SetColor(_crosshairColor);
                _currentHitMarker = _currentCrosshair.GetHitMarker();
            }
        }
    }
}
