using System.Threading;
using Cysharp.Threading.Tasks;
using gishadev.tools.StateMachine;

namespace gishadev.fort.Enemy
{
    public class Chase : IState
    {
        private readonly EnemyBase _enemyBase;
        private Player.Player _player;

        private CancellationTokenSource _cts;

        public Chase(EnemyBase enemyBase)
        {
            _enemyBase = enemyBase;
            _player = enemyBase.GetPlayer();
        }

        public async void Tick()
        {
            if (_cts.IsCancellationRequested || _player == null)
                return;
            
            await UniTask.WaitForSeconds(0.1f, cancellationToken: _cts.Token).SuppressCancellationThrow();
            if (_cts.IsCancellationRequested)
                return;

            _enemyBase.EnemyMovement.SetDestination(_player.transform.position);
        }

        public void OnEnter()
        {
            _cts = new CancellationTokenSource();
        }

        public void OnExit()
        {
            _cts.Cancel();
        }
    }
}