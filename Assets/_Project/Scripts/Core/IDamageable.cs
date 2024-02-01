using UnityEngine;

namespace gishadev.fort.Core
{
    public interface IDamageable
    {
        void TakeDamage(int damage, Vector3 hitDirection);
    }
}