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
        [SerializeField] private Gun gun;
        [SerializeField] private MMF_Player shootFeedback;

        private CustomInput _customInput;
        private Camera _cam;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void OnEnable()
        {
            _customInput = new CustomInput();
            _customInput.Enable();
            _customInput.Character.MouseBodyRotation.performed += OnMouseBodyRotationPerformed;
            _customInput.Character.Shoot.performed += OnShootPerformed;
        }

        private void OnDisable()
        {
            _customInput.Character.MouseBodyRotation.performed -= OnMouseBodyRotationPerformed;
            _customInput.Character.Shoot.performed -= OnShootPerformed;
            _customInput.Disable();
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
                    RotateTowardsHit(hand, raycastHit.point);
                else
                {
                    var hitPoint = ray.GetPoint(planeHit);
                    RotateTowardsHit(hand, hitPoint);
                }
            }
        }

        private void OnShootPerformed(InputAction.CallbackContext value)
        {
            gun.Shoot();
            shootFeedback.PlayFeedbacks();
        }

        private void RotateTowardsHit(Transform trans, Vector3 hitPoint)
        {
            var direction = hitPoint - trans.position;
            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            trans.rotation = Quaternion.AngleAxis(angle, Vector3.up);

            // Debug.DrawRay(trans.position, direction, Color.yellow, 1f);
        }
    }
}