using UnityEngine;

namespace gishadev.fort.Core
{
    public static class Constants
    {
        public const string LAYER_NAME_GROUND = "Ground";
        public const string TAG_NAME_PLAYER = "Player";
        public const string TAG_NAME_ENEMY = "Enemy";
        public const string TAG_NAME_ATTACKABLE = "Attackable";

        public static readonly int HASH_X_MOVEMENT = Animator.StringToHash("XMovement");
        public static readonly int HASH_Y_MOVEMENT = Animator.StringToHash("YMovement");
        public static readonly int HASH_MOVE_MAGNITUDE = Animator.StringToHash("MovementMagnitude");
        public static readonly int HASH_ATTACK = Animator.StringToHash("Attack");
        public static readonly int HASH_WEAPON_STATE = Animator.StringToHash("WeaponState");
        public static readonly int HASH_IS_AIMING = Animator.StringToHash("IsAiming");
    }
}