using System.Collections.Generic;
using System.Linq;
using Gisha.Effects.Audio;
using gishadev.tools.Pooling;
using UnityEngine;

namespace gishadev.tools.Effects
{
    public class SFXEmitter : PoolManager<SFXPoolObject>, IPoolEmitter
    {
        public static SFXEmitter I
        {
            get
            {
                if (_current)
                    return _current;

                _current = new GameObject("[SFXEmitter]").AddComponent<SFXEmitter>();
                DontDestroyOnLoad(_current.gameObject);
                
                return _current;
            }
        }

        private static SFXEmitter _current;

        protected override Transform Parent => transform;
        protected override List<SFXPoolObject> PoolObjectsCollection => PoolDataSO.SFXPoolObjects.ToList();

        public GameObject EmitAt(int index, Vector3 position, Quaternion rotation)
        {
            if (!TryInstantiate(index, out var obj))
                return null;

            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.AddComponent<DisableSFXOnComplete>();

            return obj;
        }

        public GameObject EmitAt(SoundEffectsEnum enumEntry, Vector3 position, Quaternion rotation) =>
            EmitAt((int) enumEntry, position, rotation);
    }
}