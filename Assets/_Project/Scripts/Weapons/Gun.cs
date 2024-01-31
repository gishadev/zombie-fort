using UnityEngine;

namespace gishadev.fort.Weapons
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Transform shootPoint;
        
        public void Shoot()
        {
            Debug.DrawRay(shootPoint.position, shootPoint.forward * 100, Color.red, 1f);
        }
    }
}