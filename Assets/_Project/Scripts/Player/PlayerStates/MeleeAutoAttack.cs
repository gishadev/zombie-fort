using gishadev.tools.StateMachine;
using UnityEngine;

namespace gishadev.fort.Player.PlayerStates
{
    public class MeleeAutoAttack : IState
    {
        public void Tick()
        {
        }

        public void OnEnter()
        {
            Debug.Log("MELEE AUTO ATTACK ON");
        }

        public void OnExit()
        {
        }
    }
}