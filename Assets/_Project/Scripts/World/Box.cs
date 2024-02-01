using gishadev.fort.Core;
using UnityEngine;

namespace gishadev.fort.World
{
    [RequireComponent(typeof(Rigidbody))]
    public class Box : MonoBehaviour, IDamageable
    {
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void TakeDamage(int damage, Vector3 hitDirection)
        {
            _rb.AddForce(hitDirection * 10, ForceMode.Impulse);
        }
    }
}