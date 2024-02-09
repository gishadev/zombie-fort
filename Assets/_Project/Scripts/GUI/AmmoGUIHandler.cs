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
                ammoCountTMP.text = $"{gun.CurrentAmmo}/{gun.MaxAmmo}";
            else
                ammoCountTMP.text = "";
        }

        private void OnEnable()
        {
            Weapon.Attack += OnFirearmAttack;
        }

        private void OnDisable()
        {
            Weapon.Attack -= OnFirearmAttack;
        }

        private void OnFirearmAttack(Weapon weapon)
        {
            if (weapon is not Gun gun)
                return;

            ammoCountTMP.text = $"{gun.CurrentAmmo}/{gun.MaxAmmo}";
        }
    }
}