using Sirenix.OdinInspector;
using UnityEngine;

namespace gishadev.fort.World.Shop
{
    public class StructureBuyable : Buyable
    {
        [Required] [SerializeField] private GameObject[] objectsToEnable;

        private void Awake()
        {
            foreach (var obj in objectsToEnable)
                obj.SetActive(false);
        }

        protected override void OnBuySuccess()
        {
            foreach (var obj in objectsToEnable)
                obj.SetActive(true);
        }
    }
}