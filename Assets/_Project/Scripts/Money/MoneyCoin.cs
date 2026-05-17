using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using gishadev.fort.Money.Magnet;
using UnityEngine;

namespace gishadev.fort.Money
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoneyCoin : MonoBehaviour
    {
        [SerializeField] private int moneyToAdd = 5;
        [SerializeField] private float minIdleTimeBeforeJump = 0.5f;
        [SerializeField] private float maxIdleTimeBeforeJump = 1f;

        public bool IsMagnetized { get; private set; }

        public int MoneyToAdd => moneyToAdd;

        private Rigidbody _rb;
        private ICoinMagnet _nearestMagnet;
        private CancellationTokenSource _cts;

        private async void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _cts = new CancellationTokenSource();
            _cts.RegisterRaiseCancelOnDestroy(gameObject);

            var timeBeforeJump = Random.Range(minIdleTimeBeforeJump, maxIdleTimeBeforeJump);
            await UniTask.WaitForSeconds(timeBeforeJump, cancellationToken: _cts.Token)
                .SuppressCancellationThrow();
            if (_cts.IsCancellationRequested)
                return;

            _nearestMagnet = FindNearestMagnet();
            JumpTo(_nearestMagnet.transform);
        }

        public void JumpTo(Transform targetTrans)
        {
            _rb.isKinematic = true;
            // Follow until it reaches target trans
            transform
                .DOJump(targetTrans.position, 1f, 1, 0.3f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => FastJumpTo(targetTrans));
            IsMagnetized = true;
        }

        private void FastJumpTo(Transform targetTrans)
        {
            transform.DOMove(targetTrans.position, 0.1f)
                .OnComplete(() => Collect(_nearestMagnet));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICoinMagnet magnet))
                Collect(magnet);
        }

        private void Collect(ICoinMagnet magnet)
        {
            magnet.OnCoinCollect(this);
            DOTween.Kill(transform);
            Destroy(gameObject);
        }

        private ICoinMagnet FindNearestMagnet()
        {
            var magnets = FindObjectsOfType<MonoBehaviour>();
            ICoinMagnet nearestMagnet = null;
            float nearestDistance = float.MaxValue;
            foreach (var magnet in magnets)
            {
                if (magnet is ICoinMagnet coinMagnet)
                {
                    var distance = Vector3.Distance(transform.position, coinMagnet.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestMagnet = coinMagnet;
                    }
                }
            }

            return nearestMagnet;
        }
    }
}