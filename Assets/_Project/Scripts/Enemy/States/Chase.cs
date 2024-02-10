using System.Threading;
using Cysharp.Threading.Tasks;
using gishadev.tools.StateMachine;

namespace gishadev.fort.Enemy
{
    public class Chase : IState
    {
        private readonly EnemyMovement _enemyMovement;

        private Player.Player _player;

        private CancellationTokenSource _cts;

        public Chase(EnemyBase enemyBase, EnemyMovement enemyMovement)
        {
            _enemyMovement = enemyMovement;
            _player = enemyBase.GetPlayer();
        }

        public async void Tick()
        {
            if (_cts.IsCancellationRequested && _player != null)
                return;
            await UniTask.WaitForSeconds(0.1f, cancellationToken: _cts.Token).SuppressCancellationThrow();
            if (_cts.IsCancellationRequested)
                return;

            _enemyMovement.SetDestination(_player.transform.position);
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