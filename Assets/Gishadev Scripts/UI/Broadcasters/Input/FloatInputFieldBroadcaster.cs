using System.Text.RegularExpressions;
using gishadev.tools.Events;
using TMPro;
using UnityEngine;

namespace gishadev.tools.UI.Input
{
    [RequireComponent(typeof(TMP_InputField))]
    public class FloatInputFieldBroadcaster : InputFieldBroadcaster<float>
    {
        public override void RaiseEvent(object input)
        {
            float result = EventChannel.Value;
            if (string.IsNullOrEmpty((string) input))
                return;

            var match = Regex.Match((string) input, @"([-+]?[0-9]*\.?[0-9]+)");
            if (match.Success && float.TryParse(match.Groups[1].Value, out result))
            {
                result = Mathf.Clamp(result,  ((FloatEventChannelSO)EventChannel).MinValue, ((FloatEventChannelSO)EventChannel).MaxValue);
            }

            EventChannel.ChangeValue(result);
        }

        public override void OnValueChanged(object value)
        {
            tmpText.text = ((float)value).ToString();
        }
    }
}