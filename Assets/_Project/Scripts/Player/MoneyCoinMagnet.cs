using Cysharp.Threading.Tasks;
using UnityEngine;

namespace gishadev.fort.Player
{
    public class MoneyCoinMagnet : MonoBehaviour
    {
        [SerializeField] private float magnetizeRadius = 5f;
        [SerializeField] private float checkInterval = 0.1f;

        private void Start() => MagnetCheckAsync();

        private async void MagnetCheckAsync()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, magnetizeRadius);
            foreach (Collider collider in colliders)
                if (collider.TryGetComponent(out Money.MoneyCoin moneyCoin) && !moneyCoin.IsMagnetized)
                    moneyCoin.JumpTo(transform);
            
            await UniTask.WaitForSeconds(checkInterval).ContinueWith(MagnetCheckAsync);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, magnetizeRadius);
        }
    }
}