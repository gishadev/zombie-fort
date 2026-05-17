using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using gishadev.fort.Core;
using UnityEngine;
using UnityEngine.AI;

namespace gishadev.fort.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float knockBackDelay = 0.2f;

        public float DistanceToDestination => _agent.remainingDistance;
        private NavMeshAgent _agent;
        private Rigidbody _rb;
        private Animator _animator;

        private CancellationTokenSource _destroyCts;
        private bool _isKnockBacking;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();

            _destroyCts = new CancellationTokenSource();
            _destroyCts.RegisterRaiseCancelOnDestroy(gameObject);

            Resume();
        }

        private void LateUpdate()
        {
            _animator.SetBool(Constants.HASH_IS_WALKING, !_agent.isStopped);
        }

        public void SetDestination(Vector3 target)
        {
            if (_isKnockBacking)
                return;

            _agent.SetDestination(target);
            Resume();
        }

        public void Stop()
        {
            _agent.isStopped = true;
            _rb.isKinematic = true;
        }

        private void Resume()
        {
            _rb.isKinematic = true;
            _agent.isStopped = false;
            _isKnockBacking = false;
        }

        public async void KnockBack(Vector3 knockVelocity)
        {
            if (_isKnockBacking || _destroyCts.Token.IsCancellationRequested)
                return;

            _agent.isStopped = true;
            _rb.isKinematic = false;
            _isKnockBacking = true;

            _rb.AddForce(knockVelocity, ForceMode.Impulse);
            await UniTask
                .WaitForSeconds(knockBackDelay, cancellationToken: _destroyCts.Token)
                .SuppressCancellationThrow();

            if (_destroyCts.Token.IsCancellationRequested)
                return;

            Resume();
        }
    }
}