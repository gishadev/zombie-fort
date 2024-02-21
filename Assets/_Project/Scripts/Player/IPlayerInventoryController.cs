using System.Collections.Generic;
using gishadev.fort.Weapons;

namespace gishadev.fort.Player
{
    public interface IPlayerInventoryController
    {
        List<WeaponDataSO> OwnedWeapons { get; }
        void AddWeapon(WeaponDataSO weaponDataSO);
        void Clear();
    }
}