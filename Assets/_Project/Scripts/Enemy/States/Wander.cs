using gishadev.tools.StateMachine;
using UnityEngine;

namespace gishadev.fort.Enemy
{
    public class Wander : IState
    {
        private readonly EnemyMovement _enemyMovement;
        private Vector3 _randomTargetPoint;
        private float _wanderRadius = 5f;

        public Wander(EnemyMovement enemyMovement)
        {
            _enemyMovement = enemyMovement;
        }

        public void Tick()
        {
            if (_enemyMovement.DistanceToDestination < 0.5f)
                _enemyMovement.SetDestination(GetRandomPoint());
        }

        public void OnEnter()
        {
            _enemyMovement.SetDestination(GetRandomPoint());
        }

        public void OnExit()
        {
            _enemyMovement.Stop();
        }

        private Vector3 GetRandomPoint()
        {
            var randPoint = Random.onUnitSphere * _wanderRadius;
            randPoint.y = _enemyMovement.transform.position.y;
            return _enemyMovement.transform.position + randPoint;
        }
    }
}