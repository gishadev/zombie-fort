using gishadev.fort.Player;
using UnityEngine;

namespace gishadev.fort.Weapons
{
    public class WeaponGiver : MonoBehaviour
    {
        [SerializeField] private GunDataSO newGunData;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player.Player player))
                return;

            player.GetComponent<WeaponController>().SwitchWeapon(newGunData);
            Destroy(gameObject);
        }
    }
}