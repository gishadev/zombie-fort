using gishadev.fort.Core;
using UnityEngine;

namespace gishadev.fort.Weapons
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Transform shootPoint;

        public void Shoot()
        {
            if (Physics.Raycast(shootPoint.position, shootPoint.forward, out var hit, 100))
            {
                var damageable = hit.collider.GetComponent<IDamageable>();
                damageable?.TakeDamage(10, shootPoint.forward);
            }

            Debug.DrawRay(shootPoint.position, shootPoint.forward * 100, Color.red, 1f);
        }
    }
}