using gishadev.fort.Core;
using UnityEngine;
using Zenject;

namespace gishadev.fort.Money
{
    public class MoneySpawner : IMoneySpawner
    {
        [Inject] private GameDataSO _gameDataSO;
        [Inject] private DiContainer _diContainer;


        public void BurstSpawnMoney(Vector3 position, int count, float burstForce)
        {
            for (int i = 0; i < count; i++)
            {
                var moneyObj =
                    _diContainer.InstantiatePrefab(_gameDataSO.MoneyPrefab, position, Quaternion.identity, null);
                var moneyRb = moneyObj.GetComponent<Rigidbody>();
                var randomDirection = Random.insideUnitSphere;
                moneyRb.AddForce(randomDirection * burstForce, ForceMode.Impulse);
            }
        }
    }
}