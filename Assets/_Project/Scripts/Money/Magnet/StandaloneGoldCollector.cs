using gishadev.fort.World;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace gishadev.fort.Money.Magnet
{
    public class StandaloneGoldCollector : MonoBehaviour, ICoinMagnet
    {
        [Required] [SerializeField] private POITrigger trigger;

        [Inject] private IMoneyController _moneyController;

        private int _collectedMoney;

        private void OnEnable() => trigger.TriggerEntered += WithdrawCoins;
        private void OnDisable() => trigger.TriggerEntered -= WithdrawCoins;

        private void WithdrawCoins()
        {
            Debug.Log("Coins to withdraw: " + _collectedMoney + "!");
            _moneyController.AddMoney(_collectedMoney);
            _collectedMoney = 0;
        }

        public void OnCoinCollect(MoneyCoin coin)
        {
            _collectedMoney += coin.MoneyToAdd;
        }
    }
}