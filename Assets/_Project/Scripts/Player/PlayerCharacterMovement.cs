using System;
using gishadev.fort.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace gishadev.fort.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerCharacterMovement : MonoBehaviour
    {
        [Inject] private GameDataSO _gameDataSO;

        public Vector2 Input => _input;

        private CustomInput _customInput;
        private Vector2 _input;
        private Rigidbody _rb;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var movementDirection = new Vector3(Input.x, 0f, Input.y);
            _rb.velocity = movementDirection * _gameDataSO.PlayerMovementSpeed;
        }

        private void LateUpdate()
        {
            CalculateMovementBlendTree();
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

        private void CalculateMovementBlendTree()
        {
            var moveDir = new Vector3(_input.x, 0f, _input.y);
            var lookDir = transform.forward;
            
            float v = Mathf.Clamp(Vector3.Dot(moveDir, lookDir), -1f, 1f);
            Vector3 lookPerp = new Vector3(lookDir.z, 0f, -lookDir.x);
            float h = Mathf.Clamp(Vector3.Dot(moveDir, lookPerp), -1f, 1f);

            _animator.SetFloat(Constants.HASH_X_MOVEMENT, h);
            _animator.SetFloat(Constants.HASH_Y_MOVEMENT, v);
            _animator.SetFloat(Constants.HASH_MOVE_MAGNITUDE, _input.magnitude);
        }
    }
}