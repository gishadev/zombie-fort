using gishadev.tools.Events;
using UnityEngine;
using UnityEngine.UI;

namespace gishadev.tools.UI
{
    public class IntIncrementButtonBroadcaster : Broadcaster<Button>
    {
        [SerializeField] private IntEventChannelSO intEventChannel;
        [SerializeField] private int incrementValue = 1;

        public virtual void OnEnable()
        {
            _inputOrigin.onClick.AddListener(OnClick);
        }

        public virtual void OnDisable()
        {
            _inputOrigin.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            var value = intEventChannel.Value;
            value += incrementValue;
            value = Mathf.Clamp(value, intEventChannel.MinValue, intEventChannel.MaxValue);
            
            RaiseEvent(value);
        }

        public override void RaiseEvent(object input)
        {
            intEventChannel.ChangeValue((int) input);
        }

        public override void OnValueChanged(object value)
        {
        }
    }
}