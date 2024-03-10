﻿using System;
using UnityEngine;

namespace gishadev.fort.Core
{
    public interface IDamageable
    {
        int Health { get; }
        void TakeDamage(int damage, Vector3 hitForce);
        event Action<int> HealthChanged;
    }
    
    public interface IAutoAttackable : IDamageable
    {
        Transform transform { get; }
        GameObject gameObject { get; }
    }
}