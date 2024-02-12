using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace gishadev.fort.Player
{
    public class MoneyCoinMagnet : MonoBehaviour
    {
        [SerializeField] private float magnetizeRadius = 5f;
        [SerializeField] private float checkInterval = 0.1f;

        private CancellationTokenSource _cts;

        private void Awake()
        {
            _cts = new CancellationTokenSource();
            _cts.RegisterRaiseCancelOnDestroy(gameObject);
        }

        private void Start()
        {
            MagnetCheckAsync();
        }

        private async void MagnetCheckAsync()
        {
            if (_cts.IsCancellationRequested)
                return;

            Collider[] colliders = Physics.OverlapSphere(transform.position, magnetizeRadius);
            foreach (Collider collider in colliders)
                if (collider.TryGetComponent(out Money.MoneyCoin moneyCoin) && !moneyCoin.IsMagnetized)
                    moneyCoin.JumpTo(transform);

            await UniTask.WaitForSeconds(checkInterval, cancellationToken: _cts.Token)
                .ContinueWith(MagnetCheckAsync)
                .SuppressCancellationThrow();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, magnetizeRadius);
        }
    }
}