using System.Threading;
using Cysharp.Threading.Tasks;
using gishadev.fort.Enemy;
using gishadev.tools.StateMachine;

namespace gishadev.fort.Player.PlayerStates
{
    public class MeleeAutoAttack : IState
    {
        private readonly PlayerAutoAttack _autoAttack;
        private readonly WeaponController _weaponController;

        private EnemyBase _nearestEnemy;
        private CancellationTokenSource _attackCTS;

        public MeleeAutoAttack(PlayerAutoAttack autoAttack, WeaponController weaponController)
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
            MeleeAttackingAsync();
        }

        public void OnExit()
        {
            _attackCTS.Cancel();
        }

        private async void MeleeAttackingAsync()
        {
            while (_nearestEnemy != null && !_attackCTS.Token.IsCancellationRequested)
            {
                _weaponController.MeleeAttack(_nearestEnemy);
                await UniTask
                    .WaitForSeconds(_weaponController.EquippedMelee.MeleeDataSO.AttackDelay,
                        cancellationToken: _attackCTS.Token)
                    .SuppressCancellationThrow();
            }
        }
    }
}