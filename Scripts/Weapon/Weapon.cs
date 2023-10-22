using System.Collections;
using Animation;
using Mechanics.Attack;
using Mechanics.ObjectsSelection;
using UnityEngine;
using Weapons;

namespace Weapon
{
    [RequireComponent(typeof(WeaponAnimator))]
    public abstract class Weapon : SelectionObject
    {
        [Header("Weapon type")]
        [SerializeField] private WeaponType _type;
        
        [Header("Weapon")]
        [SerializeField] protected LayerMask AttackMask;
        [SerializeField] protected Transform ShotOrigin;
        [SerializeField] protected int Damage;
        [SerializeField] private int _ammo;
        [SerializeField] private float _reloadingTime;
        [SerializeField] protected Trail Trail;

        private WeaponAnimator _animator;

        protected bool IsFireButtonPressed;
        protected bool IsReloading;
        protected int CurrentAmmo;

        protected virtual void Awake()
        {
            _animator = GetComponent<WeaponAnimator>();
        }

        public WeaponAnimator GetAnimator() => _animator;

        public WeaponType GetWeaponType() => _type;

        protected void SetAmmo() => CurrentAmmo = _ammo;

        public virtual void SetFireButtonState(bool isPressed) => IsFireButtonPressed = isPressed;

        public IEnumerator Reload()
        {
            IsReloading = true;
            yield return new WaitForSeconds(_reloadingTime);
            CurrentAmmo = _ammo;
            IsReloading = false;
        }

        public abstract void PerformAttack();
    }
}
