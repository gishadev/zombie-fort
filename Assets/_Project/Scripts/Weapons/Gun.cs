﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using gishadev.fort.Core;
using RayFire;
using UnityEngine;

namespace gishadev.fort.Weapons
{
    [RequireComponent(typeof(RayfireGun))]
    public class Gun : Weapon
    {
        [SerializeField] private LineRenderer lineRenderer;

        public static event Action<Gun> OutOfAmmo;
        public static event Action<Gun> Reloaded;

        public override bool IsAttacking { get; protected set; }
        public GunDataSO GunDataSO { get; private set; }
        public int CurrentAmmoInMagazine { get; private set; }
        public int CurrentAmmo { get; private set; }
        public int AllAmmoInMagazines => GunDataSO.StartMagazinesCount * GunDataSO.StartAmmoInMagazine;
        public Transform ShootPoint => _gunMesh.ShootPoint;

        private RayfireGun _rayfireGun;
        private GunMesh _gunMesh;
        private CancellationTokenSource _lineCts;
        private bool _isReloading;

        protected override void Awake()
        {
            base.Awake();
            _rayfireGun = GetComponent<RayfireGun>();
            
            _lineCts = new CancellationTokenSource();
            _lineCts.RegisterRaiseCancelOnDestroy(gameObject);
        }

        public void SetupGun(GunDataSO gunDataSO, GunMesh gunMesh)
        {
            GunDataSO = gunDataSO;
            CurrentAmmo = AllAmmoInMagazines;
            CurrentAmmoInMagazine = GunDataSO.StartAmmoInMagazine;
            _gunMesh = gunMesh;
        }


        public override void OnAttackPerformed()
        {
            if (_isReloading)
                return;

            Shoot();
        }

        public override void OnAttackCanceled()
        {
        }

        public void RefillAmmo()
        {
            CurrentAmmo = AllAmmoInMagazines;
            CurrentAmmoInMagazine = GunDataSO.StartAmmoInMagazine;
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
            await UniTask.WaitForSeconds(GunDataSO.ReloadTime);

            CalculateAmmoAfterReload();
            _isReloading = false;

            Reloaded?.Invoke(this);
        }

        private void CalculateAmmoAfterReload()
        {
            int ammoToReload = GunDataSO.StartAmmoInMagazine - CurrentAmmoInMagazine;
            if (CurrentAmmo < ammoToReload)
                ammoToReload = CurrentAmmo;

            if (!GunDataSO.IsInfinityMagazines)
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

            _rayfireGun.Shoot(ShootPoint.position, ShootPoint.forward);
            if (Physics.Raycast(ShootPoint.position, ShootPoint.forward, out var hit, 100))
            {
                var damageable = hit.collider.GetComponent<IDamageable>();
                damageable?.TakeDamage(GunDataSO.Damage, ShootPoint.forward * GunDataSO.ShootForce);

                LineEffectAsync(ShootPoint.position, hit.point);
            }
            else
                LineEffectAsync(ShootPoint.position, ShootPoint.position + ShootPoint.forward * 100);

            // Debug.DrawRay(shootPoint.position, shootPoint.forward * 100, Color.red, 1f);
            CurrentAmmoInMagazine--;
            RaiseAttackEvent(this);
        }

        private async void LineEffectAsync(Vector3 start, Vector3 end)
        {
            if (_lineCts.IsCancellationRequested)
                return;

            lineRenderer.enabled = true;

            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);

            await UniTask.WaitForSeconds(0.05f, cancellationToken: _lineCts.Token).SuppressCancellationThrow();
            if (_lineCts.IsCancellationRequested)
                return;

            lineRenderer.enabled = false;
        }

        #endregion
    }
}