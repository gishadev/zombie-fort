using DG.Tweening;
using gishadev.fort.Core;
using UnityEngine;
using Zenject;

namespace gishadev.fort.Money
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoneyCoin : MonoBehaviour
    {
        [SerializeField] private int moneyToAdd = 5;

        [Inject] private IMoneyController _moneyController;

        public bool IsMagnetized { get; private set; }

        private Rigidbody _rb;

        private void Awake() => _rb = GetComponent<Rigidbody>();

        public void JumpTo(Transform targetTrans)
        {
            _rb.isKinematic = true;
            // Follow until it reaches target trans
            transform.DOJump(targetTrans.position, 1f, 1, 0.3f).SetEase(Ease.OutCubic)
                .OnComplete(() => FastJumpTo(targetTrans));
            IsMagnetized = true;
        }

        private void FastJumpTo(Transform targetTrans)
        {
            transform.DOMove(targetTrans.position, 0.1f).OnComplete(Collect);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.PLAYER_TAG_NAME))
                Collect();
        }

        private void Collect()
        {
            DOTween.Kill(transform);
            _moneyController.AddMoney(moneyToAdd);
            Destroy(gameObject);
        }
    }
}