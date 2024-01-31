using System;
using gishadev.fort.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace gishadev.fort.Player
{
    public class WeaponController : MonoBehaviour
    {
        private CustomInput _customInput;
        private LayerMask _groundMask;
        private Camera _cam;

        private void Awake()
        {
            _groundMask = LayerMask.GetMask(Constants.GROUND_LAYER_NAME);
            _cam = Camera.main;
        }

        private void OnEnable()
        {
            _customInput = new CustomInput();
            _customInput.Enable();
            _customInput.Character.MouseBodyRotation.performed += OnMouseBodyRotation;
        }

        private void OnDisable()
        {
            _customInput.Character.MouseBodyRotation.performed -= OnMouseBodyRotation;
            _customInput.Disable();
        }

        private void OnMouseBodyRotation(InputAction.CallbackContext value)
        {
            var mousePosition = value.ReadValue<Vector2>();

            var ray = _cam.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                var direction = hit.point - transform.position;
                RotateBody(direction);
            }
        }

        private void RotateBody(Vector3 direction)
        {
            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
    }
}