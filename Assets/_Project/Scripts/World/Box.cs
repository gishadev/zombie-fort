using System;
using gishadev.fort.Core;
using UnityEngine;

namespace gishadev.fort.World
{
    [RequireComponent(typeof(Rigidbody))]
    public class Box : MonoBehaviour, IDamageable
    {
        public int Health { get; }
        private Rigidbody _rb;
        public event Action<int> HealthChanged;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }


        public void TakeDamage(int damage, Vector3 hitForce)
        {
            _rb.AddForce(hitForce, ForceMode.Impulse);
            HealthChanged?.Invoke(Health);
        }
    }
}