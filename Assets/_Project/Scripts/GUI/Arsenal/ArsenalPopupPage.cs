using System;
using System.Collections.Generic;
using gishadev.fort.Money;
using gishadev.fort.Player;
using gishadev.fort.Weapons;
using gishadev.tools.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace gishadev.fort.GUI
{
    public class ArsenalPopupPage : Page
    {
        [SerializeField] private Button buyButton, equipButton;
        [SerializeField] private TMP_Text selectedWeaponNameTMP;

        [SerializeField] private Transform container;

        [Inject] private IMoneyController _moneyController;
        [Inject] private IPlayerInventoryController _playerInventoryController;

        private List<ArsenalWeaponGUIElementHandler> _weaponGUIElementHandlers = new();
        private WeaponDataSO _selectedWeaponData;

        private void Awake()
        {
            _weaponGUIElementHandlers.AddRange(container.GetComponentsInChildren<ArsenalWeaponGUIElementHandler>());

            selectedWeaponNameTMP.text = "";
            buyButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            foreach (var weaponGUIElementHandler in _weaponGUIElementHandlers)
                weaponGUIElementHandler.PointerDown += OnWeaponGUIPointerDown;

            buyButton.onClick.AddListener(OnBuyButtonClicked);
            equipButton.onClick.AddListener(OnEquipButtonClicked);
        }

        private void OnDisable()
        {
            foreach (var weaponGUIElementHandler in _weaponGUIElementHandlers)
                weaponGUIElementHandler.PointerDown -= OnWeaponGUIPointerDown;

            buyButton.onClick.RemoveListener(OnBuyButtonClicked);
            equipButton.onClick.RemoveListener(OnEquipButtonClicked);
        }

        private void OnWeaponGUIPointerDown(WeaponDataSO weaponDataSO)
        {
            _selectedWeaponData = weaponDataSO;
            selectedWeaponNameTMP.text = $"{weaponDataSO.name}/{weaponDataSO.Price}";

            if (_playerInventoryController.OwnedWeapons.Contains(weaponDataSO))
            {
                buyButton.gameObject.SetActive(false);
                equipButton.gameObject.SetActive(true);
            }
            else
            {
                buyButton.gameObject.SetActive(true);
                equipButton.gameObject.SetActive(false);
            }
        }

        private void OnBuyButtonClicked()
        {
            if (_selectedWeaponData == null)
                return;

            if (_moneyController.MoneyCount < _selectedWeaponData.Price)
                return;

            _moneyController.AddMoney(-_selectedWeaponData.Price);
            _playerInventoryController.AddWeapon(_selectedWeaponData);

            OnWeaponGUIPointerDown(_selectedWeaponData);
        }

        private void OnEquipButtonClicked()
        {
            if (_selectedWeaponData == null)
                return;

            if (!_playerInventoryController.OwnedWeapons.Contains(_selectedWeaponData))
                return;

            FindObjectOfType<WeaponController>().SwitchWeapon(_selectedWeaponData);
            
            OnWeaponGUIPointerDown(_selectedWeaponData);
        }
    }
}