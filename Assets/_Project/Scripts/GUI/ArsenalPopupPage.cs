using System;
using System.Collections.Generic;
using gishadev.fort.Weapons;
using gishadev.tools.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace gishadev.fort.GUI
{
    public class ArsenalPopupPage : Page
    {
        [SerializeField] private Button buyButton, equipButton;
        [SerializeField] private TMP_Text selectedWeaponNameTMP;

        [SerializeField] private Transform container;

        private List<WeaponGUIElementHandler> _weaponGUIElementHandlers = new();
        private WeaponDataSO _selectedWeaponData;

        private void Awake()
        {
            _weaponGUIElementHandlers.AddRange(container.GetComponentsInChildren<WeaponGUIElementHandler>());

            selectedWeaponNameTMP.text = "";
            buyButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            foreach (var weaponGUIElementHandler in _weaponGUIElementHandlers)
                weaponGUIElementHandler.PointerDown += OnWeaponGUIPointerDown;
        }

        private void OnDisable()
        {
            foreach (var weaponGUIElementHandler in _weaponGUIElementHandlers)
                weaponGUIElementHandler.PointerDown -= OnWeaponGUIPointerDown;
        }
        
        private void OnWeaponGUIPointerDown(WeaponDataSO weaponDataSO)
        {
            _selectedWeaponData = weaponDataSO;
            selectedWeaponNameTMP.text = weaponDataSO.name;
            buyButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(true);
        }
    }
}