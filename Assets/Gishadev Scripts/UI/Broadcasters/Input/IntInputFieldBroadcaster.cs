using System.Text.RegularExpressions;
using gishadev.tools.Events;
using TMPro;
using UnityEngine;

namespace gishadev.tools.UI.Input
{
    [RequireComponent(typeof(TMP_InputField))]
    public class IntInputFieldBroadcaster : InputFieldBroadcaster<int>
    {
        public override void RaiseEvent(object input)
        {
            float result = EventChannel.Value;
            if (string.IsNullOrEmpty((string) input))
                return;

            var match = Regex.Match((string) input, @"([-+]?[0-9]*\.?[0-9]+)");
            if (match.Success && float.TryParse(match.Groups[1].Value, out result))
            {
                result = Mathf.Clamp(result,  ((IntEventChannelSO)EventChannel).MinValue, ((IntEventChannelSO)EventChannel).MaxValue);
            }

            EventChannel.ChangeValue((int)result);
        }

        public override void OnValueChanged(object value)
        {
            tmpText.text = ((int)value).ToString();
        }
    }
}