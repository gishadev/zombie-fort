using Cysharp.Threading.Tasks;
using gishadev.fort.Core;
using gishadev.fort.Weapons;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace gishadev.fort.Player
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private MeleeDataSO startMelee;

        [SerializeField] private Transform hand;
        [SerializeField] private MMF_Player shootFeedback;

        [Inject] private DiContainer _diContainer;
        [Inject] private GameDataSO _gameDataSO;
        public Gun EquippedGun { get; private set; }
        public Melee EquippedMelee { get; private set; }
        public Weapon CurrentWeapon { get; private set; }

        private Vector3 _cachedHandPosition;

        private GunDataSO _selectedGunData;
        private MeleeDataSO _selectedMeleeData;

        private CustomInput _customInput;
        private Camera _cam;
        private Animator _animator;

        private void Awake()
        {
            _cam = Camera.main;
            _animator = GetComponent<Animator>();
            _animator.enabled = false;
        }

        private void Start()
        {
            _cachedHandPosition = hand.localPosition;
            SwitchMelee(startMelee);
        }

        private void OnEnable()
        {
            _customInput = new CustomInput();
            _customInput.Enable();

            Weapon.Attack += OnFirearmAttack;
            // Gun.OutOfAmmo += OnGunOutOfAmmo;
        }

        private void OnDisable()
        {
            Weapon.Attack -= OnFirearmAttack;
            // Gun.OutOfAmmo -= OnGunOutOfAmmo;

            _customInput.Disable();
        }

        public void SwitchWeapon(WeaponDataSO weaponDataSO)
        {
            if (weaponDataSO is GunDataSO gunDataSO)
                SwitchGun(gunDataSO);
            else if (weaponDataSO is MeleeDataSO meleeDataSO)
                SwitchMelee(meleeDataSO);
        }

        public void RotateTowardsTarget(Transform target)
        {
            RotateTowards(transform, target.position);
            RotateTowards(hand, target.position, -90f);
        }

        #region Melee

        public void SwitchMelee(MeleeDataSO meleeDataSO)
        {
            if (EquippedMelee != null)
                Destroy(EquippedMelee.gameObject);

            var melee = _diContainer
                .InstantiatePrefab(_gameDataSO.MeleeCorePrefab, hand)
                .GetComponent<Melee>();

            var meleeMesh = _diContainer
                .InstantiatePrefab(meleeDataSO.WeaponMeshPrefab, melee.transform);
            meleeMesh.transform.localPosition = Vector3.zero;

            melee.SetupMelee(meleeDataSO);

            EquippedMelee = melee;
            EquippedMelee.gameObject.SetActive(false);
        }

        public void MeleeAttack(IDamageable damageable)
        {
            if (EquippedMelee.IsAttacking)
                return;

            CurrentWeapon = EquippedMelee;

            EquippedMelee.gameObject.SetActive(true);

            if (EquippedGun != null)
                EquippedGun.gameObject.SetActive(false);

            _animator.enabled = true;
            _animator.SetTrigger(Constants.MELEE_SWING_TRIGGER_NAME);

            EquippedMelee.OnAttackPerformed();
        }

        // For animation event.
        public async void OnMeleeAttackFinished()
        {
            _animator.enabled = false;
            RestoreHandTransforms();

            CurrentWeapon = EquippedGun;
            EquippedMelee.gameObject.SetActive(false);
            if (EquippedGun != null)
                EquippedGun.gameObject.SetActive(true);

            await UniTask.WaitForSeconds(EquippedMelee.MeleeDataSO.AttackDelay);
            EquippedMelee.OnAttackCanceled();
        }

        #endregion

        #region Firearm

        public void SwitchGun(GunDataSO gunDataSO)
        {
            if (EquippedGun != null)
                Destroy(EquippedGun.gameObject);

            var gun = _diContainer
                .InstantiatePrefab(_gameDataSO.GunCorePrefab, hand)
                .GetComponent<Gun>();

            var gunMesh = _diContainer
                .InstantiatePrefab(gunDataSO.WeaponMeshPrefab, gun.transform)
                .GetComponent<GunMesh>();
            gunMesh.transform.localPosition = Vector3.zero;

            gun.SetupGun(gunDataSO, gunMesh);
            gun.RefillAmmo();

            EquippedGun = gun;
        }

        public void FirearmAttack(IDamageable damageable)
        {
            if (EquippedGun == null)
                return;

            if (CurrentWeapon != EquippedGun || EquippedGun.IsAttacking)
                return;

            EquippedGun.OnAttackPerformed();
        }

        private void OnFirearmAttack(Weapon weapon) => shootFeedback.PlayFeedbacks();
        // private void OnGunOutOfAmmo(Gun gun) => SwitchGun(defaultGunData);

        public void FirearmCancel()
        {
            if (EquippedGun == null)
                return;

            if (CurrentWeapon != EquippedGun)
                return;

            EquippedGun.OnAttackCanceled();
        }

        private void OnReloadPerformed(InputAction.CallbackContext obj)
        {
            if (EquippedGun == null)
                return;

            if (CurrentWeapon != EquippedGun)
                return;

            EquippedGun.Reload();
        }

        #endregion

        private void RotateTowards(Transform trans, Vector3 point, float angleOffset = 0f)
        {
            var direction = point - trans.position;
            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + angleOffset;
            trans.rotation = Quaternion.AngleAxis(angle, Vector3.up);

            // Debug.DrawRay(trans.position, direction, Color.yellow, 1f);
        }

        private void RestoreHandTransforms()
        {
            hand.localPosition = _cachedHandPosition;
            hand.rotation = Quaternion.identity;
        }
    }
}