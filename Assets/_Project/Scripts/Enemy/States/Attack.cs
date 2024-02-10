using System.Threading;
using Cysharp.Threading.Tasks;
using gishadev.tools.StateMachine;

namespace gishadev.fort.Enemy
{
    public class Attack : IState
    {
        private readonly EnemyBase _enemyBase;

        private CancellationTokenSource _attackCts;

        public Attack(EnemyBase enemyBase)
        {
            _enemyBase = enemyBase;
        }

        public void Tick()
        {
        }

        public void OnEnter()
        {
            _attackCts = new CancellationTokenSource();
            AttackPlayerAsync();
        }

        public void OnExit()
        {
            _attackCts.Cancel();
        }

        private async void AttackPlayerAsync()
        {
            while (!_attackCts.Token.IsCancellationRequested)
            {
                await UniTask.WaitForSeconds(_enemyBase.AttackDelay, cancellationToken: _attackCts.Token)
                    .SuppressCancellationThrow();
                _enemyBase.GetPlayer().TakeDamage(_enemyBase.AttackDamage, _enemyBase.transform.forward * 5f);
            }
        }
    }
}