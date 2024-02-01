using gishadev.tools.StateMachine;
using UnityEngine;

namespace gishadev.fort.Enemy
{
    public class Dead : IState
    {
        private readonly EnemyBase _enemyBase;

        public Dead(EnemyBase enemyBase)
        {
            _enemyBase = enemyBase;
        }

        public void Tick()
        {
        }

        public void OnEnter()
        {
            Object.Destroy(_enemyBase.gameObject);
        }

        public void OnExit()
        {
        }
    }
}