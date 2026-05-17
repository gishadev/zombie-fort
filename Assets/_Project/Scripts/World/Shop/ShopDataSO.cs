using gishadev.fort.Weapons;
using UnityEngine;

namespace gishadev.fort.World.Shop
{
    [CreateAssetMenu(fileName = "ShopData", menuName = "ScriptableObjects/ShopData")]
    public class ShopDataSO : ScriptableObject
    {
        [SerializeField] private WeaponDataSO[] buyableWeapons;

        public WeaponDataSO[] BuyableWeapons => buyableWeapons;
    }
}