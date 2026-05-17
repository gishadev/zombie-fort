using System.Threading;
using Cysharp.Threading.Tasks;
using gishadev.fort.Core;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace gishadev.fort.Enemy
{
    public class EnemySpawner : IEnemySpawner
    {
        [Inject] private DiContainer _diContainer;
        [Inject] private GameDataSO _gameDataSO;

        private EnemySpawnPoint[] _spawnPoints;
        private CancellationTokenSource _spawningCTS;

        private Transform _parent;

        public void Init()
        {
            _parent = new GameObject("Enemies").transform;
            StartSpawning();
        }

        public void StartSpawning()
        {
            _spawnPoints = Object.FindObjectsOfType<EnemySpawnPoint>();
            _spawningCTS = new CancellationTokenSource();
            SpawnEnemiesAsync(10);
        }

        public void StopSpawning()
        {
            _spawningCTS.Cancel();
        }

        private async void SpawnEnemiesAsync(int amountToSpawn)
        {
            while (true)
            {
                var enemies = Object.FindObjectsOfType<EnemyBase>();
                if (enemies.Length < 1)
                {
                    for (int i = 0; i < amountToSpawn && !_spawningCTS.IsCancellationRequested; i++)
                    {
                        SpawnEnemyAtRandomSpawnPoint();
                        await UniTask.WaitForSeconds(_gameDataSO.EnemySpawnDelay,
                            cancellationToken: _spawningCTS.Token);
                    }
                }

                await UniTask.WaitForSeconds(_gameDataSO.WaveDelay, cancellationToken: _spawningCTS.Token);
            }
        }

        private void SpawnEnemyAtRandomSpawnPoint()
        {
            var spawnPosition = _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform.position;
            _diContainer.InstantiatePrefab(_gameDataSO.EnemyPrefab, spawnPosition, Quaternion.identity, _parent);
        }
    }
}