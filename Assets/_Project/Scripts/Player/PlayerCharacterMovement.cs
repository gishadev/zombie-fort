﻿using gishadev.fort.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace gishadev.fort.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerCharacterMovement : MonoBehaviour
    {
        [Inject] private GameDataSO _gameDataSO;

        private CustomInput _customInput;
        private Rigidbody _rb;

        private Vector2 _input;

        public Vector2 Input => _input;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var movementDirection = new Vector3(Input.x, 0f, Input.y);
            _rb.velocity = movementDirection * _gameDataSO.PlayerMovementSpeed;
        }

        private void OnEnable()
        {
            _customInput = new CustomInput();
            _customInput.Enable();
            _customInput.Character.Movement.performed += OnMovementPerformed;
            _customInput.Character.Movement.canceled += OnMovementCanceled;
        }

        private void OnDisable()
        {
            _customInput.Character.Movement.performed -= OnMovementPerformed;
            _customInput.Character.Movement.canceled -= OnMovementCanceled;
            _customInput.Disable();
        }

        private void OnMovementPerformed(InputAction.CallbackContext value)
        {
            _input = value.ReadValue<Vector2>();
        }

        private void OnMovementCanceled(InputAction.CallbackContext value)
        {
            _input = Vector2.zero;
        }
        
        public static void RotateTowards(Transform trans, Vector3 point, float angleOffset = 0f)
        {
            var direction = point - trans.position;
            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + angleOffset;
            trans.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
    }
}