using System.Collections;
using Mechanics.Attack;
using UnityEngine;

namespace Weapon
{
    public class AssaultRifle : Weapon
    {
        [SerializeField] private int _bulletsInBurstCount;
        [SerializeField] private int _fireRate;
        [SerializeField] private float _attackDistance;
        [SerializeField] private Vector2 _spreadRange;
        private enum ShootingType
        {
            Auto,
            Burst
        }
        [SerializeField] private ShootingType _shootingType;

        private Attack _attack;
        private float _timeBetweenShots;
        private float _lastShootTime;
        private bool _isShooting;
        private bool _isBurstFired = true;

        protected override void Awake()
        {
            base.Awake();
            
            _timeBetweenShots = 60.0f / _fireRate;
            SetAmmo();
        }
        
        public override void Initialize(Camera camera)
        {
            _attack = new RaycastAttack(camera, ShotOrigin, _attackDistance, _spreadRange, AttackMask, Damage);
        }

        public override void SetFireButtonState(bool isPressed)
        {
            base.SetFireButtonState(isPressed);
            
            if (isPressed && _shootingType == ShootingType.Burst)
            {
                _isShooting = false;
            }
        }

        public override void PerformAttack()
        {
            if (!IsReloading && IsFireButtonPressed)
            {
                switch (_shootingType)
                {
                    case ShootingType.Auto:
                        StartCoroutine(AutomaticShooting());
                        break;
                    case ShootingType.Burst:
                        StartCoroutine(BurstShooting());
                        break;
                }
            }
        }
        
        private void Shoot()
        {
            Trail trail = Instantiate(Trail, ShotOrigin.position, Quaternion.identity);
            _attack.PerformAttack(trail);
            CurrentAmmo -= 1;
        }

        private IEnumerator AutomaticShooting()
        {
            if (!_isShooting && CurrentAmmo > 0)
            {
                _isShooting = true;
                Shoot();
                yield return new WaitForSeconds(_timeBetweenShots);
                _isShooting = false;
            }
        }

        private IEnumerator BurstShooting()
        {
            if (!_isShooting && _isBurstFired && CurrentAmmo > 0)
            {
                _isShooting = true;
                _isBurstFired = false;
                for (int i = 0; i < _bulletsInBurstCount; i++)
                {
                    if (CurrentAmmo > 0)
                    {
                        Shoot();
                        yield return new WaitForSeconds(_timeBetweenShots);
                    }
                }
                _isBurstFired = true;
            }
        }
    }
}
