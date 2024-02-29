using System.Threading;
using Cysharp.Threading.Tasks;
using gishadev.fort.Enemy;
using gishadev.tools.StateMachine;

namespace gishadev.fort.Player.PlayerStates
{
    public class FirearmAutoAttack : IState
    {
        private readonly PlayerAutoAttack _autoAttack;
        private readonly WeaponController _weaponController;

        private EnemyBase _nearestEnemy;
        private CancellationTokenSource _attackCTS;

        public FirearmAutoAttack(PlayerAutoAttack autoAttack, WeaponController weaponController)
        {
            _autoAttack = autoAttack;
            _weaponController = weaponController;
        }

        public void Tick()
        {
            _nearestEnemy = _autoAttack.GetNearestEnemy();
            _weaponController.RotateTowardsTarget(_nearestEnemy.transform);
        }

        public void OnEnter()
        {
            _attackCTS = new CancellationTokenSource();
            FirearmAttackingAsync();
        }

        public void OnExit()
        {
            _weaponController.FirearmCancel();
            _attackCTS.Cancel();
        }

        private async void FirearmAttackingAsync()
        {
            while (_nearestEnemy != null && !_attackCTS.Token.IsCancellationRequested)
            {
                _weaponController.FirearmAttack(_nearestEnemy);
                await UniTask
                    .WaitForSeconds(_weaponController.EquippedGun.GunDataSO.ShootDelay,
                        cancellationToken: _attackCTS.Token)
                    .SuppressCancellationThrow();
            }
        }
    }
}