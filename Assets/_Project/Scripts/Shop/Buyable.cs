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
        
        public virtual void TryBuy(Action onBuySuccess)
        {
            if (_moneyController.MoneyCount < Price)
                return;
            
            _moneyController.AddMoney(-Price);
            onBuySuccess?.Invoke();
        }
    }
}