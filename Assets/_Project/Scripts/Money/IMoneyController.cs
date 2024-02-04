using System;

namespace gishadev.fort.Money
{
    public interface IMoneyController
    {
        int MoneyCount { get; }
        event Action<int> MoneyChanged;
        void AddMoney(int amountToAdd);
        void ChangeMoney(int newAmount);
    }
}