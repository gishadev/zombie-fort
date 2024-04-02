using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace gishadev.fort.GUI
{
    public class HealthGUIHandler : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
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
            healthSlider.value = fillValue;
            healthCountTMP.text = health.ToString();
        }
    }
}