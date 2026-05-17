using gishadev.fort.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gishadev.fort.Weapons
{
    public abstract class WeaponDataSO : ScriptableObject
    {
        [BoxGroup("General")] [SerializeField] private Vector3 localHandPosition;
        [BoxGroup("General")] [SerializeField] private Vector3 localHandEulerAngles;
        
        [AssetsOnly] [SerializeField] private GameObject weaponMeshPrefab;
        [GUIColor("red")] [SerializeField] private int damage = 5;
        [GUIColor("yellow")] [SerializeField] private int price = 5;

        [SerializeField] private CharacterWeaponState weaponState;

        public Vector3 LocalHandPosition => localHandPosition;
        public Vector3 LocalHandEulerAngles => localHandEulerAngles;
        
        public GameObject WeaponMeshPrefab => weaponMeshPrefab;
        public int Damage => damage;
        public int Price => price;
        public CharacterWeaponState WeaponState => weaponState;
    }

    public enum CharacterWeaponState
    {
        Empty = 0,
        Knife = 1,
        Handgun = 2,
        Rifle = 3
    }
}