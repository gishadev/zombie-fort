using UnityEngine;

namespace gishadev.fort.Money.Magnet
{
    public interface ICoinMagnet
    {
        Transform transform { get; }
        GameObject gameObject { get; }
        
        public void OnCoinCollect(MoneyCoin coin);
    }
}