using gishadev.tools.Events;
using TMPro;
using UnityEngine;

namespace gishadev.tools.UI.Input
{
    [RequireComponent(typeof(TMP_InputField))]
    public class VectorInputFieldBroadcaster : InputFieldBroadcaster<Vector3>
    {
        [SerializeField] private VectorEventChannelSO eventChannelSo;
        [SerializeField] private bool writeX, writeY, writeZ;
        [SerializeField] private int minValue = 1, maxValue = 10;
        
        public override void RaiseEvent(object input)
        {
            if (float.TryParse((string)input, out var intValue))
                intValue = Mathf.Clamp(intValue, minValue, maxValue);

            Vector3 result = eventChannelSo.Value;
            if (writeX)
                result.x = intValue;
            if (writeY)
                result.y = intValue;    
            if (writeZ)
                result.z = intValue;
            
            eventChannelSo.ChangeValue(result);
        }

        public override void OnValueChanged(object value)
        {
            var val = (Vector3) value;
            _inputOrigin.SetTextWithoutNotify(val.ToString());
        }
    }
}