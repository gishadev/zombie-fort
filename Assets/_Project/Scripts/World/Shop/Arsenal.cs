using System;
using Cinemachine;
using gishadev.fort.Core;
using gishadev.tools.Events;
using UnityEngine;
using Zenject;

namespace gishadev.fort.World.Shop
{
    public class Arsenal : MonoBehaviour
    {
        [SerializeField] private POITrigger trigger;
        [SerializeField] private Transform previewRoot;
        [SerializeField] private Transform weaponPreviewParent;

        [SerializeField] private CmBlendFinishedNotifier blendFinishedNotifier;
        [SerializeField] private IntEventChannelSO arsenalWeaponSelectedEventChannelSO;

        [Inject] private ShopDataSO _shopDataSO;

        public POITrigger Trigger => trigger;
        public event Action ArsenalCameraLive;
        public event Action ArsenalOpened;
        public event Action ArsenalClosed;


        private void OnEnable()
        {
            Trigger.TriggerEntered += OnTriggerEntered;
            blendFinishedNotifier.OnBlendFinished.AddListener(OnCameraLive);
            arsenalWeaponSelectedEventChannelSO.ChangedValue += OnWeaponSelectedSelected;
        }

        private void OnDisable()
        {
            Trigger.TriggerEntered -= OnTriggerEntered;
            blendFinishedNotifier.OnBlendFinished.RemoveAllListeners();
            arsenalWeaponSelectedEventChannelSO.ChangedValue -= OnWeaponSelectedSelected;
        }

        public void CloseArsenal()
        {
            previewRoot.gameObject.SetActive(false);
            ArsenalClosed?.Invoke();
        }

        private void OnWeaponSelectedSelected(int index)
        {
            ClearWeaponParent();

            var previewObject = Instantiate(_shopDataSO.BuyableWeapons[index].WeaponMeshPrefab,
                weaponPreviewParent.transform);
            previewObject.transform.localPosition = Vector3.zero;
            previewObject.transform.localRotation = Quaternion.identity;
        }

        private void ClearWeaponParent()
        {
            for (int i = 0; i < weaponPreviewParent.transform.childCount; i++)
                Destroy(weaponPreviewParent.transform.GetChild(i).gameObject);
        }

        private void OnCameraLive(CinemachineVirtualCameraBase cinemachineVirtualCameraBase)
        {
            ArsenalCameraLive?.Invoke();
        }

        private void OnTriggerEntered()
        {
            previewRoot.gameObject.SetActive(true);
            ClearWeaponParent();
            ArsenalOpened?.Invoke();
        }
    }
}