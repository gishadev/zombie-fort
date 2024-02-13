using Cysharp.Threading.Tasks;
using UnityEngine;

namespace gishadev.fort.Shop
{
    public class DelayedBuyPoint : BuyPoint
    {
        [SerializeField] private float delayToShow = 10f;

        protected override async void OnBuySuccess()
        {
            gameObject.SetActive(false);
            await UniTask.WaitForSeconds(delayToShow);
            gameObject.SetActive(true);
        }
    }
}