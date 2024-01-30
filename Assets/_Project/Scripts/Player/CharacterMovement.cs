using gishadev.fort.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace gishadev.fort.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovement : MonoBehaviour
    {
        [Inject] private GameDataSO _gameDataSO;

        private CustomInput _customInput;
        private CharacterController _characterController;

        private Vector2 _input;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (_input.magnitude <= 0)
                return;

            var movementDirection = new Vector3(_input.x, 0f, _input.y);
            _characterController.Move(movementDirection * (_gameDataSO.MovementSpeed * Time.deltaTime));
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
    }
}