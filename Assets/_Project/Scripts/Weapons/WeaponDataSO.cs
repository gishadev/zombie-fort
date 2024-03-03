using Sirenix.OdinInspector;
using UnityEngine;

namespace gishadev.fort.Weapons
{
    public abstract class WeaponDataSO : ScriptableObject
    {
        [AssetsOnly] [SerializeField] private GameObject weaponMeshPrefab;
        [GUIColor("red")] [SerializeField] private int damage = 5;
        [GUIColor("yellow")] [SerializeField] private int price = 5;

        public GameObject WeaponMeshPrefab => weaponMeshPrefab;
        public int Damage => damage;
        public int Price => price;
    }
}