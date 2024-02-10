using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace gishadev.tools.Pooling
{
    public abstract class PoolManager<T> : MonoBehaviour where T : PoolObject, new()
    {
        public const string POOL_ASSET = "PoolDataSO";

        private Dictionary<IPoolObject, List<GameObject>> _objectsByPoolObject = new();
        private Dictionary<IPoolObject, Transform> _parentByPoolObject = new();

        protected abstract Transform Parent { get; }
        protected abstract List<T> PoolObjectsCollection { get; }

        protected PoolDataSO PoolDataSO { get; private set; }


        protected virtual void Awake()
        {
            PoolDataSO = Resources.Load<PoolDataSO>(POOL_ASSET);

            _objectsByPoolObject = new Dictionary<IPoolObject, List<GameObject>>();
            _parentByPoolObject = new Dictionary<IPoolObject, Transform>();

            InitializePools(PoolObjectsCollection);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        // Unload pools.
        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            foreach (var parent in _parentByPoolObject.Values) 
                Destroy(parent.gameObject);

            _objectsByPoolObject = new Dictionary<IPoolObject, List<GameObject>>();
            _parentByPoolObject = new Dictionary<IPoolObject, Transform>();

            InitializePools(PoolObjectsCollection);
        }

        protected bool TryInstantiate(int index, out GameObject emittedObj)
        {
            T[] collection;
            if (typeof(T) == typeof(SFXPoolObject))
                collection = PoolDataSO.SFXPoolObjects.Cast<T>().ToArray();
            else if (typeof(T) == typeof(VFXPoolObject))
                collection = PoolDataSO.VFXPoolObjects.Cast<T>().ToArray();
            else
                collection = PoolDataSO.OtherPoolObjects.Cast<T>().ToArray();

            var poolObj = collection[index];
            var prefab = poolObj.GetPrefab();
            emittedObj = null;

            if (poolObj.Equals(null))
                return false;

            var po = GetOrCreatePoolObject(prefab, collection.ToList());
            if (_objectsByPoolObject.TryGetValue(po, out var sceneObjectsList))
            {
                if (sceneObjectsList.Any(x => !x.activeInHierarchy))
                {
                    emittedObj = ActivateAvailableObject(sceneObjectsList);
                    return true;
                }
            }
            else
            {
                _objectsByPoolObject.Add(po, new List<GameObject>());
                CreateObjectParent(po);
            }

            emittedObj = InstantiateNewObject(prefab, po);
            return true;
        }

        private void InitializePools(List<T> poolObjects)
        {
            foreach (var po in poolObjects)
            {
                _objectsByPoolObject.Add(po, new List<GameObject>());
                CreateObjectParent(po);
            }
        }

        #region Object Instantiating

        private GameObject InstantiateNewObject(GameObject prefab,
            IPoolObject po)
        {
            Transform parent = _parentByPoolObject[po];

            GameObject createdObject = Instantiate(prefab, parent);
            _objectsByPoolObject[po].Add(createdObject);

            return createdObject;
        }

        private GameObject ActivateAvailableObject(List<GameObject> sceneObjectsList)
        {
            GameObject objectToActivate;
            if (sceneObjectsList.Count > 1)
            {
                List<GameObject> unactiveObjects = sceneObjectsList.Where(x => !x.activeInHierarchy).ToList();
                objectToActivate = unactiveObjects.ElementAtOrDefault(new Random().Next() % unactiveObjects.Count());
            }
            else
                objectToActivate = sceneObjectsList.FirstOrDefault(x => !x.activeInHierarchy);

            objectToActivate.SetActive(true);

            return objectToActivate;
        }

        #endregion

        private IPoolObject GetOrCreatePoolObject(GameObject prefab, List<T> poolCollection)
        {
            var prefabId = prefab.GetInstanceID();
            var index = poolCollection.FindIndex(x => x.InstanceIds.Contains(prefabId));

            if (index == -1)
            {
                var newPO = (T) Activator.CreateInstance(typeof(T), prefab);
                poolCollection.Add(newPO);
                index = poolCollection.Count - 1;

                Debug.LogFormat($"New Pool Object was created with name {newPO.Name}");
            }

            return poolCollection[index];
        }

        private void CreateObjectParent(IPoolObject poKey)
        {
            var name = $"[{poKey.GetType().Name}_{poKey.Name}]";
            var parent = new GameObject(name);
            parent.transform.SetParent(Parent);

            _parentByPoolObject.Add(poKey, parent.transform);
        }
    }
}