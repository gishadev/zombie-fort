using DG.Tweening;
using gishadev.tools.Core;
using UnityEngine;

namespace gishadev.tools.UI
{
    public class PageTransitionProcessor
    {
        private readonly Page _page;

        private readonly float _slideDuration = 1f;
        private readonly float _fadeDuration = 0.6f;
        private readonly float _scaleDuration = 0.5f;

        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;

        public PageTransitionProcessor(Page page)
        {
            _page = page;

            _rectTransform = page.GetComponent<RectTransform>();
            _canvasGroup = page.gameObject.GetOrAddComponent<CanvasGroup>();
        }

        public void DoEnterTransition()
        {
            _page.gameObject.SetActive(true);
            switch (_page.EnterTransition)
            {
                case PageTransitionType.None:
                default:
                    return;

                case PageTransitionType.SideSlide:
                    SideSlideTransitionEffect(-Screen.width, 0f, _slideDuration, Ease.OutElastic);
                    break;

                case PageTransitionType.VerticalSlide:
                    VerticalSlideTransitionEffect(Screen.height, 0f, _slideDuration, Ease.OutElastic);
                    break;

                case PageTransitionType.Fade:
                    FadeTransitionEffect(0f, 1f, _fadeDuration);
                    break;

                case PageTransitionType.Scale:
                    ScaleTransitionEffect(0f, 1f, _scaleDuration, Ease.OutElastic);
                    break;
            }
        }

        public void DoExitTransition()
        {
            Sequence seq = DOTween.Sequence();
            switch (_page.ExitTransition)
            {
                case PageTransitionType.None:
                default:
                    _page.gameObject.SetActive(false);
                    return;

                case PageTransitionType.SideSlide:
                    seq = SideSlideTransitionEffect(0f, Screen.width, _slideDuration, Ease.InOutQuint);
                    break;

                case PageTransitionType.VerticalSlide:
                    seq = VerticalSlideTransitionEffect(0f, -Screen.height, _slideDuration, Ease.InOutQuint);
                    break;

                case PageTransitionType.Fade:
                    seq = FadeTransitionEffect(1f, 0f, _fadeDuration, Ease.OutSine);
                    break;

                case PageTransitionType.Scale:
                    seq = ScaleTransitionEffect(1f, 0f, _scaleDuration, Ease.InOutQuint);
                    break;
            }

            seq.OnComplete(() =>
            {
                _page.gameObject.SetActive(false);
                _canvasGroup.alpha = 1f;
                _rectTransform.localPosition = Vector3.zero;
                _rectTransform.transform.localScale = Vector3.one;
            });
        }

        #region Transition Effects

        private Sequence SideSlideTransitionEffect(float startValue, float endValue, float duration,
            Ease ease = Ease.InSine)
        {
            _rectTransform.localPosition = Vector3.right * startValue;

            var seq = DOTween.Sequence();
            seq.Append(_rectTransform
                .DOAnchorPos(Vector3.right * endValue, duration)
                .SetEase(ease))
                .SetUpdate(true);

            return seq;
        }

        private Sequence VerticalSlideTransitionEffect(float startValue, float endValue, float duration,
            Ease ease = Ease.InSine)
        {
            _rectTransform.localPosition = Vector3.up * startValue;

            var seq = DOTween.Sequence();
            seq.Append(_rectTransform
                .DOAnchorPos(Vector3.up * endValue, duration)
                .SetEase(ease))
                .SetUpdate(true);

            return seq;
        }

        private Sequence FadeTransitionEffect(float startValue, float endValue, float duration,
            Ease ease = Ease.InSine)
        {
            _canvasGroup.alpha = startValue;

            var seq = DOTween.Sequence();
            seq.Append(_canvasGroup
                .DOFade(endValue, duration)
                .SetEase(ease))
                .SetUpdate(true);

            return seq;
        }

        private Sequence ScaleTransitionEffect(float startValue, float endValue, float duration,
            Ease ease = Ease.InSine)
        {
            _rectTransform.transform.localScale = Vector3.one * startValue;

            var seq = DOTween.Sequence();
            seq.Append(_rectTransform.transform
                .DOScale(endValue, duration)
                .SetEase(ease))
                .SetUpdate(true);

            return seq;
        }

        #endregion
    }
}