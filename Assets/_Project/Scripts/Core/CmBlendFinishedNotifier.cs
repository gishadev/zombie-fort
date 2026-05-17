using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using System;

namespace gishadev.fort.Core
{
    [RequireComponent(typeof(CinemachineVirtualCameraBase))]
    public class CmBlendFinishedNotifier : MonoBehaviour
    {
        private CinemachineVirtualCameraBase _vcamBase;

        [Serializable]
        public class BlendFinishedEvent : UnityEvent<CinemachineVirtualCameraBase>
        {
        }

        public BlendFinishedEvent OnBlendFinished = new();

        private void Start()
        {
            _vcamBase = GetComponent<CinemachineVirtualCameraBase>();
            ConnectToVcam(true);
            enabled = false;
        }

        private void ConnectToVcam(bool connect)
        {
            var vcam = _vcamBase as CinemachineVirtualCamera;
            if (vcam != null)
            {
                vcam.m_Transitions.m_OnCameraLive.RemoveListener(OnCameraLive);
                if (connect)
                    vcam.m_Transitions.m_OnCameraLive.AddListener(OnCameraLive);
            }

            var freeLook = _vcamBase as CinemachineFreeLook;
            if (freeLook != null)
            {
                freeLook.m_Transitions.m_OnCameraLive.RemoveListener(OnCameraLive);
                if (connect)
                    freeLook.m_Transitions.m_OnCameraLive.AddListener(OnCameraLive);
            }
        }

        private void OnCameraLive(ICinemachineCamera vcamIn, ICinemachineCamera vcamOut) => enabled = true;

        private void Update()
        {
            var brain = CinemachineCore.Instance.FindPotentialTargetBrain(_vcamBase);
            if (brain == null)
                enabled = false;
            else if (!brain.IsBlending)
            {
                if (brain.IsLive(_vcamBase))
                    OnBlendFinished?.Invoke(_vcamBase);
                enabled = false;
            }
        }
    }
}