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

        private void Awake()
        {
            Health = MaxHealth;
        }

        public void TakeDamage(int damage, Vector3 hitForce)
        {
            Health -= damage;
            if (Health <= 0) gameObject.SetActive(false);

            HealthChanged?.Invoke(Health);
        }
    }
}