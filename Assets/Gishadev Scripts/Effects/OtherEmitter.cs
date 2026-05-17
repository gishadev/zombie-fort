using System.Collections.Generic;
using System.Linq;
using gishadev.tools.Pooling;
using UnityEngine;

namespace gishadev.tools.Effects
{
    public class OtherEmitter : PoolManager<OtherPoolObject>, IPoolEmitter
    {
        public static OtherEmitter I
        {
            get
            {
                if (_current)
                    return _current;

                _current = new GameObject("[OtherEmitter]").AddComponent<OtherEmitter>();
                DontDestroyOnLoad(_current.gameObject);

                return _current;
            }
        }

        private static OtherEmitter _current;

        protected override Transform Parent => transform;
        protected override List<OtherPoolObject> PoolObjectsCollection => PoolDataSO.OtherPoolObjects.ToList();

        public GameObject EmitAt(int index, Vector3 position, Quaternion rotation)
        {
            if (!TryInstantiate(index, out var obj))
                return null;

            obj.transform.position = position;
            obj.transform.rotation = rotation;

            return obj;
        }

        public GameObject EmitAt(OtherPoolEnum enumEntry, Vector3 position, Quaternion rotation) =>
            EmitAt((int) enumEntry, position, rotation);

        public GameObject GetPrefab(OtherPoolEnum enumEntry) => PoolDataSO.OtherPoolObjects[(int)enumEntry].GetPrefab();
    }
}