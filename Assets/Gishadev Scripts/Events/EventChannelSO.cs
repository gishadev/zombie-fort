using System;
using UnityEngine;

namespace gishadev.tools.Events
{
    public abstract class EventChannelSO<T> : ScriptableObject, ISOInitializable where T : struct
    {
        [field: SerializeField] public T InitialValue { get; private set; }
        [field: SerializeField] public T Value { get; private set; }

        public event Action<T> ChangedValue;

        public virtual void ChangeValue(T newValue)
        {
            Value = newValue;
            ChangedValue?.Invoke(newValue);
        }

        public virtual void SilentChangeValue(T newValue)
        {
            Value = newValue;
        }

        public void Initialize()
        {
            Value = InitialValue;
        }
    }
}