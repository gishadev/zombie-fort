using gishadev.tools.StateMachine;
using UnityEngine;

namespace gishadev.fort.Player.PlayerStates
{
    public class NoAutoAttack : IState
    {
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