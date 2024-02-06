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
        [SerializeField] private Firearm pistol, ak47;
        [SerializeField] private MMF_Player shootFeedback;

        private CustomInput _customInput;
        private Camera _cam;
        private Firearm _currentWeapon;

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
            Firearm.Shot += OnFirearmShot;
        }

        private void OnDisable()
        {
            _customInput.Character.MouseBodyRotation.performed -= OnMouseBodyRotationPerformed;
            _customInput.Character.Shoot.performed -= OnShootPerformed;
            _customInput.Character.Shoot.canceled -= OnShootCanceled;
            Firearm.Shot -= OnFirearmShot;

            _customInput.Disable();
        }

        private void SwitchWeapon(Firearm weapon)
        {
            if (_currentWeapon != null)
                _currentWeapon.gameObject.SetActive(false);

            _currentWeapon = weapon;
            _currentWeapon.gameObject.SetActive(true);
        }

        public void SwitchToAK() => SwitchWeapon(ak47);

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

        private void OnShootPerformed(InputAction.CallbackContext value) => _currentWeapon.OnAttackPerformed();
        private void OnShootCanceled(InputAction.CallbackContext value) => _currentWeapon.OnAttackCanceled();
        private void OnFirearmShot() => shootFeedback.PlayFeedbacks();

        private void RotateTowardsHit(Transform trans, Vector3 hitPoint, float angleOffset = 0f)
        {
            var direction = hitPoint - trans.position;
            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + angleOffset ;
            trans.rotation = Quaternion.AngleAxis(angle, Vector3.up);

            // Debug.DrawRay(trans.position, direction, Color.yellow, 1f);
        }
    }
}