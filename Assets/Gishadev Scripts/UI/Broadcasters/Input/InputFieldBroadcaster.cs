using gishadev.tools.Events;
using TMPro;
using UnityEngine;

namespace gishadev.tools.UI.Input
{
    [RequireComponent(typeof(TMP_InputField))]
    public abstract class InputFieldBroadcaster<T> : Broadcaster<TMP_InputField> where T : struct
    {
        [field: SerializeField] protected TMP_Text tmpText { get; private set; }
        [field: SerializeField] protected EventChannelSO<T> EventChannel { get; private set; }

        protected virtual void Start()
        {
            OnValueChanged(EventChannel.Value);
        }

        protected virtual void OnEnable()
        {
            _inputOrigin.onSubmit.AddListener(RaiseEvent);
            _inputOrigin.onEndEdit.AddListener(RaiseEvent);
            EventChannel.ChangedValue += value => OnValueChanged(value);
        }

        protected virtual void OnDisable()
        {
            _inputOrigin.onSubmit.RemoveListener(RaiseEvent);
            _inputOrigin.onEndEdit.RemoveListener(RaiseEvent);
            EventChannel.ChangedValue -= value => OnValueChanged(value);
        }

        private void OnStringChanged(string value)
        {
            _inputOrigin.SetTextWithoutNotify(value);
        }

        public abstract override void RaiseEvent(object input);
        public abstract override void OnValueChanged(object value);
    }
}