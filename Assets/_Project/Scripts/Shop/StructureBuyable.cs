using System;

namespace gishadev.fort.Shop
{
    public class StructureBuyable : Buyable
    {
        public override void TryBuy(Action onBuySuccess)
        {
            base.TryBuy(onBuySuccess);
            gameObject.SetActive(true);
        }
    }
}