using UnityEngine;

namespace gishadev.fort.Weapons
{
    public abstract class WeaponDataSO : ScriptableObject
    {
        [SerializeField] private GameObject weaponMeshPrefab;
        [SerializeField] private int damage = 5;
        
        [SerializeField] private int price = 5;

        public GameObject WeaponMeshPrefab => weaponMeshPrefab;
        public int Damage => damage;
        public int Price => price;
    }
}