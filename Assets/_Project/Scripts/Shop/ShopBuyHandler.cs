using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gishadev.fort.Shop
{
    public class ShopBuyHandler : MonoBehaviour
    {
        [InlineEditor, Required]
        [SerializeField]
        private BuyPoint buyPoint;

        [InlineEditor, Required]
        [SerializeField]
        private Buyable buyable;

        public static event Action<ShopBuyHandler> BuySucceeded;

        public Buyable Buyable => buyable;
        public BuyPoint BuyPoint => buyPoint;

        private void OnEnable()
        {
            BuyPoint.Init(Buyable);
            BuyPoint.Triggered += OnBuyPointTriggered;
        }

        private void OnDisable()
        {
            BuyPoint.Triggered -= OnBuyPointTriggered;
        }

        private void OnBuyPointTriggered()
        {
            Buyable.TryBuy(() => BuySucceeded?.Invoke(this));
        }
    }
}