using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace gishadev.fort.World
{
    public class DemolishedFragmentsRoot : MonoBehaviour
    {
        [SerializeField] private float deactivateDuration = 2f;
        [SerializeField] private float deactivateDelay = 1f;

        private Rigidbody[] _fragments;

        private void Awake()
        {
            _fragments = GetComponentsInChildren<Rigidbody>();

            foreach (var fragment in _fragments)
            {
                fragment.gameObject.SetActive(false);
                fragment.isKinematic = true;
            }
        }

        public void Demolish() => Demolish(Vector3.zero);

        public async void Demolish(Vector3 hitForce)
        {
            transform.SetParent(null);
            foreach (var fragment in _fragments)
            {
                fragment.gameObject.SetActive(true);
                fragment.isKinematic = false;
                fragment.AddForce(hitForce, ForceMode.Impulse);

                DeactivateAsync(fragment.gameObject);
            }

            await UniTask.WaitForSeconds(deactivateDelay + deactivateDuration);
            Destroy(gameObject);
        }

        private async void DeactivateAsync(GameObject fragmentObj)
        {
            await UniTask.WaitForSeconds(deactivateDelay);
            fragmentObj.transform.DOScale(Vector3.zero, deactivateDuration);
        }

        [Button]
        private void Init()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
                var meshFilter = transform.GetChild(i).gameObject.GetComponent<MeshFilter>();

                var meshCollider = transform.GetChild(i).gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = true;
                meshCollider.sharedMesh = meshFilter.sharedMesh;
            }
        }
    }
}