#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraConfine))]
public class CameraConfineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Regenerate Composite Collider"))
        {
            ((CameraConfine)target).RegenerateCollider();
        }
    }
}
#endif