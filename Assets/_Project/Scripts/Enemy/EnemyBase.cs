using System;
using gishadev.fort.Core;
using gishadev.tools.StateMachine;
using UnityEngine;

namespace gishadev.fort.Enemy
{
    public abstract class EnemyBase : MonoBehaviour, IAutoAttackable
    {
        [SerializeField] private int startHealth = 5;
        [SerializeField] private float attackDelay = 1f;
        [SerializeField] private int attackDamage = 10;
        
        public abstract event Action<int> HealthChanged;
        public Animator Animator { get; private set; }
        public EnemyMovement EnemyMovement { get; private set; }
        public int Health { get; protected set; }
        public float AttackDelay => attackDelay;
        public int AttackDamage => attackDamage;
        
        protected StateMachine StateMachine { get; set; }
        protected bool IsDead { get; set; }

        protected virtual void Awake()
        {
            EnemyMovement = GetComponent<EnemyMovement>();
            Animator = GetComponentInChildren<Animator>();
            InitStateMachine();
            
            Health = startHealth;
        }

        protected virtual void Update()
        {
            StateMachine.Tick();
        }

        protected abstract void InitStateMachine();
        public abstract void TakeDamage(int damage, Vector3 hitForce);
        public Player.Player GetPlayer() => FindObjectOfType<Player.Player>();
    }
}