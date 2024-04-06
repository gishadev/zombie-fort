using System.Collections.Generic;
using gishadev.fort.Money;
using gishadev.fort.Player;
using gishadev.fort.Weapons;
using gishadev.fort.World.Shop;
using gishadev.tools.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace gishadev.fort.GUI
{
    public partial class ArsenalPopupPage : Page
    {
        [SerializeField, TabGroup("Buttons")] private Button buyButton, equipButton, rightArrowButton, leftArrowButton;
        [SerializeField, TabGroup("TMP")] private TMP_Text selectedWeaponNameTMP, selectedWeaponPriceTMP;

        [Inject] private IMoneyController _moneyController;
        [Inject] private IPlayerInventoryController _playerInventoryController;
        [Inject] private ShopDataSO _shopDataSO;
        
        private WeaponDataSO SelectedWeaponData => _shopDataSO.BuyableWeapons[_selectedIndex];

        private int _selectedIndex;

        private void Awake()
        {
            buyButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            buyButton.onClick.AddListener(OnBuyButtonClicked);
            equipButton.onClick.AddListener(OnEquipButtonClicked);
            rightArrowButton.onClick.AddListener(() => SelectNextWeapon(+1));
            leftArrowButton.onClick.AddListener(() => SelectNextWeapon(-1));
            
            SelectWeapon(0);
        }

        private void OnDisable()
        {
            buyButton.onClick.RemoveAllListeners();
            equipButton.onClick.RemoveAllListeners();
            rightArrowButton.onClick.RemoveAllListeners();
            leftArrowButton.onClick.RemoveAllListeners();
        }

        private void SelectWeapon(int index)
        {
            _selectedIndex = index;
            UpdateWeaponDataGUI(SelectedWeaponData);
        }

        private void SelectNextWeapon(int iterateOperation)
        {
            _selectedIndex += iterateOperation;
            if (_selectedIndex < 0)
                _selectedIndex = _shopDataSO.BuyableWeapons.Length - 1;
            else if (_selectedIndex >= _shopDataSO.BuyableWeapons.Length)
                _selectedIndex = 0;

            SelectWeapon(_selectedIndex);
        }

        private void UpdateWeaponDataGUI(WeaponDataSO weaponDataSO)
        {
            selectedWeaponNameTMP.text = weaponDataSO.name;
            selectedWeaponPriceTMP.text = weaponDataSO.Price.ToString();

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
    }

    public partial class ArsenalPopupPage
    {
        public void OnBackButtonClicked() => FindObjectOfType<Arsenal>().CloseArsenal();

        private void OnBuyButtonClicked()
        {
            if (SelectedWeaponData == null)
                return;

            if (_moneyController.MoneyCount < SelectedWeaponData.Price)
                return;

            _moneyController.AddMoney(-SelectedWeaponData.Price);
            _playerInventoryController.AddWeapon(SelectedWeaponData);

            UpdateWeaponDataGUI(SelectedWeaponData);
        }

        private void OnEquipButtonClicked()
        {
            if (SelectedWeaponData == null)
                return;

            if (!_playerInventoryController.OwnedWeapons.Contains(SelectedWeaponData))
                return;

            FindObjectOfType<WeaponController>().SwitchWeapon(SelectedWeaponData);
            UpdateWeaponDataGUI(SelectedWeaponData);
        }
    }
}