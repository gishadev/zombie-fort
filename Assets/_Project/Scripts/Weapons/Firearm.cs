using System;
using UnityEngine;

namespace gishadev.fort.Weapons
{
    public abstract class Firearm : MonoBehaviour
    {
        public abstract void OnAttackPerformed();
        public abstract void OnAttackCanceled();

        public static event Action Shot;

        protected virtual void Awake()
        {
        }

        protected virtual void Shoot()
        {
            Shot?.Invoke();
        }
    }
}