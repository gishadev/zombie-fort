using UnityEngine;
using Zenject;

namespace gishadev.fort.Money.Magnet
{
    public class PlayerGoldCollector : MonoBehaviour, ICoinMagnet
    {
        [Inject] private IMoneyController _moneyController;

        public void OnCoinCollect(MoneyCoin coin)
            => _moneyController.AddMoney(coin.MoneyToAdd);
    }
}