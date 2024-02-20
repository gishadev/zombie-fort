using UnityEngine;

namespace gishadev.fort.World
{
    public class Arsenal : MonoBehaviour
    {
        [SerializeField] private POITrigger trigger;

        public POITrigger Trigger => trigger;
    }
}