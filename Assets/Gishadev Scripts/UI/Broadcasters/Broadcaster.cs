using UnityEngine;
using UnityEngine.UI;

namespace gishadev.tools.UI
{
    public abstract class Broadcaster<T> : MonoBehaviour where T : Selectable
    {
        protected T _inputOrigin;

        public virtual void Awake()
        {
            _inputOrigin = GetComponent<T>();
        }

        public abstract void RaiseEvent(object input);
        public abstract void OnValueChanged(object value);
    }
}