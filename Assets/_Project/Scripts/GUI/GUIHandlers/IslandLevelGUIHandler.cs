using System;
using gishadev.fort.Level;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace gishadev.fort.GUI
{
    public class IslandLevelGUIHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelCountTMP;
        [SerializeField] private Slider levelSlider;

        [Inject] private IIslandLevelController _islandLevelController;

        private void OnEnable()
        {
            _islandLevelController.LevelChanged += OnLevelChanged;
            _islandLevelController.ProgressChanged += OnLevelProgressChanged;
        }

        private void OnDisable()
        {
            _islandLevelController.LevelChanged -= OnLevelChanged;
            _islandLevelController.ProgressChanged -= OnLevelProgressChanged;
        }

        private void OnLevelChanged(int newLevel) => levelCountTMP.text = newLevel.ToString();
        private void OnLevelProgressChanged(float progress) => levelSlider.value = progress;
    }
}