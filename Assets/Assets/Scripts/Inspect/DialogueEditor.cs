# if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Dialogue))]
public class DialogueEditor : Editor
{
    Dialogue script;
    private void OnEnable()
    {
        script = (Dialogue)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Save as json", GUILayout.Height(35)))
        {
            script.SaveJson();
        }
    }
}
#endif