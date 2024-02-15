using UnityEngine;

namespace gishadev.fort.Weapons
{
    public class GunMesh : MonoBehaviour
    {
        [field: SerializeField] public Transform ShootPoint { get; private set; }
    }
}