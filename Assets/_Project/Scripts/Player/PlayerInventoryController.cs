using System.Collections.Generic;
using gishadev.fort.Weapons;

namespace gishadev.fort.Player
{
    public class PlayerInventoryController : IPlayerInventoryController
    {
        public List<WeaponDataSO> OwnedWeapons { get; private set; } = new();

        public void AddWeapon(WeaponDataSO weaponDataSO)
        {
            if (!OwnedWeapons.Contains(weaponDataSO)) 
                OwnedWeapons.Add(weaponDataSO);
        }

        public void Clear() => OwnedWeapons.Clear();
    }
}