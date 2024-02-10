using Sirenix.OdinInspector;
using UnityEngine;

namespace gishadev.fort.Core
{
    [CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 0)]
    public class GameDataSO : ScriptableObject
    {
        [field: SerializeField] public int PlayerMaxHealth { get; private set; } = 100;
        [field: SerializeField] public float PlayerMovementSpeed { get; private set; } = 4f;
        [field: SerializeField] public float EnemySpawnDelay { get; private set; } = 0.5f;
        [field: SerializeField] public float WaveDelay { get; private set; } = 3f;
        
        [TabGroup("Prefabs")]
        [field: SerializeField] public GameObject MoneyPrefab { get; private set; }
        [TabGroup("Prefabs")]
        [field: SerializeField] public GameObject EnemyPrefab { get; private set; }
    }
}