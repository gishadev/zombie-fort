using System.Threading;
using Cysharp.Threading.Tasks;
using gishadev.fort.Core;
using UnityEngine;

namespace gishadev.fort.Weapons
{
    public class Gun : Firearm
    {
        [SerializeField] private Transform shootPoint;
        [SerializeField] private LineRenderer lineRenderer;

        [SerializeField] private bool isAutomatic;
        [SerializeField] private float shootDelay = 0.1f;
        [SerializeField] private float shootForce = 5f;
        [SerializeField] private int damage = 5;

        private CancellationTokenSource _autoCts;

        public override void OnAttackPerformed()
        {
            if (isAutomatic)
            {
                _autoCts = new CancellationTokenSource();
                AutomaticShootAsync();
            }
            else
                Shoot();
        }

        public override void OnAttackCanceled()
        {
            if (isAutomatic)
                _autoCts.Cancel();
        }

        protected override void Shoot()
        {
            base.Shoot();
            if (Physics.Raycast(shootPoint.position, shootPoint.forward, out var hit, 100))
            {
                var damageable = hit.collider.GetComponent<IDamageable>();
                damageable?.TakeDamage(damage, shootPoint.forward * shootForce);

                LineEffectAsync(shootPoint.position, hit.point);
            }
            else
                LineEffectAsync(shootPoint.position, shootPoint.position + shootPoint.forward * 100);

            Debug.DrawRay(shootPoint.position, shootPoint.forward * 100, Color.red, 1f);
        }

        private async void AutomaticShootAsync()
        {
            while (!_autoCts.IsCancellationRequested)
            {
                Shoot();
                await UniTask.WaitForSeconds(shootDelay, cancellationToken: _autoCts.Token).SuppressCancellationThrow();
            }
        }

        private async void LineEffectAsync(Vector3 start, Vector3 end)
        {
            lineRenderer.enabled = true;

            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);

            await UniTask.WaitForSeconds(0.05f);
            lineRenderer.enabled = false;
        }
    }
}