namespace gishadev.fort.World.Shop.BuyPoints
{
    public class DisposableBuyPoint : BuyPoint
    {
        protected override void OnBuySuccess() => gameObject.SetActive(false);
    }
}