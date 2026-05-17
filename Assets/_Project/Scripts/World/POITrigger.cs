using System;
using UnityEngine;

namespace gishadev.fort.World
{
    public class POITrigger : MonoBehaviour
    {
        public event Action TriggerEntered;
        public event Action TriggerExited;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player.Player _))
                return;

            RaiseTriggerEntered();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Player.Player _))
                return;

            RaiseTriggerExited();
        }
        
        public void RaiseTriggerEntered() => TriggerEntered?.Invoke();
        public void RaiseTriggerExited() => TriggerExited?.Invoke();
    }
}