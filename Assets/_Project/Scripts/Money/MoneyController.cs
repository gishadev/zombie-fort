using System;

namespace gishadev.fort.Money
{
    public class MoneyController : IMoneyController
    {
        public int MoneyCount { get; private set; }
        public event Action<int> MoneyChanged;

        public void AddMoney(int amountToAdd)
        {
            ChangeMoney(MoneyCount + amountToAdd);
        }

        public void ChangeMoney(int newAmount)
        {
            MoneyCount = newAmount;
            MoneyChanged?.Invoke(MoneyCount);
        }
    }
}