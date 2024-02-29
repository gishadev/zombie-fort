using gishadev.tools.StateMachine;
using UnityEngine;

namespace gishadev.fort.Player.PlayerStates
{
    public class NoAutoAttack : IState
    {
        private readonly WeaponController _weaponController;

        public NoAutoAttack(WeaponController weaponController)
        {
            _weaponController = weaponController;
        }
        
        public void Tick()
        {
        }

        public void OnEnter()
        {
            Debug.Log("NO AUTO ATTACK ON");
        }

        public void OnExit()
        {
        }
    }
}