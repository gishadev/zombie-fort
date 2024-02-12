using System;
using gishadev.fort.Money;
using gishadev.tools.StateMachine;
using UnityEngine;
using Zenject;

namespace gishadev.fort.Enemy
{
    public class Zombie : EnemyBase
    {
        [SerializeField] private float attackRange = 0.2f;
        
        [Inject] private IMoneySpawner _moneySpawner;

        public override event Action<int> HealthChanged;

        protected override void InitStateMachine()
        {
            StateMachine = new StateMachine();

            var idle = new Idle();
            var chase = new Chase(this, EnemyMovement);
            var attack = new Attack(this);
            var dead = new Dead(this, _moneySpawner);

            At(idle, chase, PlayerExists);
            
            At(chase, attack, IsPlayerInAttackRange);
            At(chase, idle, () => !PlayerExists());
            
            At(attack, idle, () => !IsPlayerInAttackRange());
            
            Aat(dead, () => IsDead);

            StateMachine.SetState(idle);

            bool IsPlayerInAttackRange() => PlayerExists() && Vector3.Distance(transform.position, GetPlayer().transform.position) < attackRange;
            bool PlayerExists() => GetPlayer() != null;
            
            void At(IState from, IState to, Func<bool> cond) => StateMachine.AddTransition(from, to, cond);
            void Aat(IState to, Func<bool> cond) => StateMachine.AddAnyTransition(to, cond);
        }

        public override void TakeDamage(int damage, Vector3 hitForce)
        {
            Health -= damage;

            if (Health <= 0)
                IsDead = true;
            else
                EnemyMovement.KnockBack(hitForce);
            
            HealthChanged?.Invoke(Health);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}