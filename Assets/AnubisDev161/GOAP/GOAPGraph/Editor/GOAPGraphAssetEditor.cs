using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace GOAP.GOAPGraph.Editor
{
    [CustomEditor(typeof(GOAPGraphAsset))]
    public class GOAPGraphAssetEditor : UnityEditor.Editor
    {
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int index)
        {
            Object asset = EditorUtility.InstanceIDToObject(instanceId);
            if (asset.GetType() == typeof(GOAPGraphAsset))
            {
                GOAPGraphEditorWindow.Open((GOAPGraphAsset)asset);
                return true;
            }

            return false;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open"))
            {
                GOAPGraphEditorWindow.Open((GOAPGraphAsset)target);
            }
        }
    }
}