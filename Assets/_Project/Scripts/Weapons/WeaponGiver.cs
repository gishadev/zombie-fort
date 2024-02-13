using gishadev.fort.Player;
using UnityEngine;

namespace gishadev.fort.Weapons
{
    public class WeaponGiver : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player.Player player))
                return;

            player.GetComponent<WeaponController>().SwitchToAK();
            Destroy(gameObject);
        }
    }
}