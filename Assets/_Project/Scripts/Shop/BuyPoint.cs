using gishadev.fort.Core;
using UnityEngine;

namespace gishadev.fort.Shop
{
    public class BuyPoint : MonoBehaviour
    {
        [SerializeField] private Buyable buyable;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Constants.PLAYER_TAG_NAME))
                return;

            buyable.TryBuy(OnBuySuccess);
        }

        private void OnBuySuccess()
        {
            gameObject.SetActive(false);
        }
    }
}