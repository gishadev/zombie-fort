using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace gishadev.fort.GUI
{
    public class HealthGUIHandler : MonoBehaviour
    {
        [SerializeField] private Image healthFillImage;
        [SerializeField] private TMP_Text healthCountTMP;

        private Player.Player _player;

        private void Start()
        {
            _player = FindObjectOfType<Player.Player>();

            OnPlayerHealthChanged(_player.Health);
            
            _player.HealthChanged += OnPlayerHealthChanged;
        }

        private void OnDestroy() => _player.HealthChanged -= OnPlayerHealthChanged;

        private void OnPlayerHealthChanged(int health)
        {
            float fillValue = (float) health / _player.MaxHealth;
            healthFillImage.transform.localScale = new Vector3(fillValue, 1, 1);
            healthCountTMP.text = health.ToString();
        }
    }
}