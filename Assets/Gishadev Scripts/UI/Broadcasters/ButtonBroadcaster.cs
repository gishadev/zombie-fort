using gishadev.tools.Events;
using UnityEngine;
using UnityEngine.UI;

namespace gishadev.tools.UI
{
    public class ButtonBroadcaster : Broadcaster<Button>
    {
        [SerializeField] private string newValue;
        [SerializeField] private DefaultEventChannelSO eventChannelSo;

        public virtual void OnEnable()
        {
            _inputOrigin.onClick.AddListener(() => RaiseEvent(newValue));
        }

        public virtual void OnDisable()
        {
            _inputOrigin.onClick.RemoveAllListeners();
        }

        public override void RaiseEvent(object input)
        {
            eventChannelSo.ChangeValue(new StringWrapper(newValue));
        }

        public override void OnValueChanged(object value)
        {
        }
    }
}