using Character;
using UI;
using UnityEngine;

namespace Mechanics.Attack
{
    public class RaycastAttack : Attack
    {
        private readonly Camera _camera;
        private readonly Transform _rayOrigin;
        private readonly float _attackDistance;
        private readonly Vector2 _spreadRange;

        public RaycastAttack(Camera camera, Transform rayOrigin, float attackDistance, Vector2 spreadRange,
            LayerMask attackMask, int damage) : base(attackMask, damage)
        {
            _camera = camera;
            _attackDistance = attackDistance;
            _spreadRange = spreadRange;
            _rayOrigin = rayOrigin;
        }

        public override void PerformAttack(Trail trail)
        {
            Vector3 cameraRayHitPosition = CameraRayHitPosition(_camera, _attackDistance, _spreadRange);
            Vector3 directionToTarget = cameraRayHitPosition - _rayOrigin.position;
            trail.ShowTrail(_rayOrigin.position, cameraRayHitPosition);

            if (Physics.Raycast(_rayOrigin.position, directionToTarget, out RaycastHit hit,
                    directionToTarget.magnitude, AttackMask.value))
            {
                int damage = Damage;

                if (hit.transform.root.TryGetComponent(out Health health))
                {
                    if (health.IsDied) return;
                    
                    if (hit.transform.parent.TryGetComponent(out HitZone zone))
                    {
                        damage = zone.ModifyDamage(Damage);
                    }
                    
                    health.ApplyDamage(damage);
                    PlayerUI.Instance.HitMarker.Show(health.IsDied);
                }
            }
        }
    }
}