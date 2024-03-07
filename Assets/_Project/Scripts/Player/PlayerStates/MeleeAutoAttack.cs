using System.Threading;
using Cysharp.Threading.Tasks;
using gishadev.fort.Core;
using gishadev.tools.StateMachine;
using UnityEngine;

namespace gishadev.fort.Player.PlayerStates
{
    public class MeleeAutoAttack : IState
    {
        private readonly PlayerAutoAttack _autoAttack;
        private readonly WeaponController _weaponController;

        private IAutoAttackable _nearestAttackable;
        private CancellationTokenSource _attackCTS;

        public MeleeAutoAttack(PlayerAutoAttack autoAttack, WeaponController weaponController)
        {
            _autoAttack = autoAttack;
            _weaponController = weaponController;
        }

        public void Tick()
        {
            _nearestAttackable = _autoAttack.GetNearestAttackable();
            _weaponController.RotateTowardsTarget(_nearestAttackable.transform);
        }

        public void OnEnter()
        {
            Debug.Log("Melee Auto Attack");
            
            _attackCTS = new CancellationTokenSource();
            _nearestAttackable = _autoAttack.GetNearestAttackable();
            
            MeleeAttackingAsync();
        }

        public void OnExit()
        {
            _attackCTS.Cancel();
        }

        private async void MeleeAttackingAsync()
        {
            while (_nearestAttackable != null && !_attackCTS.Token.IsCancellationRequested)
            {
                _weaponController.MeleeAttack(_nearestAttackable);
                await UniTask
                    .WaitForSeconds(_weaponController.EquippedMelee.MeleeDataSO.AttackDelay,
                        cancellationToken: _attackCTS.Token)
                    .SuppressCancellationThrow();
            }
        }
    }
}