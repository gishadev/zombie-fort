using UnityEngine;

namespace gishadev.fort.Weapons
{
    [CreateAssetMenu(fileName = "GunDataSO", menuName = "ScriptableObjects/GunDataSO")]
    public class GunDataSO : WeaponDataSO
    {
        [SerializeField] private bool isInfinityMagazines;

        [SerializeField] private float shootDelay = 0.1f;
        [SerializeField] private float shootForce = 10f;
        
        [SerializeField, Range(0f,1f)] private float maxAccuracy = 1f;
        [SerializeField, Range(0f,1f)] private float minAccuracy = 0.5f;
        [SerializeField] private float maxShootOffset = 0.1f;

        [Space] [SerializeField] private int startAmmoInMagazine = 14;
        [SerializeField] private int startMagazinesCount = 3;

        [SerializeField] private float reloadTime = 1f;

        public float ShootDelay => shootDelay;
        public float ShootForce => shootForce;
        public bool IsInfinityMagazines => isInfinityMagazines;
        public int StartAmmoInMagazine => startAmmoInMagazine;
        public int StartMagazinesCount => startMagazinesCount;
        public float ReloadTime => reloadTime;
        public float MaxAccuracy => maxAccuracy;
        public float MinAccuracy => minAccuracy;
        public float MaxShootOffset => maxShootOffset;
    }
}