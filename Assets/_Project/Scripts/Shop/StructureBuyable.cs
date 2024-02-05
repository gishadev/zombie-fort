namespace gishadev.fort.Shop
{
    public class StructureBuyable : Buyable
    {
        protected override void OnBuySuccess() => gameObject.SetActive(true);
    }
}