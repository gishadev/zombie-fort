using gishadev.tools.Events;
using UnityEngine;
using UnityEngine.UI;

namespace gishadev.tools.UI
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleBroadcaster : Broadcaster<Toggle>
    {
        [SerializeField] private BoolEventChannelSO eventChannelSo;


        private void OnEnable()
        {
            _inputOrigin.onValueChanged.AddListener(value => RaiseEvent(value));
            eventChannelSo.ChangedValue += value => OnValueChanged(value);
            OnValueChanged(eventChannelSo.Value);
        }

        private void OnDisable()
        {
            _inputOrigin.onValueChanged.RemoveListener(value => RaiseEvent(value));
            eventChannelSo.ChangedValue -= value => OnValueChanged(value);
        }

        public override void RaiseEvent(object input)
        {
            eventChannelSo.ChangeValue((bool) input);
        }

        public override void OnValueChanged(object value)
        {
            _inputOrigin.SetIsOnWithoutNotify((bool) value);
        }
    }
}