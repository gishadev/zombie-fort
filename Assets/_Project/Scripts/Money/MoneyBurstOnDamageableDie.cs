using System;
using gishadev.fort.Core;
using UnityEngine;
using Zenject;

namespace gishadev.fort.Money
{
    [RequireComponent(typeof(IDamageable))]
    public class MoneyBurstOnDamageableDie : MonoBehaviour
    {
        [SerializeField] private int coinsCount = 5;

        [Inject] private IMoneySpawner _moneySpawner;

        private IDamageable _damageable;

        private void Awake() => _damageable = GetComponent<IDamageable>();
        private void OnEnable() => _damageable.HealthChanged += OnHealthChanged;
        private void OnDisable() => _damageable.HealthChanged -= OnHealthChanged;

        private void OnHealthChanged(int health)
        {
            if (health <= 0)
                _moneySpawner.BurstSpawnMoney(transform.position, coinsCount, 3f);
        }
    }
}