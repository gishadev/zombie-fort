using gishadev.tools.Core;
using gishadev.tools.Audio;
using gishadev.tools.Pooling;
using UnityEditor;
using UnityEngine;

namespace gishadev.tools.editor
{
    [CustomEditor(typeof(AudioMasterSO))]
    public class AudioEditor : Editor
    {
        private AudioMasterSO _audioMaster;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _audioMaster = (AudioMasterSO) target;
            EditorGUILayout.Space();
            EditorDropAreaCreator<MusicData, AudioClip>.Create(_audioMaster, _audioMaster.MusicCollection);
            EditorGUILayout.Space();
            EditorDropAreaCreator<SFXData, AudioClip>.Create(_audioMaster, _audioMaster.SFXCollection);

            if (GUILayout.Button("Generate Enums"))
            {
                var enumsGen = (ScriptableObjectEnumsGenerator) target;
                enumsGen.OnCollectionChanged();
            }
        }
    }

    [CustomEditor(typeof(PoolDataSO))]
    public class PoolEditor : Editor
    {
        private PoolDataSO _poolData;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _poolData = (PoolDataSO) target;
            EditorGUILayout.Space();
            EditorDropAreaCreator<SFXPoolObject, GameObject>.Create(_poolData, _poolData.SFXPoolObjects);
            EditorGUILayout.Space();
            EditorDropAreaCreator<VFXPoolObject, GameObject>.Create(_poolData, _poolData.VFXPoolObjects);
            EditorGUILayout.Space();
            EditorDropAreaCreator<OtherPoolObject, GameObject>.Create(_poolData, _poolData.OtherPoolObjects);
            
            if (GUILayout.Button("Generate Enums"))
            {
                var enumsGen = (ScriptableObjectEnumsGenerator) target;
                enumsGen.OnCollectionChanged();
            }
            
            EditorUtility.SetDirty(_poolData);
        }
    }
}