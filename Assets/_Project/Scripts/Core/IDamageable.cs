using UnityEngine;

namespace gishadev.fort.Core
{
    public interface IDamageable
    {
        int Health { get; }
        void TakeDamage(int damage, Vector3 hitDirection);
    }
}