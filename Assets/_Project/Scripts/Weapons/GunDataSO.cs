using Sirenix.OdinInspector;
using UnityEngine;

namespace gishadev.fort.Weapons
{
    [CreateAssetMenu(fileName = "GunDataSO", menuName = "ScriptableObjects/GunDataSO")]
    public class GunDataSO : WeaponDataSO
    {
        [BoxGroup("General")] [SerializeField] private float shootDelay = 0.1f;
        [BoxGroup("General")] [SerializeField] private float shootForce = 10f;
        [BoxGroup("General")] [SerializeField] private int shootRaysCount = 1;

        [BoxGroup("Accuracy")]
        [SerializeField, Range(0f, 1f)]
        private float maxAccuracy = 1f;

        [BoxGroup("Accuracy")]
        [SerializeField, Range(0f, 1f)]
        private float minAccuracy = 0.5f;

        [BoxGroup("Accuracy")]
        [SerializeField]
        private float maxShootOffset = 0.1f;

        [BoxGroup("Ammo")] [SerializeField] private bool isInfinityMagazines;
        [BoxGroup("Ammo")] [SerializeField] private int startAmmoInMagazine = 14;
        [BoxGroup("Ammo")] [SerializeField] private int startMagazinesCount = 3;
        [BoxGroup("Ammo")] [SerializeField] private float reloadTime = 1f;

        public float ShootDelay => shootDelay;
        public float ShootForce => shootForce;
        public bool IsInfinityMagazines => isInfinityMagazines;
        public int StartAmmoInMagazine => startAmmoInMagazine;
        public int StartMagazinesCount => startMagazinesCount;
        public float ReloadTime => reloadTime;
        public float MaxAccuracy => maxAccuracy;
        public float MinAccuracy => minAccuracy;
        public float MaxShootOffset => maxShootOffset;
        public int ShootRaysCount => shootRaysCount;
    }
}