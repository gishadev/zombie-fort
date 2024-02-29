using System.Collections.Generic;
using gishadev.fort.Core;
using UnityEngine;

namespace gishadev.fort.Weapons
{
    public class Melee : Weapon
    {
        public override bool IsAttacking { get; protected set; }
        public MeleeDataSO MeleeDataSO { get; private set; }

        private List<IDamageable> _damageables = new();

        private void Update()
        {
            if (!IsAttacking)
                return;

            var ray = new Ray(transform.position, transform.right);
            if (Physics.SphereCast(ray, 0.5f, out var hit, 1f))
            {
                var damageable = hit.collider.GetComponent<IDamageable>();

                if (!_damageables.Contains(damageable))
                {
                    damageable?.TakeDamage(MeleeDataSO.Damage, transform.right * MeleeDataSO.PunchForce);
                    _damageables.Add(damageable);
                }
            }

            Debug.DrawRay(ray.origin, ray.direction, Color.red, 1f);
        }

        public void SetupMelee(MeleeDataSO meleeDataSO)
        {
            MeleeDataSO = meleeDataSO;
        }

        public override void OnAttackPerformed()
        {
            IsAttacking = true;
            Debug.Log("melee attack performed!");
        }

        public override void OnAttackCanceled()
        {
            IsAttacking = false;
            _damageables.Clear();
        }
    }
}