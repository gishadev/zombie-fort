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
        [SerializeField] private GunDataSO defaultGunData;
        [SerializeField] private MeleeDataSO defaultMeleeData;

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
            SwitchGun(defaultGunData);
            CurrentWeapon = EquippedGun;

            SwitchMelee(defaultMeleeData);
        }

        private void OnEnable()
        {
            _customInput = new CustomInput();
            _customInput.Enable();

            _customInput.Character.MouseBodyRotation.performed += OnMouseBodyRotationPerformed;
            _customInput.Character.Shoot.performed += OnShootPerformed;
            _customInput.Character.Shoot.canceled += OnShootCanceled;
            _customInput.Character.Reload.performed += OnReloadPerformed;

            _customInput.Character.Melee.performed += OnMeleePerformed;

            Weapon.Attack += OnFirearmAttack;
            Gun.OutOfAmmo += OnGunOutOfAmmo;
        }

        private void OnDisable()
        {
            _customInput.Character.MouseBodyRotation.performed -= OnMouseBodyRotationPerformed;
            _customInput.Character.Shoot.performed -= OnShootPerformed;
            _customInput.Character.Shoot.canceled -= OnShootCanceled;
            _customInput.Character.Reload.performed -= OnReloadPerformed;

            _customInput.Character.Melee.performed -= OnMeleePerformed;

            Weapon.Attack -= OnFirearmAttack;
            Gun.OutOfAmmo -= OnGunOutOfAmmo;

            _customInput.Disable();
        }

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


        
        private void OnMouseBodyRotationPerformed(InputAction.CallbackContext value)
        {
            var mousePosition = value.ReadValue<Vector2>();

            var ray = _cam.ScreenPointToRay(mousePosition);
            var plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out var planeHit))
            {
                var hitPoint = ray.GetPoint(planeHit);
                RotateTowardsHit(transform, hitPoint);
            }

            if (Physics.Raycast(ray, out var raycastHit))
            {
                if (raycastHit.collider.TryGetComponent(out IDamageable _))
                    RotateTowardsHit(hand, raycastHit.point, -90f);
                else
                {
                    var hitPoint = ray.GetPoint(planeHit);
                    RotateTowardsHit(hand, hitPoint, -90f);
                }
            }
        }

        private async void OnMeleePerformed(InputAction.CallbackContext value)
        {
            if (EquippedMelee.IsAttacking)
                return;

            CurrentWeapon = EquippedMelee;
            
            EquippedMelee.gameObject.SetActive(true);
            EquippedGun.gameObject.SetActive(false);

            _animator.enabled = true;
            _animator.SetTrigger(Constants.MELEE_SWING_TRIGGER_NAME);

            EquippedMelee.OnAttackPerformed();
        }
        
        public async void OnMeleeAttackFinished()
        {
            _animator.enabled = false;
            RestoreHandTransforms();

            CurrentWeapon = EquippedGun;
            EquippedMelee.gameObject.SetActive(false);
            EquippedGun.gameObject.SetActive(true);
            await UniTask.WaitForSeconds(EquippedMelee.MeleeDataSO.AttackDelay);
            EquippedMelee.OnAttackCanceled();
        }

        private void OnShootPerformed(InputAction.CallbackContext value)
        {
            if (CurrentWeapon != EquippedGun || EquippedGun.IsAttacking)
                return;

            EquippedGun.OnAttackPerformed();
        }

        private void OnShootCanceled(InputAction.CallbackContext value)
        {
            if (CurrentWeapon != EquippedGun)
                return;

            EquippedGun.OnAttackCanceled();
        }

        private void OnReloadPerformed(InputAction.CallbackContext obj)
        {
            if (CurrentWeapon != EquippedGun)
                return;

            EquippedGun.Reload();
        }

        private void OnFirearmAttack(Weapon weapon) => shootFeedback.PlayFeedbacks();
        private void OnGunOutOfAmmo(Gun gun) => SwitchGun(defaultGunData);

        private void RotateTowardsHit(Transform trans, Vector3 hitPoint, float angleOffset = 0f)
        {
            var direction = hitPoint - trans.position;
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