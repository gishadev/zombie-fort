using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace gishadev.tools.SceneLoading
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader I
        {
            get
            {
                if (_current)
                    return _current;

                _current = new GameObject("[SceneLoader]").AddComponent<SceneLoader>();
                DontDestroyOnLoad(_current.gameObject);

                return _current;
            }
        }

        private static SceneLoader _current;

        private Canvas _canvas;

        private Image _fadeImage;
        private GameObject _fadeObject;
        private bool _isLoadingScene;

        private void Awake()
        {
            _canvas = gameObject.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.sortingOrder = 9999;

            _fadeObject = new GameObject("Fade");
            _fadeObject.transform.SetParent(_canvas.transform);

            _fadeImage = _fadeObject.AddComponent<Image>();
            _fadeImage.color = Color.black;

            var rt = _fadeObject.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;
            rt.anchoredPosition = Vector2.zero;

            gameObject.AddComponent<GraphicRaycaster>();
        }

        public async void AsyncSceneLoad(string sceneToLoad)
        {
            if (_isLoadingScene)
                return;
            
            _isLoadingScene = true;
            
            _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b, 0f);
            _fadeObject.gameObject.SetActive(true);
            
            await _fadeImage
                .DOFade(1f, .5f)
                .AsyncWaitForCompletion();

            var loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);

            while (!loadOperation.isDone)
                await UniTask.Yield();

            await _fadeImage
                .DOFade(0f, .5f)
                .OnComplete(() => _fadeObject.gameObject.SetActive(false))
                .AsyncWaitForCompletion();
            
            _isLoadingScene = false;
        }
    }
}