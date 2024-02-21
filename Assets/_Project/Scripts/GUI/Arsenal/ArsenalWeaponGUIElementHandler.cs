using System;
using gishadev.fort.Weapons;
using UnityEngine;
using UnityEngine.EventSystems;

namespace gishadev.fort.GUI
{
    public class ArsenalWeaponGUIElementHandler : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private WeaponDataSO weaponDataSO;
        public event Action<WeaponDataSO> PointerDown;

        public void OnPointerDown(PointerEventData eventData) => PointerDown?.Invoke(weaponDataSO);
    }
}