using Sirenix.OdinInspector;
using UnityEngine;

namespace gishadev.fort.Core
{
    [CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 0)]
    public class GameDataSO : ScriptableObject
    {
        [field: SerializeField] public float MovementSpeed { get; private set; } = 4f;
        
        [TabGroup("Prefabs")]
        [field: SerializeField] public GameObject MoneyPrefab { get; private set; }
    }
}