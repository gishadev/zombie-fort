using System;
using gishadev.tools.StateMachine;
using UnityEngine;

namespace gishadev.fort.Enemy
{
    public class Zombie : EnemyBase
    {
        [SerializeField] private float attackRange = 0.2f;
        [SerializeField] private float chaseRange = 2f;

        public override event Action<int> HealthChanged;

        protected override void InitStateMachine()
        {
            StateMachine = new StateMachine();

            var idle = new Idle();
            var chase = new Chase(this, EnemyMovement);
            var attack = new Attack(this);
            var wander = new Wander(EnemyMovement);
            var dead = new Dead(this);

            At(idle, wander, PlayerExists);

            At(chase, attack, () => IsPlayerInRange(attackRange));
            At(chase, wander, () => !PlayerExists() || !IsPlayerInRange(chaseRange));

            At(attack, chase, () => !IsPlayerInRange(attackRange) && IsPlayerInRange(chaseRange));
            At(attack, wander, () => !IsPlayerInRange(attackRange) && !IsPlayerInRange(chaseRange));

            At(wander, chase, () => IsPlayerInRange(chaseRange));
            
            Aat(dead, () => IsDead);

            StateMachine.SetState(idle);

            bool IsPlayerInRange(float range) => PlayerExists() &&
                                           Vector3.Distance(transform.position, GetPlayer().transform.position) <
                                           range;

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

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }
}