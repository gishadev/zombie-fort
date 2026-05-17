using UnityEngine;

namespace gishadev.tools.Events
{
    [CreateAssetMenu(fileName = "VoidEventChannelSO", menuName = "ScriptableObjects/Events/VoidEventChannelSO")]
    public class DefaultEventChannelSO : EventChannelSO<StringWrapper>
    {
    }
    
    [System.Serializable]
    public struct StringWrapper
    {
        public string value;

        public StringWrapper(string value)
        {
            this.value = value;
        }
    }
}