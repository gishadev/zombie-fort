using gishadev.fort.Money;
using gishadev.tools.StateMachine;
using UnityEngine;

namespace gishadev.fort.Enemy
{
    public class Dead : IState
    {
        private readonly EnemyBase _enemyBase;
        private readonly IMoneySpawner _moneySpawner;

        public Dead(EnemyBase enemyBase, IMoneySpawner moneySpawner)
        {
            _enemyBase = enemyBase;
            _moneySpawner = moneySpawner;
        }

        public void Tick()
        {
        }

        public void OnEnter()
        {
            _moneySpawner.BurstSpawnMoney(_enemyBase.transform.position, 5, 3f);
            Object.Destroy(_enemyBase.gameObject);
        }

        public void OnExit()
        {
        }
    }
}