using System;
using gishadev.fort.Money;
using UnityEngine;
using Zenject;

namespace gishadev.fort.Shop
{
    public abstract class Buyable : MonoBehaviour
    {
        [field: SerializeField] public int Price { get; private set; }
        [Inject] private IMoneyController _moneyController;

        public void TryBuy(Action onBuySuccessCallback)
        {
            if (_moneyController.MoneyCount < Price)
                return;

            _moneyController.AddMoney(-Price);
            onBuySuccessCallback?.Invoke();
            OnBuySuccess();
        }

        protected abstract void OnBuySuccess();
    }
}