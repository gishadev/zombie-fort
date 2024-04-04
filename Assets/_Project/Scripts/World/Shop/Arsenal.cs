using System;
using Cinemachine;
using gishadev.fort.Core;
using UnityEngine;

namespace gishadev.fort.World.Shop
{
    public class Arsenal : MonoBehaviour
    {
        [SerializeField] private POITrigger trigger;
        [SerializeField] private GameObject previewObject;

        [SerializeField] private CmBlendFinishedNotifier blendFinishedNotifier;
        public event Action ArsenalCameraLive;
        public event Action ArsenalOpened;
        public event Action ArsenalClosed;

        public POITrigger Trigger => trigger;

        private void OnEnable()
        {
            Trigger.TriggerEntered += OnTriggerEntered;
            blendFinishedNotifier.OnBlendFinished.AddListener(OnCameraLive);
        }

        private void OnDisable()
        {
            Trigger.TriggerEntered -= OnTriggerEntered;
            blendFinishedNotifier.OnBlendFinished.RemoveAllListeners();
        }

        private void OnCameraLive(CinemachineVirtualCameraBase cinemachineVirtualCameraBase)
        {
            ArsenalCameraLive?.Invoke();
        }

        private void OnTriggerEntered()
        {
            previewObject.SetActive(true);
            ArsenalOpened?.Invoke();
        }

        public void CloseArsenal()
        {
            previewObject.SetActive(false);
            ArsenalClosed?.Invoke();
        }
    }
}