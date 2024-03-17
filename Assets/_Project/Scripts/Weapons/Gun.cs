using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using gishadev.fort.Core;
using RayFire;
using UnityEngine;
using Random = UnityEngine.Random;

namespace gishadev.fort.Weapons
{
    [RequireComponent(typeof(RayfireGun))]
    public class Gun : Weapon
    {
        [SerializeField] private GameObject lineRendererPrefab;

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
        private LayerMask _nonPlayerLayers;
        private bool _isReloading;
        private LineRenderer[] _lineRenderers;

        protected override void Awake()
        {
            base.Awake();
            _rayfireGun = GetComponent<RayfireGun>();

            _lineCts = new CancellationTokenSource();
            _lineCts.RegisterRaiseCancelOnDestroy(gameObject);

            _nonPlayerLayers = ~(1 << LayerMask.NameToLayer("Player"));
        }

        public void SetupGun(GunDataSO gunDataSO, GunMesh gunMesh)
        {
            GunDataSO = gunDataSO;
            CurrentAmmo = AllAmmoInMagazines;
            CurrentAmmoInMagazine = GunDataSO.StartAmmoInMagazine;
            _gunMesh = gunMesh;
            InitLineRenderers();
        }


        public override void OnAttackPerformed(IAutoAttackable attackable)
        {
            if (_isReloading)
                return;

            Shoot(attackable);
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

        private void Shoot(IAutoAttackable attackable)
        {
            if (CurrentAmmoInMagazine <= 0)
            {
                Reload();
                return;
            }

            for (int i = 0; i < GunDataSO.ShootRaysCount; i++)
            {
                float randAccuracy = Random.Range(GunDataSO.MinAccuracy, GunDataSO.MaxAccuracy);
                Vector2 shootOffset = Random.insideUnitCircle * (GunDataSO.MaxShootOffset * (1 - randAccuracy));
                Vector3 direction = attackable.transform.position - ShootPoint.transform.position;
                var shootRay = new Ray(ShootPoint.position, direction + (Vector3) shootOffset);

                _rayfireGun.Shoot(shootRay.origin, shootRay.direction);
                if (Physics.Raycast(shootRay, out var hit, 100, _nonPlayerLayers))
                {
                    var damageable = hit.collider.GetComponent<IDamageable>();
                    damageable?.TakeDamage(GunDataSO.Damage, ShootPoint.forward * GunDataSO.ShootForce);

                    LineEffectAsync(i,shootRay.origin, hit.point);
                }
                else
                    LineEffectAsync(i,shootRay.origin, shootRay.origin + shootRay.direction * 100);
            }

            CurrentAmmoInMagazine--;
            RaiseAttackEvent(this);
        }

        private void InitLineRenderers()
        {
            _lineRenderers = new LineRenderer[GunDataSO.ShootRaysCount];
            for (int i = 0; i < GunDataSO.ShootRaysCount; i++)
            {
                var lineRenderer = Instantiate(lineRendererPrefab, transform).GetComponent<LineRenderer>();
                _lineRenderers[i] = lineRenderer;
            }
        }
        
        private async void LineEffectAsync(int index,Vector3 start, Vector3 end)
        {
            if (_lineCts.IsCancellationRequested)
                return;

            _lineRenderers[index].enabled = true;

            _lineRenderers[index].SetPosition(0, start);
            _lineRenderers[index].SetPosition(1, end);

            await UniTask.WaitForSeconds(0.05f, cancellationToken: _lineCts.Token).SuppressCancellationThrow();
            if (_lineCts.IsCancellationRequested)
                return;

            _lineRenderers[index].enabled = false;
        }

        #endregion
    }
}