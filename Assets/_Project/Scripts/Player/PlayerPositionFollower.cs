using UnityEngine;

namespace gishadev.fort.Player
{
    public class PlayerPositionFollower : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;

        private void FixedUpdate()
        {
            var position = playerTransform.position;
            transform.position = position;
        }
    }
}