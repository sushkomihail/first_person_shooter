using UnityEngine;

namespace UI
{
    public class AssaultRifleCrosshair : Crosshair
    {
        [SerializeField] private float _scaleMultiplier;
        [SerializeField] private float _animationSpeed;

        public override void Animate()
        {
            Vector2 targetSize = DefaultSize;
            if (CanAnimate)
            {
                targetSize = DefaultSize * _scaleMultiplier;
            }

            CurrentSize = Vector2.MoveTowards(CurrentSize, targetSize, _animationSpeed * Time.deltaTime);
            Rect.sizeDelta = CurrentSize;
        }
    }
}
