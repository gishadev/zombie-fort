using System;
using UnityEngine;

namespace gishadev.fort.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public abstract void OnAttackPerformed();
        public abstract void OnAttackCanceled();

        public static event Action<Weapon> Attack;

        protected virtual void Awake()
        {
        }

        protected static void RaiseAttackEvent(Weapon weapon) => Attack?.Invoke(weapon);
    }
}