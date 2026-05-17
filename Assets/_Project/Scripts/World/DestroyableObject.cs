using System;
using gishadev.fort.Core;
using UnityEngine;

namespace gishadev.fort.World
{
    public class DestroyableObject : MonoBehaviour, IAutoAttackable
    {
        [SerializeField] private int startHealth = 10;
        [SerializeField] private DemolishedFragmentsRoot fragmentsRoot;

        public int Health { get; private set; }
        public event Action<int> HealthChanged;

        private void Awake()
        {
            Health = startHealth;
        }

        public void TakeDamage(int damage, Vector3 hitForce)
        {
            Health -= damage;

            HealthChanged?.Invoke(Health);

            if (Health <= 0)
            {
                fragmentsRoot.Demolish();
                Destroy(gameObject);
            }
        }
    }
}