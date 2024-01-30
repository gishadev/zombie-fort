using UnityEngine;

namespace gishadev.fort.Core
{
    [CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 0)]
    public class GameDataSO : ScriptableObject
    {
        [field: SerializeField] public float MovementSpeed { get; private set; } = 4f;
    }
}