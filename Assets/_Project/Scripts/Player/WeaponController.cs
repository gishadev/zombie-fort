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

        private Vector3 _cachedHandPosition;

        private GunDataSO _selectedGunData;
        private MeleeDataSO _selectedMeleeData;

        private CustomInput _customInput;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _cachedHandPosition = hand.localPosition;
            SwitchWeapon(startMelee);
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

            _animator.SetInteger(Constants.HASH_WEAPON_STATE, (int) weaponDataSO.WeaponState);
        }

        public void RotateTowardsTarget(Transform target)
        {
            PlayerCharacterMovement.RotateTowards(transform, target.position);
            PlayerCharacterMovement.RotateTowards(hand, target.position, -90f);
        }
        
        public void SetAiming(bool isAiming) => _animator.SetBool(Constants.HASH_IS_AIMING, isAiming);

        #region Melee

        private void SwitchMelee(MeleeDataSO meleeDataSO)
        {
            if (EquippedMelee != null)
                Destroy(EquippedMelee.gameObject);

            var melee = _diContainer
                .InstantiatePrefab(_gameDataSO.MeleeCorePrefab, hand)
                .GetComponent<Melee>();

            var meleeMesh = _diContainer
                .InstantiatePrefab(meleeDataSO.WeaponMeshPrefab, melee.transform);
            
            meleeMesh.transform.localPosition = meleeDataSO.LocalHandPosition;
            meleeMesh.transform.localRotation = Quaternion.Euler(meleeDataSO.LocalHandEulerAngles);

            melee.SetupMelee(meleeDataSO);

            EquippedMelee = melee;
        }

        public void MeleeAttack(IAutoAttackable attackable)
        {
            if (EquippedMelee.IsAttacking)
                return;

            EquippedMelee.OnAttackPerformed(attackable);
            _animator.SetTrigger(Constants.HASH_ATTACK);

            Invoke(nameof(OnMeleeAttackFinished), GetAnimationDuration());
        }

        // For animation event.
        public async void OnMeleeAttackFinished()
        {
            RestoreHandTransforms();

            await UniTask.WaitForSeconds(EquippedMelee.MeleeDataSO.AttackDelay);
            EquippedMelee.OnAttackCanceled();
        }

        #endregion

        #region Firearm

        private void SwitchGun(GunDataSO gunDataSO)
        {
            if (EquippedGun != null)
                Destroy(EquippedGun.gameObject);

            var gun = _diContainer
                .InstantiatePrefab(_gameDataSO.GunCorePrefab, hand)
                .GetComponent<Gun>();

            var gunMesh = _diContainer
                .InstantiatePrefab(gunDataSO.WeaponMeshPrefab, gun.transform)
                .GetComponent<GunMesh>();

            gunMesh.transform.localPosition = gunDataSO.LocalHandPosition;
            gunMesh.transform.localRotation = Quaternion.Euler(gunDataSO.LocalHandEulerAngles);

            gun.SetupGun(gunDataSO, gunMesh);
            gun.RefillAmmo();

            EquippedGun = gun;
        }

        public void FirearmAttack(IAutoAttackable attackable)
        {
            if (EquippedGun == null || EquippedGun.IsAttacking)
                return;

            EquippedGun.OnAttackPerformed(attackable);
            _animator.SetTrigger(Constants.HASH_ATTACK);
        }

        private void OnFirearmAttack(Weapon weapon) => shootFeedback.PlayFeedbacks();
        // private void OnGunOutOfAmmo(Gun gun) => SwitchGun(defaultGunData);

        public void FirearmCancel()
        {
            if (EquippedGun == null)
                return;

            EquippedGun.OnAttackCanceled();
        }

        private void OnReloadPerformed(InputAction.CallbackContext obj)
        {
            if (EquippedGun == null)
                return;

            EquippedGun.Reload();
        }

        #endregion


        private void RestoreHandTransforms()
        {
            hand.localPosition = _cachedHandPosition;
            hand.rotation = Quaternion.identity;
        }

        private float GetAnimationDuration()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.length;
        }
    }
}