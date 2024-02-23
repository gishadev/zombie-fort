using UnityEngine;

namespace gishadev.fort.World
{
    public class Helipad : MonoBehaviour
    {
        public static System.Action<Helipad> HelipadSpawned;

        private void Start()
        {
            Debug.Log("Helipad initialized!");
            HelipadSpawned?.Invoke(this);
        }
    }
}