using System;
using Cysharp.Threading.Tasks;
using gishadev.fort.Enemy;
using gishadev.fort.Money;
using gishadev.fort.Player;
using gishadev.fort.World;
using UnityEngine;
using Zenject;

namespace gishadev.fort.Core
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private IEnemySpawner _enemySpawner;
        [Inject] private IMoneyController _moneyController;

        public static event Action Won;

        private Player.Player _player;
        private PlayerSpawnpoint _playerSpawnpoint;

        private void Awake()
        {
            _player = FindObjectOfType<Player.Player>();
            _playerSpawnpoint = FindObjectOfType<Location>().PlayerSpawnpoint;
        }

        private void Start()
        {
            _enemySpawner.Init();
            _moneyController.Init();
        }

        private void OnEnable()
        {
            _player.PlayerDied += OnPlayerDied;
            Helipad.HelipadSpawned += OnHelipadSpawned;
        }

        private void OnDisable()
        {
            _player.PlayerDied -= OnPlayerDied;
            Helipad.HelipadSpawned -= OnHelipadSpawned;
        }

        private void Win()
        {
            Debug.Log("Win");
            Won?.Invoke();
        }

        private void OnHelipadSpawned(Helipad helipad) => Win();

        private async void OnPlayerDied()
        {
            _player.gameObject.SetActive(false);
            _player.transform.position = _playerSpawnpoint.transform.position;
            await UniTask.WaitForSeconds(0.5f);
            _player.gameObject.SetActive(true);
            _player.Heal(1f);
        }
    }
}