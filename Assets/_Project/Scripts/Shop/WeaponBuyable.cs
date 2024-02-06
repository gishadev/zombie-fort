using gishadev.fort.Player;

namespace gishadev.fort.Shop
{
    public class WeaponBuyable : Buyable
    {
        protected override void OnBuySuccess()
        {
            FindObjectOfType<WeaponController>().SwitchToAK();
        }
    }
}