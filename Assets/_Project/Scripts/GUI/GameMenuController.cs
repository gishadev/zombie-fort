using gishadev.fort.World.Shop;
using gishadev.fort.Core;
using gishadev.fort.World;
using gishadev.tools.UI;
using UnityEngine;

namespace gishadev.fort.GUI
{
    public class GameMenuController : MenuController
    {
        [SerializeField] private CanvasGroup mainGUIGroup;

        [SerializeField] private Page losePopupPage;
        [SerializeField] private Page winPopupPage;
        [SerializeField] private Page arsenalPopupPage;

        private Arsenal _arsenal;

        protected override void Awake()
        {
            base.Awake();
            _arsenal = FindObjectOfType<Location>().Arsenal;
        }

        private void OnEnable()
        {
            GameManager.Lost += OnGameLost;
            GameManager.Won += OnGameWon;

            _arsenal.ArsenalCameraLive += OnArsenalCameraLive;
            _arsenal.ArsenalClosed += OnArsenalClosed;
        }

        private void OnDisable()
        {
            GameManager.Lost -= OnGameLost;
            GameManager.Won -= OnGameWon;

            _arsenal.ArsenalCameraLive -= OnArsenalCameraLive;
            _arsenal.ArsenalClosed -= OnArsenalClosed;
        }

        private void OnGameLost()
        {
            PushPage(losePopupPage);
        }

        private void OnGameWon()
        {
            PushPage(winPopupPage);
        }

        private void OnArsenalCameraLive()
        {
            mainGUIGroup.alpha = 0f;
            PushPage(arsenalPopupPage);
        }

        private void OnArsenalClosed()
        {
            mainGUIGroup.alpha = 1f;
            PopPage();
        }
    }
}