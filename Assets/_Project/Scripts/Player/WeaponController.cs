using gishadev.fort.Weapons;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace gishadev.fort.Player
{
    public class WeaponController : MonoBehaviour
    {
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
            if (Physics.Raycast(ray, out var hit))
            {
                var direction = hit.point - transform.position;
                RotateBody(direction);
            }
        }

        private void OnShootPerformed(InputAction.CallbackContext value)
        {
            gun.Shoot();
            shootFeedback.PlayFeedbacks();
        }

        private void RotateBody(Vector3 direction)
        {
            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
    }
}