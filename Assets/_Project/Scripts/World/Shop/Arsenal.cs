using UnityEngine;

namespace gishadev.fort.World.Shop
{
    public class Arsenal : MonoBehaviour
    {
        [SerializeField] private POITrigger trigger;

        public POITrigger Trigger => trigger;
    }
}