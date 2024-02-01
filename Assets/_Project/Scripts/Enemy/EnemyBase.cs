using gishadev.fort.Core;
using gishadev.tools.StateMachine;
using UnityEngine;

namespace gishadev.fort.Enemy
{
    public abstract class EnemyBase : MonoBehaviour, IDamageable
    {
        protected StateMachine StateMachine { get; set; }
        protected EnemyMovement EnemyMovement { get; private set; }
        protected bool IsDead { get; set; }

        protected virtual void Awake()
        {
            EnemyMovement = GetComponent<EnemyMovement>();
            InitStateMachine();
        }

        protected virtual void Update()
        {
            StateMachine.Tick();
        }

        public abstract void InitStateMachine();
        public abstract void TakeDamage(int damage, Vector3 hitDirection);

        public Player.Player GetPlayer() => FindObjectOfType<Player.Player>();
    }
}