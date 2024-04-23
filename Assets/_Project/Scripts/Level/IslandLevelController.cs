using System;
using gishadev.fort.World.Shop;
using Zenject;

namespace gishadev.fort.Level
{
    public class IslandLevelController : IIslandLevelController, IInitializable, IDisposable
    {
        public int CurrentLevel { get; private set; } = 1;

        public event Action<int> LevelChanged;

        public void Initialize()
        {
            ShopBuyHandler.BuySucceeded += OnShopBuySucceeded;
        }

        public void Dispose()
        {
            ShopBuyHandler.BuySucceeded -= OnShopBuySucceeded;
        }

        private void OnShopBuySucceeded(ShopBuyHandler shopBuyHandler)
        {
            if (shopBuyHandler.Buyable is not StructureBuyable) return;

            CurrentLevel++;
            LevelChanged?.Invoke(CurrentLevel);
        }
    }
}