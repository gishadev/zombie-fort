﻿using UnityEngine;

namespace gishadev.fort.Core
{
    public static class Constants
    {
        public const string GROUND_LAYER_NAME = "Ground";
        public const string PLAYER_TAG_NAME = "Player";
        public const string ENEMY_TAG_NAME = "Enemy";
        public const string ATTACKABLE_TAG_NAME = "Attackable";
        
        public static int MELEE_SWING_TRIGGER_NAME = Animator.StringToHash("MeleeSwing");
    }
}