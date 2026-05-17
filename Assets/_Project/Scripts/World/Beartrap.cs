using gishadev.fort.Enemy;
using UnityEngine;

namespace gishadev.fort.World
{
    /// <summary>
    /// Instant kill of an enemy that steps on it.
    /// </summary>
    public class Beartrap : MonoBehaviour
    {
        [SerializeField] private float cooldownTime = 1.5f;

        private bool _isUsed;
        private MeshRenderer _mr;

        private void Awake()
        {
            _mr = GetComponent<MeshRenderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out EnemyBase enemyBase) || _isUsed)
                return;

            Use(enemyBase);
        }

        private void Use(EnemyBase enemyBase)
        {
            enemyBase.TakeDamage(9999, Vector3.zero);
            _isUsed = true;
            _mr.enabled = false;
            Invoke(nameof(Reset), cooldownTime);
        }

        private void Reset()
        {
            _isUsed = false;
            _mr.enabled = true;
        }
    }
}