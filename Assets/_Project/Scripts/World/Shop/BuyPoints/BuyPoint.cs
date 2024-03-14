using System;
using gishadev.fort.Core;
using TMPro;
using UnityEngine;

namespace gishadev.fort.World.Shop.BuyPoints
{
    public abstract class BuyPoint : MonoBehaviour
    {
        [SerializeField] private TMP_Text pointTMP;
        public event Action Triggered;

        public void Init(Buyable buyable)
        {
            pointTMP.text = $"BUY {buyable.Price}";
        }

        private void OnEnable() => ShopBuyHandler.BuySucceeded += OnBuySucceededTriggered;
        private void OnDisable() => ShopBuyHandler.BuySucceeded -= OnBuySucceededTriggered;

        private void OnBuySucceededTriggered(ShopBuyHandler shopBuyHandler)
        {
            if (shopBuyHandler.BuyPoint != this)
                return;

            OnBuySuccess();
        }
        
        protected abstract void OnBuySuccess();
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Constants.TAG_NAME_PLAYER))
                return;

            Triggered?.Invoke();
        }
    }
}