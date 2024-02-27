using gishadev.tools.StateMachine;
using UnityEngine;

namespace gishadev.fort.Player.PlayerStates
{
    public class FirearmAutoAttack : IState
    {
        public void Tick()
        {
        }

        public void OnEnter()
        {
            Debug.Log("FIREARM AUTO ATTACK ON");
        }

        public void OnExit()
        {
        }
    }
}