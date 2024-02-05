using Sirenix.OdinInspector;
using UnityEngine;

namespace gishadev.fort.Shop
{
    public class ShopBuyHandler : MonoBehaviour
    {
        [Required] [SerializeField] private BuyPoint buyPoint;
        [Required] [SerializeField] private Buyable buyable;

        private void OnEnable()
        {
            buyPoint.Init(buyable);
            buyPoint.Triggered += OnBuyPointTriggered;
        }

        private void OnDisable()
        {
            buyPoint.Triggered -= OnBuyPointTriggered;
        }

        private void OnBuyPointTriggered()
        {
            buyable.TryBuy(OnBuySuccess);
        }

        private void OnBuySuccess()
        {
            buyPoint.gameObject.SetActive(false);
        }
    }
}