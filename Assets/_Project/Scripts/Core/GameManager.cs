using System;
using Cysharp.Threading.Tasks;
using gishadev.fort.Enemy;
using gishadev.fort.Money;
using gishadev.fort.Player;
using gishadev.fort.World;
using gishadev.tools.Events;
using UnityEngine;
using Zenject;

namespace gishadev.fort.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private DefaultEventChannelSO winChannelEvent;

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
            winChannelEvent.ChangedValue += OnLeaveIslandClicked;
        }

        private void OnDisable()
        {
            _player.PlayerDied -= OnPlayerDied;
            winChannelEvent.ChangedValue -= OnLeaveIslandClicked;
        }

        private void OnLeaveIslandClicked(StringWrapper stringWrapper)
        {
            Won?.Invoke();
            Debug.Log("Win detected, moving to next scene.");
        }

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