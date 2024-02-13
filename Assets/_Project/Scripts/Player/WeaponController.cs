using System;
using gishadev.fort.Core;
using gishadev.fort.Weapons;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace gishadev.fort.Player
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Transform hand;
        [SerializeField] private Gun pistol, ak47;
        [SerializeField] private MMF_Player shootFeedback;

        private CustomInput _customInput;
        private Camera _cam;
        public Weapon CurrentWeapon { get; private set; }

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Start()
        {
            SwitchWeapon(pistol);
        }

        private void OnEnable()
        {
            _customInput = new CustomInput();
            _customInput.Enable();

            _customInput.Character.MouseBodyRotation.performed += OnMouseBodyRotationPerformed;
            _customInput.Character.Shoot.performed += OnShootPerformed;
            _customInput.Character.Shoot.canceled += OnShootCanceled;
            _customInput.Character.Reload.performed += OnReloadPerformed;

            Weapon.Attack += OnFirearmAttack;
            Gun.OutOfAmmo += OnGunOutOfAmmo;
        }


        private void OnDisable()
        {
            _customInput.Character.MouseBodyRotation.performed -= OnMouseBodyRotationPerformed;
            _customInput.Character.Shoot.performed -= OnShootPerformed;
            _customInput.Character.Shoot.canceled -= OnShootCanceled;
            _customInput.Character.Reload.performed -= OnReloadPerformed;

            Weapon.Attack -= OnFirearmAttack;
            Gun.OutOfAmmo -= OnGunOutOfAmmo;

            _customInput.Disable();
        }

        private void SwitchWeapon(Weapon weapon)
        {
            if (CurrentWeapon != null)
                CurrentWeapon.gameObject.SetActive(false);

            CurrentWeapon = weapon;
            CurrentWeapon.gameObject.SetActive(true);
        }

        public void SwitchToAK()
        {
            ak47.RefillAmmo();
            SwitchWeapon(ak47);
        }

        public void SwitchToPistol()
        {
            pistol.RefillAmmo();
            SwitchWeapon(pistol);
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

        private void OnShootPerformed(InputAction.CallbackContext value) => CurrentWeapon.OnAttackPerformed();
        private void OnShootCanceled(InputAction.CallbackContext value) => CurrentWeapon.OnAttackCanceled();
        private void OnFirearmAttack(Weapon weapon) => shootFeedback.PlayFeedbacks();

        private void OnReloadPerformed(InputAction.CallbackContext obj)
        {
            if (CurrentWeapon is Gun gun)
                gun.Reload();
        }

        private void OnGunOutOfAmmo(Gun gun) => SwitchToPistol();

        private void RotateTowardsHit(Transform trans, Vector3 hitPoint, float angleOffset = 0f)
        {
            var direction = hitPoint - trans.position;
            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + angleOffset;
            trans.rotation = Quaternion.AngleAxis(angle, Vector3.up);

            // Debug.DrawRay(trans.position, direction, Color.yellow, 1f);
        }
    }
}