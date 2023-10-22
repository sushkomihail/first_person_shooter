using System.Collections;
using UnityEngine;

namespace Mechanics.Attack
{
    public class ProjectileTrail : Trail
    {
        [SerializeField] private float _trailSpeed;

        protected override IEnumerator PlayTrail(Vector3 origin, Vector3 hitPosition)
        {
            float distance = Vector3.Distance(origin, hitPosition);
            float remainingDistance = distance;
            while (remainingDistance > 0)
            {
                float step = Mathf.Clamp01(1 - remainingDistance / distance);
                transform.position = Vector3.Lerp(origin, hitPosition, step);
                remainingDistance -= _trailSpeed * Time.deltaTime;
                yield return null;
            }
            transform.position = hitPosition;
            Destroy(gameObject);
        }

        public override void ShowTrail(Vector3 origin, Vector3 hitPosition)
        {
            StartCoroutine(PlayTrail(origin, hitPosition));
        }
    }
}