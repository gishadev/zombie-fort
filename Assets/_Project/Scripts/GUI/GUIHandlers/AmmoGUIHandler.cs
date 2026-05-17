using gishadev.fort.Player;
using gishadev.fort.Weapons;
using TMPro;
using UnityEngine;

namespace gishadev.fort.GUI
{
    public class AmmoGUIHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text ammoCountTMP;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            var weaponController = FindObjectOfType<WeaponController>();

            if (weaponController.EquippedGun != null)
            {
                UpdateGunAmmoTMP(weaponController.EquippedGun);
                _canvasGroup.alpha = 1f;
            }
            else
                _canvasGroup.alpha = 0f;
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
            _canvasGroup.alpha = 1f;
            ammoCountTMP.text = gun.GunDataSO.IsInfinityMagazines
                ? $"{gun.CurrentAmmoInMagazine}/∞"
                : $"{gun.CurrentAmmoInMagazine}/{gun.CurrentAmmo}";
        }
    }
}