using gishadev.tools.Events;
using UnityEngine;
using UnityEngine.UI;

namespace gishadev.tools.UI
{
    public class VectorIncrementButtonBroadcaster : Broadcaster<Button>
    {
        [SerializeField] private VectorEventChannelSO vectorEventChannel;
        [SerializeField] private float incrementValue = 1f;
        [SerializeField] private bool useIntegers;

        [Header("Constraints")]
        [SerializeField] private bool incrementX;
        [SerializeField] private bool incrementY;
        [SerializeField] private bool incrementZ;


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
            var value = vectorEventChannel.Value;
            var increment = useIntegers ? (int) incrementValue : incrementValue;

            if (incrementX)
                value.x += increment;
            if (incrementY)
                value.y += increment;
            if (incrementZ)
                value.z += increment;

            RaiseEvent(value);
        }

        public override void RaiseEvent(object input)
        {
            vectorEventChannel.ChangeValue((Vector3) input);
        }

        public override void OnValueChanged(object value)
        {
        }
    }
}