using gishadev.fort.Money;
using TMPro;
using UnityEngine;
using Zenject;

namespace gishadev.fort.GUI
{
    public class MoneyGUIHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyCountTMP;

        [Inject] private IMoneyController _moneyController;

        private void Start() => moneyCountTMP.text = _moneyController.MoneyCount.ToString();
        private void OnEnable() => _moneyController.MoneyChanged += OnMoneyChanged;
        private void OnDisable() => _moneyController.MoneyChanged -= OnMoneyChanged;
        private void OnMoneyChanged(int newMoneyCount) => moneyCountTMP.text = newMoneyCount.ToString();
    }
}