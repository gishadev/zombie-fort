using System;
using gishadev.fort.World.Shop;
using Zenject;

namespace gishadev.fort.Level
{
    public class IslandLevelController : IIslandLevelController, IInitializable, IDisposable
    {
        public int CurrentLevel { get; private set; } = 1;
        public float CurrentProgress { get; private set; }

        public event Action<int> LevelChanged;
        public event Action<float> ProgressChanged;

        public void Initialize()
        {
            ShopBuyHandler.BuySucceeded += OnShopBuySucceeded;

            ProgressChanged?.Invoke(CurrentProgress);
            LevelChanged?.Invoke(CurrentLevel);
        }

        public void Dispose()
        {
            ShopBuyHandler.BuySucceeded -= OnShopBuySucceeded;
        }

        private void OnShopBuySucceeded(ShopBuyHandler shopBuyHandler)
        {
            if (shopBuyHandler.Buyable is not StructureBuyable) return;

            CurrentProgress += 0.5f;

            if (CurrentProgress >= 1)
            {
                CurrentProgress = 0;
                CurrentLevel++;
                LevelChanged?.Invoke(CurrentLevel);
            }

            ProgressChanged?.Invoke(CurrentProgress);
        }
    }
}