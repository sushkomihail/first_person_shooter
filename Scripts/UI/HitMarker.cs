using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class HitMarker : MonoBehaviour
    {
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _killColor;
        [SerializeField] private float _fadingTime;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _image.SetAlpha(0);
        }

        public void Show(bool isKillMarker)
        {
            _image.color = isKillMarker ? _killColor : _defaultColor;
            StartCoroutine(ShowRoutine());
        }

        private IEnumerator ShowRoutine()
        {
            float time = 1;

            while (time > 0)
            {
                float alpha = Mathf.Lerp(0, 1, time);
                _image.SetAlpha(alpha);
                time -= Time.deltaTime / _fadingTime;
                yield return null;
            }

            _image.SetAlpha(0);
            yield return null;
        }
    }
}
