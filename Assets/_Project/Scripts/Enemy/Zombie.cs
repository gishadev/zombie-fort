using System;
using gishadev.tools.StateMachine;
using UnityEngine;

namespace gishadev.fort.Enemy
{
    public class Zombie : EnemyBase
    {
        public override void InitStateMachine()
        {
            StateMachine = new StateMachine();

            var idle = new Idle();
            var chase = new Chase(this, EnemyMovement);
            var attack = new Attack();
            var dead = new Dead(this);

            At(idle, chase, () => true);

            Aat(dead, () => IsDead);

            StateMachine.SetState(idle);

            void At(IState from, IState to, Func<bool> cond) => StateMachine.AddTransition(from, to, cond);
            void Aat(IState to, Func<bool> cond) => StateMachine.AddAnyTransition(to, cond);
        }

        public override void TakeDamage(int damage, Vector3 hitDirection)
        {
            IsDead = true;
        }
    }
}