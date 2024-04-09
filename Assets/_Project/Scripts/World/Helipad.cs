using System;
using UnityEngine;

namespace gishadev.fort.World
{
    public class Helipad : MonoBehaviour
    {
        public static Action<Helipad> HelipadTriggered;

        private void Start() => HelipadTriggered?.Invoke(this);

        private void OnTriggerEnter(Collider other)
        {
            HelipadTriggered?.Invoke(this);
        }
    }
}