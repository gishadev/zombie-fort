using System;
using gishadev.fort.Core;
using RayFire;
using UnityEngine;

namespace gishadev.fort.World
{
    [RequireComponent(typeof(RayfireRigid))]
    public class DestroyableObject : MonoBehaviour, IAutoAttackable
    {
        [SerializeField] private int startHealth = 10;

        public int Health { get; private set; }
        public event Action<int> HealthChanged;

        private RayfireRigid _rayfireRigid;

        private void Awake()
        {
            Health = startHealth;
            _rayfireRigid = GetComponent<RayfireRigid>();
        }

        public void TakeDamage(int damage, Vector3 hitForce)
        {
            Health -= damage;

            if (Health <= 0)
            {
                _rayfireRigid.Activate();
                _rayfireRigid.Demolish();
            }
        }

    }
}