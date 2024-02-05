using System;
using gishadev.fort.Core;
using TMPro;
using UnityEngine;

namespace gishadev.fort.Shop
{
    public class BuyPoint : MonoBehaviour
    {
        [SerializeField] private TMP_Text pointTMP;
        public event Action Triggered;

        public void Init(Buyable buyable)
        {
            pointTMP.text = $"BUY {buyable.Price}";
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Constants.PLAYER_TAG_NAME))
                return;

            Triggered?.Invoke();
        }
    }
}