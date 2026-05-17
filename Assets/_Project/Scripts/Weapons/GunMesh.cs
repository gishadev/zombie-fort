using Sirenix.OdinInspector;
using UnityEngine;

namespace gishadev.fort.Weapons
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(Collider))]
    public class GunMesh : MonoBehaviour
    {
        [field: SerializeField] public Transform ShootPoint { get; private set; }


#if UNITY_EDITOR
        [Button]
        private void CreateShootPoint()
        {
            if (ShootPoint != null)
                DestroyImmediate(ShootPoint.gameObject);

            var shootPoint = new GameObject("ShootPoint");
            shootPoint.transform.SetParent(transform);
            shootPoint.transform.localPosition = Vector3.zero;
            shootPoint.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.up);
            ShootPoint = shootPoint.transform;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(ShootPoint.position, 0.03f);
            Gizmos.DrawRay(ShootPoint.position, ShootPoint.forward * 2f);
        }
#endif
    }
}