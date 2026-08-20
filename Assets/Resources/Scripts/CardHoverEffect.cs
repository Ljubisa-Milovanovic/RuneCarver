using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Resources.Scripts
{
    public class CardHoverEffect : MonoBehaviour
    {
        [SerializeField] private float hoverScale = 1.2f;
        [SerializeField] private float tweenDuration = 0.2f;
        [SerializeField] private Ease tweenEase = Ease.OutQuad;

        private Vector3 _baseScale;
        private int _baseSiblingIndex;
        private Tween _scaleTween;

        private void Awake()
        {
            _baseScale = transform.localScale;
        }

        private void OnMouseEnter()
        {
            _baseSiblingIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();

            ScaleTo(_baseScale * hoverScale);
            Debug.Log("uso sam");
        }

        private void OnMouseExit()
        {
            transform.SetSiblingIndex(_baseSiblingIndex);
            ScaleTo(_baseScale);
        }

        private void ScaleTo(Vector3 target)
        {
            _scaleTween?.Kill();
            _scaleTween = transform.DOScale(target, tweenDuration)
                .SetEase(tweenEase)
                .SetLink(gameObject);
        }
    }
}