namespace gishadev.fort.Shop
{
    public class DisposableBuyPoint : BuyPoint
    {
        protected override void OnBuySuccess() => gameObject.SetActive(false);
    }
}