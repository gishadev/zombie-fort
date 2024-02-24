using gishadev.fort.World.Shop;
using gishadev.fort.Core;
using gishadev.fort.World;
using gishadev.tools.UI;
using UnityEngine;

namespace gishadev.fort.GUI
{
    public class GameMenuController : MenuController
    {
        [SerializeField] private Page losePopupPage;
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

            _arsenal.Trigger.TriggerEntered += OnArsenalTriggerEntered;
            _arsenal.Trigger.TriggerExited += OnArsenalTriggerExited;
        }

        private void OnDisable()
        {
            GameManager.Lost -= OnGameLost;
            GameManager.Won -= OnGameWon;

            _arsenal.Trigger.TriggerEntered -= OnArsenalTriggerEntered;
            _arsenal.Trigger.TriggerExited -= OnArsenalTriggerExited;
        }

        private void OnGameLost()
        {
            PushPage(losePopupPage);
        }

        private void OnGameWon()
        {
        }

        private void OnArsenalTriggerEntered() => PushPage(arsenalPopupPage);
        private void OnArsenalTriggerExited() => PopPage();
    }
}