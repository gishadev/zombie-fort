using System;
using System.Collections.Generic;
using System.Linq;
using gishadev.fort.Core;
using gishadev.tools.StateMachine;
using UnityEngine;

namespace gishadev.fort.Player
{
    [RequireComponent(typeof(WeaponController), typeof(PlayerCharacterMovement))]
    public class PlayerAutoAttack : MonoBehaviour
    {
        [SerializeField] private float meleeMaxAutoAttackRange = 1f;
        [SerializeField] private float firearmMaxAutoAttackRange = 10f;

        private List<IAutoAttackable> _autoAttackablesInRange;
        private StateMachine _stateMachine;
        private WeaponController _weaponController;
        private PlayerCharacterMovement _playerCharacterMovement;

        private void Awake()
        {
            _playerCharacterMovement = GetComponent<PlayerCharacterMovement>();
            _weaponController = GetComponent<WeaponController>();
        }

        private void Start() => InitStateMachine();

        private void Update()
        {
            _autoAttackablesInRange = GetAllAttackablesInRange(firearmMaxAutoAttackRange + 1);
            _stateMachine.Tick();
        }

        public IAutoAttackable GetNearestAttackable()
        {
            var enemies = GetAllAttackablesInRange(firearmMaxAutoAttackRange);
            if (enemies.Count == 0)
                return null;

            var nearestEnemy = enemies[0];
            var nearestDistance = (transform.position - nearestEnemy.transform.position).sqrMagnitude;

            foreach (var enemy in enemies)
            {
                var distance = (transform.position - enemy.transform.position).sqrMagnitude;
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }

            return nearestEnemy;
        }

        private void InitStateMachine()
        {
            _stateMachine = new StateMachine();

            var noAutoAttack = new PlayerStates.NoAutoAttack(_playerCharacterMovement);
            var meleeAutoAttack = new PlayerStates.MeleeAutoAttack(this, _weaponController);
            var firearmAutoAttack = new PlayerStates.FirearmAutoAttack(this, _weaponController);

            Aat(noAutoAttack,
                () => !IsEnemyInRange(meleeMaxAutoAttackRange) && IsEnemyInRange(firearmMaxAutoAttackRange) &&
                      _weaponController.EquippedGun == null);
            Aat(noAutoAttack,
                () => !IsEnemyInRange(meleeMaxAutoAttackRange) && !IsEnemyInRange(firearmMaxAutoAttackRange) &&
                      _weaponController.EquippedGun != null);
            
            Aat(meleeAutoAttack,
                () => IsEnemyInRange(meleeMaxAutoAttackRange) && IsEnemyInRange(firearmMaxAutoAttackRange));
            Aat(firearmAutoAttack,
                () => !IsEnemyInRange(meleeMaxAutoAttackRange) && IsEnemyInRange(firearmMaxAutoAttackRange) &&
                      _weaponController.EquippedGun != null);

            _stateMachine.SetState(noAutoAttack);

            bool IsEnemyInRange(float range) => _autoAttackablesInRange.Any(x =>
                Vector3.Distance(transform.position, x.transform.position) < range);

            void At(IState from, IState to, Func<bool> cond) => _stateMachine.AddTransition(from, to, cond);
            void Aat(IState to, Func<bool> cond) => _stateMachine.AddAnyTransition(to, cond);
        }

        private List<IAutoAttackable> GetAllAttackablesInRange(float range)
        {
            var autoAttackables = FindObjectsOfType<MonoBehaviour>().OfType<IAutoAttackable>();
            var autoAttackablesInRange = new List<IAutoAttackable>();
            autoAttackablesInRange.AddRange(autoAttackables.Where(x =>
                Vector3.Distance(transform.position, x.transform.position) < range));

            return autoAttackablesInRange;
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