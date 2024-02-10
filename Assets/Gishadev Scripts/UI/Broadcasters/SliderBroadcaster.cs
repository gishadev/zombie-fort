using gishadev.tools.Events;
using UnityEngine;
using UnityEngine.UI;

namespace gishadev.tools.UI
{
    [RequireComponent(typeof(Slider))]
    public class SliderBroadcaster : Broadcaster<Slider>
    {
        [SerializeField] private FloatEventChannelSO eventChannelSo;

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
            eventChannelSo.ChangeValue((float) input);
        }

        public override void OnValueChanged(object value)
        {
            _inputOrigin.SetValueWithoutNotify((float) value);
        }
    }
}