using System;
using gishadev.fort.Enemy;
using gishadev.fort.Money;
using gishadev.fort.World;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;


namespace gishadev.fort.Core
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private IEnemySpawner _enemySpawner;
        [Inject] private IMoneyController _moneyController;
        
        public static event Action Won;
        public static event Action Lost;

        private Player.Player _player;

        private void Awake()
        {
            _player = FindObjectOfType<Player.Player>();
        }

        private void Start()
        {
            _enemySpawner.Init();
            _moneyController.Init();
        }

        private void OnEnable()
        {
            _player.HealthChanged += OnPlayerHealthChanged;
            Helipad.HelipadSpawned += OnHelipadSpawned;
        }

        private void OnDisable()
        {
            _player.HealthChanged -= OnPlayerHealthChanged;
            Helipad.HelipadSpawned -= OnHelipadSpawned;
        }

        public static void RestartGame()
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        private void Win()
        {
            Debug.Log("Win");
            Won?.Invoke();
        }

        private void Lose()
        {
            Debug.Log("Lose");
            Lost?.Invoke();
        }

        private void OnPlayerHealthChanged(int health)
        {
            if (health <= 0) Lose();
        }

        private void OnHelipadSpawned(Helipad helipad) => Win();
    }
}