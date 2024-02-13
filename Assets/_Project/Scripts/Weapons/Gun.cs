using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using gishadev.fort.Core;
using UnityEngine;

namespace gishadev.fort.Weapons
{
    public class Gun : Weapon
    {
        [SerializeField] private Transform shootPoint;
        [SerializeField] private LineRenderer lineRenderer;

        [SerializeField] private bool isAutomatic;
        [SerializeField] private float shootDelay = 0.1f;
        [SerializeField] private float shootForce = 5f;
        [SerializeField] private int damage = 5;
        [Space] [SerializeField] private bool isInfinityMagazines;

        [SerializeField] private int startAmmoInMagazine = 30;
        [SerializeField] private int startMagazinesCount = 3;

        [SerializeField] private float reloadTime = 1f;

        public static event Action<Gun> OutOfAmmo;
        public static event Action<Gun> Reloaded;

        public int AllAmmoInMagazines => startMagazinesCount * startAmmoInMagazine;
        public int CurrentAmmoInMagazine { get; private set; }
        public int CurrentAmmo { get; private set; }
        public bool IsInfinityMagazines => isInfinityMagazines;


        private CancellationTokenSource _autoCts;
        private bool _isReloading;

        protected override void Awake()
        {
            base.Awake();
            CurrentAmmo = AllAmmoInMagazines;
            CurrentAmmoInMagazine = startAmmoInMagazine;
        }

        public override void OnAttackPerformed()
        {
            if (_isReloading)
                return;

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

        public void RefillAmmo()
        {
            CurrentAmmo = AllAmmoInMagazines;
            CurrentAmmoInMagazine = startAmmoInMagazine;
            Reloaded?.Invoke(this);
        }

        #region Reloading

        public async void Reload()
        {
            if (_isReloading)
                return;

            if (CurrentAmmo <= 0 && CurrentAmmoInMagazine > 0)
                return;

            if (CurrentAmmo <= 0 && CurrentAmmoInMagazine <= 0)
            {
                OutOfAmmo?.Invoke(this);
                return;
            }

            _isReloading = true;
            await UniTask.WaitForSeconds(reloadTime);

            CalculateAmmoAfterReload();
            _isReloading = false;

            Reloaded?.Invoke(this);
        }

        private void CalculateAmmoAfterReload()
        {
            int ammoToReload = startAmmoInMagazine - CurrentAmmoInMagazine;
            if (CurrentAmmo < ammoToReload)
                ammoToReload = CurrentAmmo;

            if (!IsInfinityMagazines)
            {
                CurrentAmmo -= ammoToReload;
                CurrentAmmo = Mathf.Clamp(CurrentAmmo, 0, AllAmmoInMagazines);
            }

            CurrentAmmoInMagazine += ammoToReload;
        }

        #endregion

        #region Shooting

        private void Shoot()
        {
            if (CurrentAmmoInMagazine <= 0)
            {
                Reload();
                return;
            }

            if (Physics.Raycast(shootPoint.position, shootPoint.forward, out var hit, 100))
            {
                var damageable = hit.collider.GetComponent<IDamageable>();
                damageable?.TakeDamage(damage, shootPoint.forward * shootForce);

                LineEffectAsync(shootPoint.position, hit.point);
            }
            else
                LineEffectAsync(shootPoint.position, shootPoint.position + shootPoint.forward * 100);

            // Debug.DrawRay(shootPoint.position, shootPoint.forward * 100, Color.red, 1f);
            CurrentAmmoInMagazine--;
            RaiseAttackEvent(this);
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

        #endregion
    }
}