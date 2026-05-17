using System.Threading;
using Cysharp.Threading.Tasks;
using gishadev.fort.Core;
using gishadev.tools.StateMachine;
using UnityEngine;

namespace gishadev.fort.Player.PlayerStates
{
    public class FirearmAutoAttack : IState
    {
        private readonly PlayerAutoAttack _autoAttack;
        private readonly WeaponController _weaponController;

        private IAutoAttackable _nearestAttackable;
        private CancellationTokenSource _attackCTS;

        public FirearmAutoAttack(PlayerAutoAttack autoAttack, WeaponController weaponController)
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
            Debug.Log("Firearm Auto Attack");
            _attackCTS = new CancellationTokenSource();
            _nearestAttackable = _autoAttack.GetNearestAttackable();
            _weaponController.SetAiming(true);
            
            FirearmAttackingAsync();
        }

        public void OnExit()
        {
            _weaponController.FirearmCancel();
            _attackCTS.Cancel();
            _weaponController.SetAiming(false);
        }

        private async void FirearmAttackingAsync()
        {
            while (_nearestAttackable != null && !_attackCTS.Token.IsCancellationRequested)
            {
                if (!IsViewObstructed())
                    _weaponController.FirearmAttack(_nearestAttackable);

                await UniTask
                    .WaitForSeconds(_weaponController.EquippedGun.GunDataSO.ShootDelay,
                        cancellationToken: _attackCTS.Token)
                    .SuppressCancellationThrow();
            }
        }

        private bool IsViewObstructed()
        {
            var ray = new Ray(_weaponController.EquippedGun.ShootPoint.position,
                _nearestAttackable.transform.position - _weaponController.EquippedGun.ShootPoint.position);
            return Physics.Raycast(ray, out var hitInfo) &&
                   !hitInfo.collider.CompareTag(Constants.TAG_NAME_ATTACKABLE);
        }
    }
}