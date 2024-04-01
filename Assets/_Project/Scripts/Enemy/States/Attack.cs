using System.Threading;
using Cysharp.Threading.Tasks;
using gishadev.fort.Core;
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
            _attackCts.RegisterRaiseCancelOnDestroy(_enemyBase.gameObject);
            _enemyBase.EnemyMovement.Stop();
            
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
                var player = _enemyBase.GetPlayer();
                await UniTask.WaitForSeconds(_enemyBase.AttackDelay, cancellationToken: _attackCts.Token)
                    .SuppressCancellationThrow();

                if (_attackCts.Token.IsCancellationRequested)
                    return;

                if (player != null)
                {
                    _enemyBase.Animator.SetTrigger(Constants.HASH_ATTACK);
                    player.TakeDamage(_enemyBase.AttackDamage, _enemyBase.transform.forward * 10f);
                }
            }
        }
    }
}