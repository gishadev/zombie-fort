using gishadev.tools.StateMachine;
using UnityEngine;

namespace gishadev.fort.Enemy
{
    public class Wander : IState
    {
        private EnemyMovement EnemyMovement => _enemyBase.EnemyMovement;
        private readonly EnemyBase _enemyBase;
        private Vector3 _randomTargetPoint;
        private readonly float _wanderRadius = 5f;

        public Wander(EnemyBase enemyBase)
        {
            _enemyBase = enemyBase;
        }

        public void Tick()
        {
            if (EnemyMovement.DistanceToDestination < 0.5f)
                EnemyMovement.SetDestination(GetRandomPoint());
        }

        public void OnEnter()
        {
            EnemyMovement.SetDestination(GetRandomPoint());
        }

        public void OnExit()
        {
            EnemyMovement.Stop();
        }

        private Vector3 GetRandomPoint()
        {
            var randPoint = Random.onUnitSphere * _wanderRadius;
            randPoint.y = EnemyMovement.transform.position.y;
            return EnemyMovement.transform.position + randPoint;
        }
    }
}