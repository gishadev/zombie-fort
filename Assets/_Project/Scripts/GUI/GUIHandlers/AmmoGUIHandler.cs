using gishadev.fort.Player;
using gishadev.fort.Weapons;
using TMPro;
using UnityEngine;

namespace gishadev.fort.GUI
{
    public class AmmoGUIHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text ammoCountTMP;

        private void Start()
        {
            var weaponController = FindObjectOfType<WeaponController>();

            if (weaponController.CurrentWeapon is Gun gun)
                UpdateGunAmmoTMP(gun);
            else
                ammoCountTMP.text = "";
        }

        private void OnEnable()
        {
            Weapon.Attack += UpdateGUI;
            Gun.Reloaded += UpdateGUI;
        }

        private void OnDisable()
        {
            Weapon.Attack -= UpdateGUI;
            Gun.Reloaded -= UpdateGUI;
        }

        private void UpdateGUI(Weapon weapon)
        {
            if (weapon is not Gun gun)
                return;

            UpdateGunAmmoTMP(gun);
        }

        private void UpdateGunAmmoTMP(Gun gun)
        {
            ammoCountTMP.text = gun.GunDataSO.IsInfinityMagazines
                ? $"{gun.CurrentAmmoInMagazine}/∞"
                : $"{gun.CurrentAmmoInMagazine}/{gun.CurrentAmmo}";
        }
    }
}