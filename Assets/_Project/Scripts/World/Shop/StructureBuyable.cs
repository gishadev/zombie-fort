using Sirenix.OdinInspector;
using UnityEngine;

namespace gishadev.fort.World.Shop
{
    public class StructureBuyable : Buyable
    {
        [Required] [SerializeField] private GameObject objectToEnable;

        private void Awake() => objectToEnable.SetActive(false);
        protected override void OnBuySuccess() => objectToEnable.SetActive(true);
    }
}