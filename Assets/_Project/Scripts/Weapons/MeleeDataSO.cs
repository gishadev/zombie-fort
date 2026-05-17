using UnityEngine;

namespace gishadev.fort.Weapons
{
    [CreateAssetMenu(fileName = "MeleeDataSO", menuName = "ScriptableObjects/MeleeDataSO")]
    public class MeleeDataSO : WeaponDataSO
    {
        [SerializeField] private float attackDelay = 1f;
        [SerializeField] private float punchForce = 10f;

        public float AttackDelay => attackDelay;

        public float PunchForce => punchForce;
    }
}