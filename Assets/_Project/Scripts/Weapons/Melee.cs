using gishadev.fort.Core;
using UnityEngine;

namespace gishadev.fort.Weapons
{
    public class Melee : Weapon
    {
        public override bool IsAttacking { get; protected set; }
        public MeleeDataSO MeleeDataSO { get; private set; }

        public void SetupMelee(MeleeDataSO meleeDataSO)
        {
            MeleeDataSO = meleeDataSO;
        }

        public override void OnAttackPerformed()
        {
            IsAttacking = true;
            Debug.Log("melee attack performed!");
            gameObject.SetActive(true);

            var ray = new Ray(transform.position, transform.right);
            if (Physics.SphereCast(ray, 0.5f, out var hit, 1f))
            {
                var damageable = hit.collider.GetComponent<IDamageable>();
                damageable?.TakeDamage(MeleeDataSO.Damage, transform.forward * MeleeDataSO.PunchForce);
            }

            Debug.DrawRay(ray.origin, ray.direction * 1f, Color.red, 1f);
        }

        public override void OnAttackCanceled()
        {
            IsAttacking = false;
            gameObject.SetActive(false);
        }
    }
}