using System;
using gishadev.fort.Enemy;
using gishadev.tools.SceneLoading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;


namespace gishadev.fort.Core
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private IEnemySpawner _enemySpawner;

        public static event Action Won;
        public static event Action Lost;
        
        private Player.Player _player;

        private void Awake()
        {
            _player = FindObjectOfType<Player.Player>();
        }

        private void Start()
        {
            _enemySpawner.StartSpawning();
        }

        private void OnEnable() => _player.HealthChanged += OnPlayerHealthChanged;

        private void OnDisable() => _player.HealthChanged -= OnPlayerHealthChanged;

        public static void RestartGame()
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
        
        private void Win() => Won?.Invoke();
        private void Lose() => Lost?.Invoke();

        private void OnPlayerHealthChanged(int health)
        {
            if (health <= 0) Lose();
        }
    }
}