using System;
using System.Collections.Generic;
using System.Linq;
using gishadev.fort.Enemy;
using gishadev.tools.StateMachine;
using UnityEngine;

namespace gishadev.fort.Player
{
    public class PlayerAutoAttack : MonoBehaviour
    {
        [SerializeField] private float meleeMaxAutoAttackRange = 1f;
        [SerializeField] private float firearmMaxAutoAttackRange = 10f;

        private List<EnemyBase> _enemiesInRange;
        private StateMachine _stateMachine;

        private void InitStateMachine()
        {
            _stateMachine = new StateMachine();

            var noAutoAttack = new PlayerStates.NoAutoAttack();
            var meleeAutoAttack = new PlayerStates.MeleeAutoAttack();
            var firearmAutoAttack = new PlayerStates.FirearmAutoAttack();

            Aat(noAutoAttack, () => !IsEnemyInRange(meleeMaxAutoAttackRange) && !IsEnemyInRange(firearmMaxAutoAttackRange));
            Aat(meleeAutoAttack, () => IsEnemyInRange(meleeMaxAutoAttackRange) && IsEnemyInRange(firearmMaxAutoAttackRange));
            Aat(firearmAutoAttack, () => !IsEnemyInRange(meleeMaxAutoAttackRange) && IsEnemyInRange(firearmMaxAutoAttackRange));

            _stateMachine.SetState(noAutoAttack);

            bool IsEnemyInRange(float range) => _enemiesInRange.Any(x =>
                Vector3.Distance(transform.position, x.transform.position) < range);

            void At(IState from, IState to, Func<bool> cond) => _stateMachine.AddTransition(from, to, cond);
            void Aat(IState to, Func<bool> cond) => _stateMachine.AddAnyTransition(to, cond);
        }

        private void Start() => InitStateMachine();

        private void Update()
        {
            _enemiesInRange = GetAllEnemiesInRange(firearmMaxAutoAttackRange + 1);
            _stateMachine.Tick();
        }

        private List<EnemyBase> GetAllEnemiesInRange(float range)
        {
            var enemies = FindObjectsOfType<EnemyBase>();
            var enemiesInRange = new List<EnemyBase>();
            enemiesInRange.AddRange(enemies.Where(x =>
                Vector3.Distance(transform.position, x.transform.position) < range));

            return enemiesInRange;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, meleeMaxAutoAttackRange);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, firearmMaxAutoAttackRange);
        }
    }
}