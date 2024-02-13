using Sirenix.OdinInspector;
using UnityEngine;

namespace gishadev.fort.Core
{
    [CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 0)]
    public class GameDataSO : ScriptableObject
    {
        [SerializeField] private int playerMaxHealth = 100;
        [SerializeField] private float playerMovementSpeed = 4f;
        [SerializeField] private float enemySpawnDelay = 0.5f;
        [SerializeField] private float waveDelay = 3f;

        [TabGroup("Prefabs")] [SerializeField] private GameObject moneyPrefab;
        [TabGroup("Prefabs")] [SerializeField] private GameObject enemyPrefab;
        [TabGroup("Prefabs")] [SerializeField] private GameObject weaponGiverPrefab;

        public GameObject MoneyPrefab => moneyPrefab;
        public GameObject EnemyPrefab => enemyPrefab;
        public float WaveDelay => waveDelay;
        public float EnemySpawnDelay => enemySpawnDelay;
        public float PlayerMovementSpeed => playerMovementSpeed;
        public int PlayerMaxHealth => playerMaxHealth;

        public GameObject WeaponGiverPrefab => weaponGiverPrefab;
    }
}