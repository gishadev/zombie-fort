using System;
using gishadev.fort.Core;
using UnityEngine;
using Zenject;

namespace gishadev.fort.Player
{
    public class Player : MonoBehaviour, IDamageable
    {
        [Inject] private GameDataSO _gameDataSO;

        public int Health { get; private set; }
        public int MaxHealth => _gameDataSO.PlayerMaxHealth;
        public event Action<int> HealthChanged;
        public event Action PlayerDied;

        private void Awake() => Health = MaxHealth;

        public void TakeDamage(int damage, Vector3 hitForce)
        {
            Health -= damage;
            if (Health <= 0)
            {
                PlayerDied?.Invoke();
                Health = 0;
            }

            HealthChanged?.Invoke(Health);
        }

        public void Heal(float percentage)
        {
            Health += Mathf.RoundToInt(MaxHealth * percentage);
            if (Health > MaxHealth)
                Health = MaxHealth;

            HealthChanged?.Invoke(Health);
        }
    }
}