using gishadev.tools.StateMachine;
using UnityEngine;

namespace gishadev.fort.Player.PlayerStates
{
    public class NoAutoAttack : IState
    {
        private readonly PlayerCharacterMovement _playerCharacterMovement;

        public NoAutoAttack(PlayerCharacterMovement playerCharacterMovement)
        {
            _playerCharacterMovement = playerCharacterMovement;
        }

        public void Tick()
        {
            if (_playerCharacterMovement.Input.magnitude <= 0f)
                return;

            var point = _playerCharacterMovement.transform.position;
            point.x += _playerCharacterMovement.Input.x;
            point.z += _playerCharacterMovement.Input.y;

            PlayerCharacterMovement.RotateTowards(_playerCharacterMovement.transform, point);
        }

        public void OnEnter()
        {
            Debug.Log("No Auto Attack");
        }

        public void OnExit()
        {
        }
    }
}