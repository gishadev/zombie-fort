using UnityEngine;

namespace gishadev.tools.Events
{
    [CreateAssetMenu(fileName = "FloatEventChannelSO", menuName = "ScriptableObjects/Events/FloatEventChannelSO")]
    public class FloatEventChannelSO : EventChannelSO<float>
    {
        [field: SerializeField] public float MaxValue { get; private set; }
        [field: SerializeField] public float MinValue { get; private set; }
    }
}