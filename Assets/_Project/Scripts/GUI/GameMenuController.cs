using gishadev.fort.Core;
using gishadev.tools.UI;
using UnityEngine;

namespace gishadev.fort.GUI
{
    public class GameMenuController : MenuController
    {
        [SerializeField] private GamePopupPage losePopupPage;

        private void OnEnable()
        {
            GameManager.Lost += OnGameLost;
            GameManager.Won += OnGameWon;
        }

        private void OnDisable()
        {
            GameManager.Lost -= OnGameLost;
            GameManager.Won -= OnGameWon;
        }

        private void OnGameLost()
        {
            PushPage(losePopupPage);
        }

        private void OnGameWon()
        {
        }
    }
}