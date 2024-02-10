using UnityEngine;

namespace gishadev.tools.Events
{
    [CreateAssetMenu(fileName = "IntEventChannelSO", menuName = "ScriptableObjects/Events/IntEventChannelSO")]
    public class IntEventChannelSO : EventChannelSO<int>
    {
        [field: SerializeField] public int MaxValue { get; private set; }
        [field: SerializeField] public int MinValue { get; private set; }
    }
}